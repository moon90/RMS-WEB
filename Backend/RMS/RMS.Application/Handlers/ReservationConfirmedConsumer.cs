using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RMS.Application.Interfaces;
using RMS.Domain.Events;
using RMS.Infrastructure.Persistences;
using System.Threading.Tasks;

namespace RMS.Application.Handlers
{
    public class ReservationConfirmedConsumer : IConsumer<ReservationConfirmedEvent>
    {
        private readonly RestaurantDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ReservationConfirmedConsumer> _logger;

        public ReservationConfirmedConsumer(
            RestaurantDbContext context,
            INotificationService notificationService,
            IEmailSender emailSender,
            ILogger<ReservationConfirmedConsumer> logger)
        {
            _context = context;
            _notificationService = notificationService;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ReservationConfirmedEvent> context)
        {
            var msg = context.Message;
            _logger.LogInformation("Processing ReservationConfirmedEvent for ReservationId: {ReservationId}", msg.ReservationId);

            var reservation = await _context.TableReservations
                .Include(r => r.Customer)
                .Include(r => r.DiningTable)
                .FirstOrDefaultAsync(r => r.Id == msg.ReservationId);

            var customerEmail = reservation?.Customer?.CustomerEmail ?? "customer@example.com";
            var customerName = reservation?.Customer?.CustomerName ?? "Valued Customer";
            var tableName = reservation?.DiningTable?.TableName ?? $"Table #{msg.DiningTableId}";

            // AC-4: Dispatch Email Receipt
            var subject = $"Table Reservation Confirmed - Ref #{msg.TransactionReference}";
            var emailBody = $@"
                <h2>Reservation Confirmed!</h2>
                <p>Dear {customerName},</p>
                <p>Your deposit of <strong>${msg.DepositAmount:F2}</strong> has been successfully processed.</p>
                <ul>
                    <li><strong>Table:</strong> {tableName}</li>
                    <li><strong>Time:</strong> {reservation?.ReservationStartTime:g}</li>
                    <li><strong>Transaction Reference:</strong> {msg.TransactionReference}</li>
                </ul>
                <p>Please present your Confirmation Code <code>{msg.ReservationId.ToString()[..8].ToUpper()}</code> upon arrival.</p>";

            await _emailSender.SendEmailAsync(customerEmail, subject, emailBody);

            // AC-2, AC-3: Broadcast real-time update to Hostess & KDS dashboards over SignalR (/rmshub)
            await _notificationService.SendReservationUpdateAsync(new
            {
                ReservationId = msg.ReservationId,
                DiningTableId = msg.DiningTableId,
                ReservationStatus = "Confirmed",
                CustomerName = customerName,
                DepositAmount = msg.DepositAmount,
                ConfirmedAt = msg.ConfirmedAt
            });

            _logger.LogInformation("Confirmation email and SignalR update dispatched for Reservation {ReservationId}", msg.ReservationId);
        }
    }
}
