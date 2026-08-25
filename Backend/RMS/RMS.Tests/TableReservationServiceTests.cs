using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using RMS.Application.DTOs;
using RMS.Application.Implementations;
using RMS.Domain.Entities;
using RMS.Infrastructure.Persistences;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RMS.Tests
{
    public class TableReservationServiceTests
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
        public async Task CreateHoldAsync_CreatesReservationWithPendingStatusAnd15MinExpiry_SatisfiesAC1AndAC3()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            db.DiningTables.Add(new DiningTable { TableID = 1, TableName = "Table 1" });
            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object);

            var dto = new CreateReservationHoldDto
            {
                DiningTableId = 1,
                CustomerId = 10,
                ReservationStartTime = DateTime.UtcNow.AddHours(2),
                ReservationEndTime = DateTime.UtcNow.AddHours(3.5),
                DepositAmount = 25.00m
            };

            // Act
            var response = await service.CreateHoldAsync(dto, "user1");

            // Assert
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Data);
            Assert.Equal("PendingPayment", response.Data.ReservationStatus);
            Assert.True(response.Data.HoldExpiresAt > DateTime.UtcNow.AddMinutes(14));

            // Verify Redis lock acquisition (AC-3)
            mockCache.Verify(c => c.SetAsync(
                It.Is<string>(k => k.StartsWith("reservation_lock:1:")),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateHoldAsync_RejectsDuplicateHoldForLockedTable_SatisfiesAC2()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            db.DiningTables.Add(new DiningTable { TableID = 1, TableName = "Table 1" });
            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            // Simulate active Redis lock
            mockCache.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(Encoding.UTF8.GetBytes("existing-reservation-id"));

            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object);

            var dto = new CreateReservationHoldDto
            {
                DiningTableId = 1,
                CustomerId = 11,
                ReservationStartTime = DateTime.UtcNow.AddHours(2),
                ReservationEndTime = DateTime.UtcNow.AddHours(3.5),
                DepositAmount = 25.00m
            };

            // Act
            var response = await service.CreateHoldAsync(dto, "user2");

            // Assert (AC-2)
            Assert.False(response.IsSuccess);
            Assert.Contains("locked", response.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task CancelHoldAsync_CancelsActiveHoldAndRemovesRedisLock_SatisfiesAC5()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var startTime = DateTime.UtcNow.AddHours(2);

            db.TableReservations.Add(new TableReservation
            {
                Id = reservationId,
                DiningTableId = 1,
                CustomerId = 10,
                ReservationStartTime = startTime,
                ReservationEndTime = startTime.AddHours(1.5),
                HoldExpiresAt = DateTime.UtcNow.AddMinutes(15),
                ReservationStatus = "PendingPayment"
            });
            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object);

            // Act
            var response = await service.CancelHoldAsync(reservationId, "user1");

            // Assert (AC-5)
            Assert.True(response.IsSuccess);
            Assert.True(response.Data);

            var updated = await db.TableReservations.FindAsync(reservationId);
            Assert.NotNull(updated);
            Assert.Equal("Cancelled", updated.ReservationStatus);

            // Verify Redis lock removal
            mockCache.Verify(c => c.RemoveAsync(It.Is<string>(k => k.StartsWith("reservation_lock:1:")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReleaseExpiredHoldsAsync_MarksExpiredHoldsAndRemovesLocks_SatisfiesAC4()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var startTime = DateTime.UtcNow.AddHours(1);

            // Add 1 expired reservation hold
            db.TableReservations.Add(new TableReservation
            {
                Id = Guid.NewGuid(),
                DiningTableId = 1,
                CustomerId = 10,
                ReservationStartTime = startTime,
                ReservationEndTime = startTime.AddHours(1),
                HoldExpiresAt = DateTime.UtcNow.AddMinutes(-5), // Expired 5 minutes ago
                ReservationStatus = "PendingPayment"
            });

            // Add 1 active reservation hold
            db.TableReservations.Add(new TableReservation
            {
                Id = Guid.NewGuid(),
                DiningTableId = 2,
                CustomerId = 12,
                ReservationStartTime = startTime,
                ReservationEndTime = startTime.AddHours(1),
                HoldExpiresAt = DateTime.UtcNow.AddMinutes(10), // Active for 10 more minutes
                ReservationStatus = "PendingPayment"
            });

            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object);

            // Act
            var releasedCount = await service.ReleaseExpiredHoldsAsync();

            // Assert (AC-4)
            Assert.Equal(1, releasedCount);
            var expiredCount = await db.TableReservations.CountAsync(r => r.ReservationStatus == "Expired");
            var pendingCount = await db.TableReservations.CountAsync(r => r.ReservationStatus == "PendingPayment");

            Assert.Equal(1, expiredCount);
            Assert.Equal(1, pendingCount);
        }
    }
}
