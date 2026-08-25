using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RMS.Application.Handlers;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Events;
using RMS.Infrastructure.Persistences;
using System;
using System.Threading.Tasks;
using Xunit;

namespace RMS.Tests
{
    public class ReservationConfirmedConsumerTests
    {
        private RestaurantDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var mockTenantService = new Mock<RMS.Domain.Interfaces.ITenantService>();
            mockTenantService.Setup(t => t.BranchID).Returns(1);

            return new RestaurantDbContext(options, mockTenantService.Object);
        }

        [Fact]
        public async Task Consume_DispatchesEmailAndSignalRUpdateOnConfirmedEvent_SatisfiesAC1ThroughAC4()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();

            var customer = new Customer
            {
                CustomerID = 5,
                CustomerName = "John Doe",
                CustomerEmail = "johndoe@example.com"
            };

            var table = new DiningTable
            {
                TableID = 3,
                TableName = "VIP Booth 1"
            };

            db.Customers.Add(customer);
            db.DiningTables.Add(table);
            db.TableReservations.Add(new TableReservation
            {
                Id = reservationId,
                DiningTableId = 3,
                CustomerId = 5,
                Customer = customer,
                DiningTable = table,
                ReservationStartTime = DateTime.UtcNow.AddHours(2),
                ReservationStatus = "Confirmed"
            });
            await db.SaveChangesAsync();

            var mockEmailSender = new Mock<IEmailSender>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockLogger = new Mock<ILogger<ReservationConfirmedConsumer>>();

            var consumer = new ReservationConfirmedConsumer(db, mockNotificationService.Object, mockEmailSender.Object, mockLogger.Object);

            var eventMsg = new ReservationConfirmedEvent
            {
                ReservationId = reservationId,
                DiningTableId = 3,
                CustomerId = 5,
                DepositAmount = 50.00m,
                TransactionReference = "pi_test_confirmed_999",
                ConfirmedAt = DateTime.UtcNow
            };

            var mockConsumeContext = new Mock<ConsumeContext<ReservationConfirmedEvent>>();
            mockConsumeContext.Setup(c => c.Message).Returns(eventMsg);

            // Act
            await consumer.Consume(mockConsumeContext.Object);

            // Assert (AC-4 Email Receipt)
            mockEmailSender.Verify(e => e.SendEmailAsync(
                "johndoe@example.com",
                It.Is<string>(s => s.Contains("pi_test_confirmed_999")),
                It.Is<string>(b => b.Contains("VIP Booth 1") && b.Contains("$50.00"))
            ), Times.Once);

            // Assert (AC-2, AC-3 SignalR Broadcast)
            mockNotificationService.Verify(n => n.SendReservationUpdateAsync(
                It.IsAny<object>()
            ), Times.Once);
        }

        [Fact]
        public async Task Consume_HandlesMissingCustomerGracefully_SatisfiesAC4()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();

            var mockEmailSender = new Mock<IEmailSender>();
            var mockNotificationService = new Mock<INotificationService>();
            var mockLogger = new Mock<ILogger<ReservationConfirmedConsumer>>();

            var consumer = new ReservationConfirmedConsumer(db, mockNotificationService.Object, mockEmailSender.Object, mockLogger.Object);

            var eventMsg = new ReservationConfirmedEvent
            {
                ReservationId = reservationId,
                DiningTableId = 1,
                CustomerId = 99,
                DepositAmount = 20.00m,
                TransactionReference = "pi_test_fallback_111",
                ConfirmedAt = DateTime.UtcNow
            };

            var mockConsumeContext = new Mock<ConsumeContext<ReservationConfirmedEvent>>();
            mockConsumeContext.Setup(c => c.Message).Returns(eventMsg);

            // Act
            await consumer.Consume(mockConsumeContext.Object);

            // Assert Fallback Handling
            mockEmailSender.Verify(e => e.SendEmailAsync(
                "customer@example.com",
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Once);

            mockNotificationService.Verify(n => n.SendReservationUpdateAsync(
                It.IsAny<object>()
            ), Times.Once);
        }
    }
}
