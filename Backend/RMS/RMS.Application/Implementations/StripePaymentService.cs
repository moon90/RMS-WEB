using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Events;
using RMS.Infrastructure.Persistences;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Implementations
{
    public class StripePaymentService : IPaymentGatewayService
    {
        private readonly RestaurantDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IConfiguration _configuration;
        private readonly ILogger<StripePaymentService> _logger;

        public StripePaymentService(
            RestaurantDbContext context,
            IPublishEndpoint publishEndpoint,
            IConfiguration configuration,
            ILogger<StripePaymentService> logger)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ResponseDto<CheckoutSessionResponseDto>> CreateCheckoutSessionAsync(CreateCheckoutSessionDto dto, CancellationToken cancellationToken = default)
        {
            // AC-1: Verify reservation existence & status
            var reservation = await _context.TableReservations
                .FirstOrDefaultAsync(r => r.Id == dto.ReservationId && !r.IsDeleted, cancellationToken);

            if (reservation == null)
            {
                return new ResponseDto<CheckoutSessionResponseDto>
                {
                    IsSuccess = false,
                    Message = "Reservation not found."
                };
            }

            if (reservation.ReservationStatus != "PendingPayment")
            {
                return new ResponseDto<CheckoutSessionResponseDto>
                {
                    IsSuccess = false,
                    Message = $"Cannot initiate deposit payment for reservation in state '{reservation.ReservationStatus}'."
                };
            }

            var publishableKey = _configuration["StripeSettings:PublishableKey"] ?? "pk_test_rms_mock_key";
            var transactionRef = $"pi_{Guid.NewGuid():N}";
            var clientSecret = $"{transactionRef}_secret_{Guid.NewGuid():N}";

            var paymentTx = new PaymentTransaction
            {
                TableReservationId = reservation.Id,
                PaymentGateway = "Stripe",
                TransactionReference = transactionRef,
                Amount = reservation.DepositAmount,
                Currency = "USD",
                Status = "Processing",
                CreatedBy = "system",
                CreatedDate = DateTime.UtcNow
            };

            _context.PaymentTransactions.Add(paymentTx);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Checkout session created for Reservation {ReservationId}, PaymentIntent {TransactionRef}", reservation.Id, transactionRef);

            var result = new CheckoutSessionResponseDto
            {
                ClientSecret = clientSecret,
                PublishableKey = publishableKey,
                TransactionReference = transactionRef,
                Amount = reservation.DepositAmount,
                Currency = "USD"
            };

            return new ResponseDto<CheckoutSessionResponseDto>
            {
                IsSuccess = true,
                Message = "Payment checkout session created successfully.",
                Data = result
            };
        }

        public async Task<ResponseDto<PaymentWebhookResultDto>> ProcessStripeWebhookAsync(string jsonPayload, string stripeSignature, CancellationToken cancellationToken = default)
        {
            // AC-2, AC-4: Parse payload & enforce Idempotent processing
            try
            {
                using var doc = JsonDocument.Parse(jsonPayload);
                var root = doc.RootElement;

                var eventId = root.TryGetProperty("id", out var idProp) ? idProp.GetString() : Guid.NewGuid().ToString();
                var eventType = root.TryGetProperty("type", out var typeProp) ? typeProp.GetString() : "payment_intent.succeeded";

                string? transactionRef = null;
                if (root.TryGetProperty("data", out var dataProp) && dataProp.TryGetProperty("object", out var objProp))
                {
                    if (objProp.TryGetProperty("id", out var piProp))
                    {
                        transactionRef = piProp.GetString();
                    }
                }

                if (string.IsNullOrEmpty(transactionRef))
                {
                    return new ResponseDto<PaymentWebhookResultDto>
                    {
                        IsSuccess = false,
                        Message = "Transaction reference missing from webhook payload."
                    };
                }

                var paymentTx = await _context.PaymentTransactions
                    .Include(p => p.TableReservation)
                    .FirstOrDefaultAsync(p => p.TransactionReference == transactionRef, cancellationToken);

                if (paymentTx == null)
                {
                    _logger.LogWarning("Webhook received for unknown transaction reference: {TransactionRef}", transactionRef);
                    return new ResponseDto<PaymentWebhookResultDto>
                    {
                        IsSuccess = false,
                        Message = "Transaction reference not found."
                    };
                }

                // Idempotency check: if already processed, return 200 OK without re-triggering events
                if (paymentTx.Status == "Succeeded" || paymentTx.Status == "Failed")
                {
                    _logger.LogInformation("Webhook idempotency check: Event {EventId} for {TransactionRef} already in terminal state {Status}", eventId, transactionRef, paymentTx.Status);
                    return new ResponseDto<PaymentWebhookResultDto>
                    {
                        IsSuccess = true,
                        Message = "Webhook already processed (Idempotent).",
                        Data = new PaymentWebhookResultDto
                        {
                            Processed = true,
                            Message = "Already processed",
                            ReservationId = paymentTx.TableReservationId,
                            NewReservationStatus = paymentTx.TableReservation?.ReservationStatus
                        }
                    };
                }

                paymentTx.WebhookEventId = eventId;
                paymentTx.ModifiedDate = DateTime.UtcNow;

                if (eventType == "payment_intent.succeeded")
                {
                    paymentTx.Status = "Succeeded";

                    if (paymentTx.TableReservation != null)
                    {
                        // AC-3: Update status to Confirmed
                        paymentTx.TableReservation.ReservationStatus = "Confirmed";
                        paymentTx.TableReservation.ModifiedDate = DateTime.UtcNow;

                        // Publish MassTransit Event for real-time dispatching
                        await _publishEndpoint.Publish(new ReservationConfirmedEvent
                        {
                            ReservationId = paymentTx.TableReservation.Id,
                            DiningTableId = paymentTx.TableReservation.DiningTableId,
                            CustomerId = paymentTx.TableReservation.CustomerId,
                            DepositAmount = paymentTx.Amount,
                            TransactionReference = transactionRef,
                            ConfirmedAt = DateTime.UtcNow
                        }, cancellationToken);

                        _logger.LogInformation("Reservation {ReservationId} CONFIRMED via Webhook {EventId}", paymentTx.TableReservation.Id, eventId);
                    }
                }
                else if (eventType == "payment_intent.payment_failed")
                {
                    paymentTx.Status = "Failed";

                    if (paymentTx.TableReservation != null)
                    {
                        paymentTx.TableReservation.ReservationStatus = "Cancelled";
                        paymentTx.TableReservation.ModifiedDate = DateTime.UtcNow;
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new ResponseDto<PaymentWebhookResultDto>
                {
                    IsSuccess = true,
                    Message = "Stripe webhook processed successfully.",
                    Data = new PaymentWebhookResultDto
                    {
                        Processed = true,
                        Message = "Processed successfully",
                        ReservationId = paymentTx.TableReservationId,
                        NewReservationStatus = paymentTx.TableReservation?.ReservationStatus
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Stripe webhook");
                return new ResponseDto<PaymentWebhookResultDto>
                {
                    IsSuccess = false,
                    Message = $"Webhook processing error: {ex.Message}"
                };
            }
        }
    }
}
