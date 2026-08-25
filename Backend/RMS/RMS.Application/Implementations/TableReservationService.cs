using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Infrastructure.Persistences;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Implementations
{
    public class TableReservationService : ITableReservationService
    {
        private readonly RestaurantDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly ILogger<TableReservationService> _logger;
        private readonly MassTransit.IPublishEndpoint? _publishEndpoint;

        public TableReservationService(
            RestaurantDbContext context,
            IDistributedCache cache,
            ILogger<TableReservationService> logger,
            MassTransit.IPublishEndpoint? publishEndpoint = null)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<ResponseDto<TableReservationDto>> CreateHoldAsync(CreateReservationHoldDto dto, string createdBy, CancellationToken cancellationToken = default)
        {
            // AC-2: Validate dining table existence
            var tableExists = await _context.DiningTables.AnyAsync(t => t.TableID == dto.DiningTableId && !t.IsDeleted, cancellationToken);
            if (!tableExists)
            {
                return new ResponseDto<TableReservationDto>
                {
                    IsSuccess = false,
                    Message = "Dining table not found."
                };
            }

            var lockKey = $"reservation_lock:{dto.DiningTableId}:{dto.ReservationStartTime:yyyyMMddHHmm}";

            // AC-3: Redis Distributed Lock Check
            var existingLock = await _cache.GetStringAsync(lockKey, cancellationToken);
            if (!string.IsNullOrEmpty(existingLock))
            {
                _logger.LogWarning("Concurrent reservation hold rejected for Table {TableId} at {Time}", dto.DiningTableId, dto.ReservationStartTime);
                return new ResponseDto<TableReservationDto>
                {
                    IsSuccess = false,
                    Message = "Table is currently locked by another customer. Please select another slot or table."
                };
            }

            // DB Overlap Check for active reservations
            var hasOverlap = await _context.TableReservations
                .AsNoTracking()
                .AnyAsync(r => r.DiningTableId == dto.DiningTableId
                            && (r.ReservationStatus == "PendingPayment" || r.ReservationStatus == "Confirmed")
                            && r.ReservationStartTime < dto.ReservationEndTime
                            && r.ReservationEndTime > dto.ReservationStartTime
                            && !r.IsDeleted, cancellationToken);

            if (hasOverlap)
            {
                return new ResponseDto<TableReservationDto>
                {
                    IsSuccess = false,
                    Message = "Table is already reserved for the requested time slot."
                };
            }

            // AC-1: Set 15-minute Hold Expiration
            var holdExpiresAt = DateTime.UtcNow.AddMinutes(15);
            var reservation = new TableReservation
            {
                DiningTableId = dto.DiningTableId,
                CustomerId = dto.CustomerId,
                ReservationStartTime = dto.ReservationStartTime,
                ReservationEndTime = dto.ReservationEndTime,
                HoldExpiresAt = holdExpiresAt,
                ReservationStatus = "PendingPayment",
                DepositAmount = dto.DepositAmount,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };

            _context.TableReservations.Add(reservation);
            await _context.SaveChangesAsync(cancellationToken);

            // AC-3: Acquire Redis Lock Key with 15-minute TTL
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = holdExpiresAt
            };
            await _cache.SetStringAsync(lockKey, reservation.Id.ToString(), cacheOptions, cancellationToken);

            _logger.LogInformation("Reservation hold created: {ReservationId} for Table {TableId} expiring at {ExpiresAt}", reservation.Id, dto.DiningTableId, holdExpiresAt);

            var resultDto = new TableReservationDto
            {
                Id = reservation.Id,
                DiningTableId = reservation.DiningTableId,
                CustomerId = reservation.CustomerId,
                ReservationStartTime = reservation.ReservationStartTime,
                ReservationEndTime = reservation.ReservationEndTime,
                HoldExpiresAt = reservation.HoldExpiresAt,
                ReservationStatus = reservation.ReservationStatus,
                DepositAmount = reservation.DepositAmount
            };

            return new ResponseDto<TableReservationDto>
            {
                IsSuccess = true,
                Message = "Table hold placed successfully for 15 minutes.",
                Data = resultDto
            };
        }

        public async Task<ResponseDto<bool>> CancelHoldAsync(Guid reservationId, string cancelledBy, CancellationToken cancellationToken = default)
        {
            // AC-5: Manual cancellation of active hold
            var reservation = await _context.TableReservations
                .FirstOrDefaultAsync(r => r.Id == reservationId && !r.IsDeleted, cancellationToken);

            if (reservation == null)
            {
                return new ResponseDto<bool>
                {
                    IsSuccess = false,
                    Message = "Reservation not found.",
                    Data = false
                };
            }

            if (reservation.ReservationStatus != "PendingPayment")
            {
                return new ResponseDto<bool>
                {
                    IsSuccess = false,
                    Message = $"Reservation cannot be cancelled in state '{reservation.ReservationStatus}'.",
                    Data = false
                };
            }

            reservation.ReservationStatus = "Cancelled";
            reservation.ModifiedBy = cancelledBy;
            reservation.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            // Remove Redis Lock
            var lockKey = $"reservation_lock:{reservation.DiningTableId}:{reservation.ReservationStartTime:yyyyMMddHHmm}";
            await _cache.RemoveAsync(lockKey, cancellationToken);

            _logger.LogInformation("Reservation hold cancelled: {ReservationId}", reservationId);

            return new ResponseDto<bool>
            {
                IsSuccess = true,
                Message = "Reservation hold cancelled successfully.",
                Data = true
            };
        }

        public async Task<int> ReleaseExpiredHoldsAsync(CancellationToken cancellationToken = default)
        {
            // AC-4: Background release of expired holds
            var now = DateTime.UtcNow;
            var expiredReservations = await _context.TableReservations
                .Where(r => r.ReservationStatus == "PendingPayment" && r.HoldExpiresAt <= now && !r.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!expiredReservations.Any())
                return 0;

            foreach (var reservation in expiredReservations)
            {
                reservation.ReservationStatus = "Expired";
                reservation.ModifiedBy = "ReservationHoldExpiryWorker";
                reservation.ModifiedDate = now;

                var lockKey = $"reservation_lock:{reservation.DiningTableId}:{reservation.ReservationStartTime:yyyyMMddHHmm}";
                await _cache.RemoveAsync(lockKey, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Released {Count} expired reservation holds at {Timestamp}", expiredReservations.Count, now);

            return expiredReservations.Count;
        }

        public async Task<ResponseDto<TableReservationDto?>> GetActiveReservationByTableAsync(int tableId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var windowStart = now.AddHours(-2);
            var windowEnd = now.AddHours(4);

            var reservation = await _context.TableReservations
                .Include(r => r.Customer)
                .Include(r => r.DiningTable)
                .AsNoTracking()
                .Where(r => r.DiningTableId == tableId 
                         && !r.IsDeleted
                         && (r.ReservationStatus == "Confirmed" || r.ReservationStatus == "Seated")
                         && r.ReservationStartTime >= windowStart
                         && r.ReservationStartTime <= windowEnd)
                .OrderBy(r => r.ReservationStartTime)
                .FirstOrDefaultAsync(cancellationToken);

            if (reservation == null)
            {
                return new ResponseDto<TableReservationDto?>
                {
                    IsSuccess = true,
                    Message = "No active confirmed reservation found for this table.",
                    Data = null
                };
            }

            var dto = new TableReservationDto
            {
                Id = reservation.Id,
                DiningTableId = reservation.DiningTableId,
                TableName = reservation.DiningTable?.TableName,
                CustomerId = reservation.CustomerId,
                CustomerName = reservation.Customer?.CustomerName,
                CustomerPhone = reservation.Customer?.CustomerPhone,
                ReservationStartTime = reservation.ReservationStartTime,
                ReservationEndTime = reservation.ReservationEndTime,
                HoldExpiresAt = reservation.HoldExpiresAt,
                ReservationStatus = reservation.ReservationStatus,
                DepositAmount = reservation.DepositAmount
            };

            return new ResponseDto<TableReservationDto?>
            {
                IsSuccess = true,
                Message = "Active reservation retrieved successfully.",
                Data = dto
            };
        }

        public async Task<ResponseDto<RefundCalculationDto>> CalculateRefundAsync(Guid reservationId, CancellationToken cancellationToken = default)
        {
            var reservation = await _context.TableReservations
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == reservationId && !r.IsDeleted, cancellationToken);

            if (reservation == null)
            {
                return new ResponseDto<RefundCalculationDto>
                {
                    IsSuccess = false,
                    Message = "Reservation not found."
                };
            }

            var now = DateTime.UtcNow;
            var hoursNotice = (reservation.ReservationStartTime - now).TotalHours;

            int refundPct;
            decimal refundAmt;
            decimal forfeitAmt;
            string tierDescription;

            if (hoursNotice > 24)
            {
                refundPct = 100;
                refundAmt = reservation.DepositAmount;
                forfeitAmt = 0m;
                tierDescription = "Full 100% refund (> 24 hours notice)";
            }
            else if (hoursNotice >= 12)
            {
                refundPct = 50;
                refundAmt = Math.Round(reservation.DepositAmount * 0.50m, 2);
                forfeitAmt = reservation.DepositAmount - refundAmt;
                tierDescription = "Partial 50% refund (12-24 hours notice)";
            }
            else
            {
                refundPct = 0;
                refundAmt = 0m;
                forfeitAmt = reservation.DepositAmount;
                tierDescription = "No refund / deposit forfeited (< 12 hours notice)";
            }

            return new ResponseDto<RefundCalculationDto>
            {
                IsSuccess = true,
                Message = "Refund calculated successfully.",
                Data = new RefundCalculationDto
                {
                    ReservationId = reservation.Id,
                    ReservationStartTime = reservation.ReservationStartTime,
                    HoursNotice = Math.Round(hoursNotice, 2),
                    DepositAmount = reservation.DepositAmount,
                    RefundPercentage = refundPct,
                    RefundAmount = refundAmt,
                    ForfeitAmount = forfeitAmt,
                    PolicyTierDescription = tierDescription
                }
            };
        }

        public async Task<ResponseDto<TableReservationDto>> CancelReservationAsync(Guid reservationId, CancelReservationDto dto, string cancelledBy, CancellationToken cancellationToken = default)
        {
            var reservation = await _context.TableReservations
                .Include(r => r.Customer)
                .Include(r => r.DiningTable)
                .FirstOrDefaultAsync(r => r.Id == reservationId && !r.IsDeleted, cancellationToken);

            if (reservation == null)
            {
                return new ResponseDto<TableReservationDto>
                {
                    IsSuccess = false,
                    Message = "Reservation not found."
                };
            }

            if (reservation.ReservationStatus == "Cancelled")
            {
                return new ResponseDto<TableReservationDto>
                {
                    IsSuccess = false,
                    Message = "Reservation is already cancelled."
                };
            }

            if (reservation.ReservationStatus == "Completed" || reservation.ReservationStatus == "Seated")
            {
                return new ResponseDto<TableReservationDto>
                {
                    IsSuccess = false,
                    Message = $"Cannot cancel reservation in '{reservation.ReservationStatus}' status."
                };
            }

            decimal refundAmount = 0m;

            if (reservation.ReservationStatus == "Confirmed")
            {
                var refundCalc = await CalculateRefundAsync(reservationId, cancellationToken);
                if (refundCalc.IsSuccess && refundCalc.Data != null)
                {
                    refundAmount = refundCalc.Data.RefundAmount;
                }
            }

            reservation.ReservationStatus = "Cancelled";
            reservation.CancelledAt = DateTime.UtcNow;
            reservation.CancellationReason = dto?.Reason ?? "Cancelled by user";
            reservation.RefundAmount = refundAmount;
            reservation.ModifiedDate = DateTime.UtcNow;

            var paymentTx = await _context.PaymentTransactions
                .FirstOrDefaultAsync(p => p.TableReservationId == reservation.Id && p.Status == "Succeeded" && !p.IsDeleted, cancellationToken);

            // Release redis hold lock
            var lockKey = $"reservation_lock:{reservation.DiningTableId}:{reservation.ReservationStartTime:yyyyMMddHHmm}";
            await _cache.RemoveAsync(lockKey, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            // Publish MassTransit event via transactional outbox
            if (_publishEndpoint != null)
            {
                await _publishEndpoint.Publish(new RMS.Domain.Events.ReservationCancelledEvent
                {
                    ReservationId = reservation.Id,
                    DiningTableId = reservation.DiningTableId,
                    CustomerId = reservation.CustomerId,
                    CustomerName = reservation.Customer?.CustomerName,
                    CustomerEmail = reservation.Customer?.CustomerEmail,
                    DepositAmount = reservation.DepositAmount,
                    RefundAmount = refundAmount,
                    TransactionReference = paymentTx?.TransactionReference,
                    Reason = dto?.Reason,
                    CancelledAt = DateTime.UtcNow
                }, cancellationToken);
            }

            _logger.LogInformation("Reservation {ReservationId} CANCELLED. Refund: {RefundAmount:C}. Reason: {Reason}",
                reservation.Id, refundAmount, reservation.CancellationReason);

            var resultDto = new TableReservationDto
            {
                Id = reservation.Id,
                DiningTableId = reservation.DiningTableId,
                TableName = reservation.DiningTable?.TableName,
                CustomerId = reservation.CustomerId,
                CustomerName = reservation.Customer?.CustomerName,
                CustomerPhone = reservation.Customer?.CustomerPhone,
                ReservationStartTime = reservation.ReservationStartTime,
                ReservationEndTime = reservation.ReservationEndTime,
                HoldExpiresAt = reservation.HoldExpiresAt,
                ReservationStatus = reservation.ReservationStatus,
                DepositAmount = reservation.DepositAmount,
                RefundAmount = reservation.RefundAmount,
                CancelledAt = reservation.CancelledAt,
                CancellationReason = reservation.CancellationReason
            };

            return new ResponseDto<TableReservationDto>
            {
                IsSuccess = true,
                Message = "Reservation cancelled successfully.",
                Data = resultDto
            };
        }
    }
}
