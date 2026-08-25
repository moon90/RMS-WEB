using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using RMS.Application.DTOs;
using RMS.Application.Handlers;
using RMS.Application.Implementations;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Domain.Events;
using RMS.Infrastructure.Persistences;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RMS.Tests
{
    public class CancellationAndRefundPolicyTests
    {
        private RestaurantDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<RestaurantDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            var mockTenantService = new Mock<RMS.Domain.Interfaces.ITenantService>();
            mockTenantService.Setup(t => t.BranchID).Returns(1);

            return new RestaurantDbContext(options, mockTenantService.Object);
        }

        [Fact]
        public async Task CalculateRefund_GreaterThan24HoursNotice_Returns100PercentRefund_SatisfiesAC1()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var reservation = new TableReservation
            {
                Id = reservationId,
                DiningTableId = 1,
                CustomerId = 10,
                BranchID = 1,
                ReservationStartTime = DateTime.UtcNow.AddHours(36), // 36 hours ahead
                ReservationEndTime = DateTime.UtcNow.AddHours(38),
                HoldExpiresAt = DateTime.UtcNow.AddHours(36),
                ReservationStatus = "Confirmed",
                DepositAmount = 100.00m
            };
            db.TableReservations.Add(reservation);
            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var mockPublish = new Mock<IPublishEndpoint>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object, mockPublish.Object);

            // Act
            var result = await service.CalculateRefundAsync(reservationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(100, result.Data.RefundPercentage);
            Assert.Equal(100.00m, result.Data.RefundAmount);
            Assert.Equal(0.00m, result.Data.ForfeitAmount);
        }

        [Fact]
        public async Task CalculateRefund_Between12And24HoursNotice_Returns50PercentRefund_SatisfiesAC1()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var reservation = new TableReservation
            {
                Id = reservationId,
                DiningTableId = 2,
                CustomerId = 12,
                BranchID = 1,
                ReservationStartTime = DateTime.UtcNow.AddHours(18), // 18 hours ahead
                ReservationEndTime = DateTime.UtcNow.AddHours(20),
                HoldExpiresAt = DateTime.UtcNow.AddHours(18),
                ReservationStatus = "Confirmed",
                DepositAmount = 100.00m
            };
            db.TableReservations.Add(reservation);
            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var mockPublish = new Mock<IPublishEndpoint>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object, mockPublish.Object);

            // Act
            var result = await service.CalculateRefundAsync(reservationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(50, result.Data.RefundPercentage);
            Assert.Equal(50.00m, result.Data.RefundAmount);
            Assert.Equal(50.00m, result.Data.ForfeitAmount);
        }

        [Fact]
        public async Task CalculateRefund_LessThan12HoursNotice_Returns0PercentRefund_SatisfiesAC1()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var reservation = new TableReservation
            {
                Id = reservationId,
                DiningTableId = 3,
                CustomerId = 15,
                BranchID = 1,
                ReservationStartTime = DateTime.UtcNow.AddHours(6), // 6 hours ahead
                ReservationEndTime = DateTime.UtcNow.AddHours(8),
                HoldExpiresAt = DateTime.UtcNow.AddHours(6),
                ReservationStatus = "Confirmed",
                DepositAmount = 80.00m
            };
            db.TableReservations.Add(reservation);
            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var mockPublish = new Mock<IPublishEndpoint>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object, mockPublish.Object);

            // Act
            var result = await service.CalculateRefundAsync(reservationId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(0, result.Data.RefundPercentage);
            Assert.Equal(0.00m, result.Data.RefundAmount);
            Assert.Equal(80.00m, result.Data.ForfeitAmount);
        }

        [Fact]
        public async Task CancelReservation_ConfirmedStatus_UpdatesEntityAndPublishesOutboxEvent_SatisfiesAC2AndAC3()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var customer = new Customer { CustomerID = 20, CustomerName = "Sarah Connor", CustomerEmail = "sarah@example.com" };
            var table = new DiningTable { TableID = 4, TableName = "Table 4" };
            var reservation = new TableReservation
            {
                Id = reservationId,
                DiningTableId = 4,
                DiningTable = table,
                CustomerId = 20,
                Customer = customer,
                BranchID = 1,
                ReservationStartTime = DateTime.UtcNow.AddHours(30), // 100% refund tier
                ReservationEndTime = DateTime.UtcNow.AddHours(32),
                HoldExpiresAt = DateTime.UtcNow.AddHours(30),
                ReservationStatus = "Confirmed",
                DepositAmount = 60.00m
            };
            var paymentTx = new PaymentTransaction
            {
                Id = Guid.NewGuid(),
                TableReservationId = reservationId,
                TransactionReference = "pi_mock_123456",
                Amount = 60.00m,
                Currency = "USD",
                Status = "Succeeded"
            };

            db.Customers.Add(customer);
            db.DiningTables.Add(table);
            db.TableReservations.Add(reservation);
            db.PaymentTransactions.Add(paymentTx);
            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var mockPublish = new Mock<IPublishEndpoint>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object, mockPublish.Object);

            var cancelDto = new CancelReservationDto { Reason = "Schedule conflict" };

            // Act
            var result = await service.CancelReservationAsync(reservationId, cancelDto, "user1");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Cancelled", result.Data.ReservationStatus);
            Assert.Equal(60.00m, result.Data.RefundAmount);
            Assert.Equal("Schedule conflict", result.Data.CancellationReason);

            // Verify Outbox Event Published (AC-3)
            mockPublish.Verify(p => p.Publish(
                It.Is<ReservationCancelledEvent>(e =>
                    e.ReservationId == reservationId &&
                    e.RefundAmount == 60.00m &&
                    e.TransactionReference == "pi_mock_123456"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancelReservation_SeatedOrCompletedStatus_RejectsCancellation_SatisfiesAC2()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var customer = new Customer { CustomerID = 22, CustomerName = "Test Customer" };
            var table = new DiningTable { TableID = 5, TableName = "Table 5" };
            var reservation = new TableReservation
            {
                Id = reservationId,
                DiningTableId = 5,
                DiningTable = table,
                CustomerId = 22,
                Customer = customer,
                BranchID = 1,
                ReservationStartTime = DateTime.UtcNow.AddHours(1),
                ReservationEndTime = DateTime.UtcNow.AddHours(3),
                HoldExpiresAt = DateTime.UtcNow.AddHours(1),
                ReservationStatus = "Seated", // Already seated
                DepositAmount = 50.00m
            };
            db.Customers.Add(customer);
            db.DiningTables.Add(table);
            db.TableReservations.Add(reservation);
            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var mockPublish = new Mock<IPublishEndpoint>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object, mockPublish.Object);

            // Act
            var result = await service.CancelReservationAsync(reservationId, new CancelReservationDto(), "user1");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Cannot cancel reservation in 'Seated' status", result.Message);
        }

        [Fact]
        public async Task ReservationCancelledConsumer_WithRefund_InvokesGatewayAndDispatchesNotifications_SatisfiesAC4AndAC5()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var customer = new Customer { CustomerID = 30, CustomerName = "Bruce Wayne", CustomerEmail = "bruce@wayne.com" };
            var table = new DiningTable { TableID = 6, TableName = "Table 6 Penthouse" };
            var reservation = new TableReservation
            {
                Id = reservationId,
                DiningTableId = 6,
                DiningTable = table,
                CustomerId = 30,
                Customer = customer,
                BranchID = 1,
                ReservationStartTime = DateTime.UtcNow.AddHours(28),
                ReservationEndTime = DateTime.UtcNow.AddHours(30),
                HoldExpiresAt = DateTime.UtcNow.AddHours(28),
                ReservationStatus = "Cancelled",
                DepositAmount = 100.00m,
                RefundAmount = 100.00m
            };

            db.Customers.Add(customer);
            db.DiningTables.Add(table);
            db.TableReservations.Add(reservation);
            await db.SaveChangesAsync();

            var mockGateway = new Mock<IPaymentGatewayService>();
            mockGateway.Setup(g => g.RefundDepositAsync(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ResponseDto<PaymentTransaction> { IsSuccess = true });

            var mockNotification = new Mock<INotificationService>();
            var mockEmail = new Mock<IEmailSender>();
            var mockLogger = new Mock<ILogger<ReservationCancelledConsumer>>();

            var consumer = new ReservationCancelledConsumer(db, mockGateway.Object, mockNotification.Object, mockEmail.Object, mockLogger.Object);

            var contextMock = new Mock<ConsumeContext<ReservationCancelledEvent>>();
            contextMock.Setup(c => c.Message).Returns(new ReservationCancelledEvent
            {
                ReservationId = reservationId,
                DiningTableId = 6,
                CustomerId = 30,
                CustomerName = "Bruce Wayne",
                CustomerEmail = "bruce@wayne.com",
                DepositAmount = 100.00m,
                RefundAmount = 100.00m,
                TransactionReference = "pi_bruce_123",
                Reason = "Business meeting rescheduled",
                CancelledAt = DateTime.UtcNow
            });

            // Act
            await consumer.Consume(contextMock.Object);

            // Assert
            // AC-4: Gateway refund triggered
            mockGateway.Verify(g => g.RefundDepositAsync("pi_bruce_123", 100.00m, "Business meeting rescheduled", It.IsAny<CancellationToken>()), Times.Once);

            // AC-5: Guest email confirmation sent
            mockEmail.Verify(e => e.SendEmailAsync("bruce@wayne.com", It.Is<string>(s => s.Contains("Cancellation & Refund")), It.IsAny<string>()), Times.Once);

            // AC-5: SignalR live table release dispatched
            mockNotification.Verify(n => n.SendReservationUpdateAsync(It.IsAny<object>()), Times.Once);
        }
    }
}
