using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using RMS.Application.DTOs;
using RMS.Application.Implementations;
using RMS.Domain.Entities;
using RMS.Domain.Events;
using RMS.Infrastructure.Persistences;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RMS.Tests
{
    public class StripePaymentServiceTests
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
        public async Task CreateCheckoutSessionAsync_CreatesPaymentTransactionAndReturnsClientSecret_SatisfiesAC1()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            db.TableReservations.Add(new TableReservation
            {
                Id = reservationId,
                DiningTableId = 1,
                CustomerId = 10,
                ReservationStartTime = DateTime.UtcNow.AddHours(2),
                ReservationEndTime = DateTime.UtcNow.AddHours(3.5),
                HoldExpiresAt = DateTime.UtcNow.AddMinutes(15),
                ReservationStatus = "PendingPayment",
                DepositAmount = 25.00m
            });
            await db.SaveChangesAsync();

            var mockPublish = new Mock<IPublishEndpoint>();
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["StripeSettings:PublishableKey"]).Returns("pk_test_rms_123");

            var mockLogger = new Mock<ILogger<StripePaymentService>>();
            var service = new StripePaymentService(db, mockPublish.Object, mockConfig.Object, mockLogger.Object);

            var dto = new CreateCheckoutSessionDto { ReservationId = reservationId };

            // Act
            var response = await service.CreateCheckoutSessionAsync(dto);

            // Assert (AC-1)
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Data);
            Assert.Equal("pk_test_rms_123", response.Data.PublishableKey);
            Assert.Equal(25.00m, response.Data.Amount);
            Assert.StartsWith("pi_", response.Data.TransactionReference);
            Assert.Contains("_secret_", response.Data.ClientSecret);

            var paymentTx = await db.PaymentTransactions.FirstOrDefaultAsync(p => p.TableReservationId == reservationId);
            Assert.NotNull(paymentTx);
            Assert.Equal("Processing", paymentTx.Status);
        }

        [Fact]
        public async Task ProcessStripeWebhookAsync_ConfirmsReservationAndPublishesEventOnSuccess_SatisfiesAC2AndAC3()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var transactionRef = "pi_test_succeeded_123";

            db.TableReservations.Add(new TableReservation
            {
                Id = reservationId,
                DiningTableId = 2,
                CustomerId = 15,
                ReservationStartTime = DateTime.UtcNow.AddHours(1),
                ReservationEndTime = DateTime.UtcNow.AddHours(2),
                HoldExpiresAt = DateTime.UtcNow.AddMinutes(10),
                ReservationStatus = "PendingPayment",
                DepositAmount = 30.00m
            });

            db.PaymentTransactions.Add(new PaymentTransaction
            {
                TableReservationId = reservationId,
                PaymentGateway = "Stripe",
                TransactionReference = transactionRef,
                Amount = 30.00m,
                Currency = "USD",
                Status = "Processing"
            });
            await db.SaveChangesAsync();

            var mockPublish = new Mock<IPublishEndpoint>();
            var mockConfig = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<StripePaymentService>>();
            var service = new StripePaymentService(db, mockPublish.Object, mockConfig.Object, mockLogger.Object);

            var webhookPayload = $"{{\"id\":\"evt_101\",\"type\":\"payment_intent.succeeded\",\"data\":{{\"object\":{{\"id\":\"{transactionRef}\"}}}}}}";

            // Act
            var response = await service.ProcessStripeWebhookAsync(webhookPayload, "sig_header");

            // Assert (AC-2, AC-3)
            Assert.True(response.IsSuccess);
            Assert.Equal("Confirmed", response.Data?.NewReservationStatus);

            var updatedReservation = await db.TableReservations.FindAsync(reservationId);
            Assert.NotNull(updatedReservation);
            Assert.Equal("Confirmed", updatedReservation.ReservationStatus);

            var updatedTx = await db.PaymentTransactions.FirstOrDefaultAsync(p => p.TransactionReference == transactionRef);
            Assert.NotNull(updatedTx);
            Assert.Equal("Succeeded", updatedTx.Status);
            Assert.Equal("evt_101", updatedTx.WebhookEventId);

            // Verify MassTransit Event Publication
            mockPublish.Verify(p => p.Publish(It.Is<ReservationConfirmedEvent>(e =>
                e.ReservationId == reservationId &&
                e.DiningTableId == 2 &&
                e.DepositAmount == 30.00m
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ProcessStripeWebhookAsync_IgnoresReplayedWebhookForTerminalTransaction_SatisfiesAC4()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var transactionRef = "pi_test_replayed_123";

            db.TableReservations.Add(new TableReservation
            {
                Id = reservationId,
                ReservationStatus = "Confirmed"
            });

            db.PaymentTransactions.Add(new PaymentTransaction
            {
                TableReservationId = reservationId,
                TransactionReference = transactionRef,
                Status = "Succeeded",
                WebhookEventId = "evt_original"
            });
            await db.SaveChangesAsync();

            var mockPublish = new Mock<IPublishEndpoint>();
            var mockConfig = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<StripePaymentService>>();
            var service = new StripePaymentService(db, mockPublish.Object, mockConfig.Object, mockLogger.Object);

            var webhookPayload = $"{{\"id\":\"evt_replayed\",\"type\":\"payment_intent.succeeded\",\"data\":{{\"object\":{{\"id\":\"{transactionRef}\"}}}}}}";

            // Act
            var response = await service.ProcessStripeWebhookAsync(webhookPayload, "sig_header");

            // Assert (AC-4 Idempotency)
            Assert.True(response.IsSuccess);
            Assert.Contains("Idempotent", response.Message);

            // Verify MassTransit Event was NOT published again
            mockPublish.Verify(p => p.Publish(It.IsAny<ReservationConfirmedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ProcessStripeWebhookAsync_CancelsReservationOnPaymentFailure_SatisfiesAC5()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var transactionRef = "pi_test_failed_123";

            db.TableReservations.Add(new TableReservation
            {
                Id = reservationId,
                ReservationStatus = "PendingPayment"
            });

            db.PaymentTransactions.Add(new PaymentTransaction
            {
                TableReservationId = reservationId,
                TransactionReference = transactionRef,
                Status = "Processing"
            });
            await db.SaveChangesAsync();

            var mockPublish = new Mock<IPublishEndpoint>();
            var mockConfig = new Mock<IConfiguration>();
            var mockLogger = new Mock<ILogger<StripePaymentService>>();
            var service = new StripePaymentService(db, mockPublish.Object, mockConfig.Object, mockLogger.Object);

            var webhookPayload = $"{{\"id\":\"evt_failed_1\",\"type\":\"payment_intent.payment_failed\",\"data\":{{\"object\":{{\"id\":\"{transactionRef}\"}}}}}}";

            // Act
            var response = await service.ProcessStripeWebhookAsync(webhookPayload, "sig_header");

            // Assert (AC-5)
            Assert.True(response.IsSuccess);

            var updatedReservation = await db.TableReservations.FindAsync(reservationId);
            Assert.NotNull(updatedReservation);
            Assert.Equal("Cancelled", updatedReservation.ReservationStatus);

            var updatedTx = await db.PaymentTransactions.FirstOrDefaultAsync(p => p.TransactionReference == transactionRef);
            Assert.NotNull(updatedTx);
            Assert.Equal("Failed", updatedTx.Status);
        }
    }
}
