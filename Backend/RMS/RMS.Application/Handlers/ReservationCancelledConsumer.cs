using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Application.Interfaces;
using RMS.Domain.Events;
using RMS.Infrastructure.Persistences;
using System;
using System.Threading.Tasks;

namespace RMS.Application.Handlers
{
    public class ReservationCancelledConsumer : IConsumer<ReservationCancelledEvent>
    {
        private readonly RestaurantDbContext _context;
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly INotificationService _notificationService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ReservationCancelledConsumer> _logger;

        public ReservationCancelledConsumer(
            RestaurantDbContext context,
            IPaymentGatewayService paymentGatewayService,
            INotificationService notificationService,
            IEmailSender emailSender,
            ILogger<ReservationCancelledConsumer> logger)
        {
            _context = context;
            _paymentGatewayService = paymentGatewayService;
            _notificationService = notificationService;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservationCancelledEvent> context)
        {
            var msg = context.Message;
            _logger.LogInformation("Processing ReservationCancelledEvent for ReservationId: {ReservationId}", msg.ReservationId);

            // AC-4: Execute automated Stripe refund if refund amount is greater than zero
            if (msg.RefundAmount > 0 && !string.IsNullOrWhiteSpace(msg.TransactionReference))
            {
                var refundResult = await _paymentGatewayService.RefundDepositAsync(
                    msg.TransactionReference,
                    msg.RefundAmount,
                    msg.Reason,
                    context.CancellationToken);

                if (!refundResult.IsSuccess)
                {
                    _logger.LogWarning("Refund processing for Transaction {TransactionRef} returned error: {Message}",
                        msg.TransactionReference, refundResult.Message);
                }
            }

            var reservation = await _context.TableReservations
                .Include(r => r.Customer)
                .Include(r => r.DiningTable)
                .FirstOrDefaultAsync(r => r.Id == msg.ReservationId, context.CancellationToken);

            var customerEmail = msg.CustomerEmail ?? reservation?.Customer?.CustomerEmail ?? "customer@example.com";
            var customerName = msg.CustomerName ?? reservation?.Customer?.CustomerName ?? "Valued Customer";
            var tableName = reservation?.DiningTable?.TableName ?? $"Table #{msg.DiningTableId}";

            // AC-5: Dispatch Cancellation & Refund Email Receipt
            var subject = $"Reservation Cancellation & Refund Confirmation - Ref #{msg.ReservationId.ToString()[..8].ToUpper()}";
            var emailBody = $@"
                <h2>Reservation Cancelled</h2>
                <p>Dear {customerName},</p>
                <p>Your table booking for <strong>{tableName}</strong> has been cancelled.</p>
                <ul>
                    <li><strong>Original Deposit:</strong> ${msg.DepositAmount:F2}</li>
                    <li><strong>Refund Amount:</strong> ${msg.RefundAmount:F2}</li>
                    <li><strong>Cancellation Reason:</strong> {msg.Reason ?? "Requested by guest"}</li>
                    <li><strong>Cancelled At:</strong> {msg.CancelledAt:g}</li>
                </ul>
                {(msg.RefundAmount > 0 ? "<p>Your refund has been initiated to your original payment method.</p>" : "<p>Due to cancellation notice policy rules, no deposit refund was applicable for this booking.</p>")}
                <p>Thank you for considering our restaurant. We hope to see you again soon.</p>";

            try
            {
                await _emailSender.SendEmailAsync(customerEmail, subject, emailBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send cancellation email to {Email}", customerEmail);
            }

            // AC-5: Broadcast SignalR real-time table availability release to POS & Hostess
            await _notificationService.SendReservationUpdateAsync(new
            {
                ReservationId = msg.ReservationId,
                DiningTableId = msg.DiningTableId,
                ReservationStatus = "Cancelled",
                IsTableAvailable = true,
                CustomerName = customerName,
                RefundAmount = msg.RefundAmount,
                CancelledAt = msg.CancelledAt
            });

            _logger.LogInformation("Cancellation email and SignalR availability release dispatched for Reservation {ReservationId}", msg.ReservationId);
        }
    }
}
