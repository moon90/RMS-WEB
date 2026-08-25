using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using RMS.Application.AutoMappers;
using RMS.Application.DTOs;
using RMS.Application.DTOs.Orders;
using RMS.Application.Implementations;
using RMS.Application.Interfaces;
using FluentValidation;
using RMS.Application.Validators.OrderValidators;
using RMS.Domain.Entities;
using RMS.Domain.Events;
using RMS.Infrastructure.Persistences;
using RMS.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace RMS.Tests
{
    public class PosCheckInAndSettlementTests
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

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public async Task GetActiveReservationByTableAsync_ReturnsConfirmedReservationWithGuestDetails_SatisfiesAC1()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var customer = new Customer
            {
                CustomerID = 101,
                CustomerName = "John Doe",
                CustomerPhone = "+15551234567"
            };
            var table = new DiningTable
            {
                TableID = 5,
                TableName = "Table 5 VIP"
            };
            var reservation = new TableReservation
            {
                Id = Guid.NewGuid(),
                DiningTableId = 5,
                DiningTable = table,
                CustomerId = 101,
                Customer = customer,
                ReservationStartTime = DateTime.UtcNow.AddMinutes(30),
                ReservationEndTime = DateTime.UtcNow.AddMinutes(120),
                HoldExpiresAt = DateTime.UtcNow.AddHours(2),
                ReservationStatus = "Confirmed",
                DepositAmount = 50.00m
            };

            db.Customers.Add(customer);
            db.DiningTables.Add(table);
            db.TableReservations.Add(reservation);
            await db.SaveChangesAsync();

            var mockCache = new Mock<IDistributedCache>();
            var mockLogger = new Mock<ILogger<TableReservationService>>();
            var service = new TableReservationService(db, mockCache.Object, mockLogger.Object);

            // Act
            var result = await service.GetActiveReservationByTableAsync(5);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(reservation.Id, result.Data.Id);
            Assert.Equal("John Doe", result.Data.CustomerName);
            Assert.Equal("+15551234567", result.Data.CustomerPhone);
            Assert.Equal("Table 5 VIP", result.Data.TableName);
            Assert.Equal(50.00m, result.Data.DepositAmount);
            Assert.Equal("Confirmed", result.Data.ReservationStatus);
        }

        [Fact]
        public async Task CreateOrder_WithReservationId_BindsReservationAndSetsStatusToSeated_SatisfiesAC2()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var reservation = new TableReservation
            {
                Id = reservationId,
                DiningTableId = 3,
                CustomerId = 101,
                BranchID = 1,
                ReservationStartTime = DateTime.UtcNow.AddMinutes(15),
                ReservationEndTime = DateTime.UtcNow.AddMinutes(105),
                HoldExpiresAt = DateTime.UtcNow.AddHours(2),
                ReservationStatus = "Confirmed",
                DepositAmount = 40.00m
            };
            db.TableReservations.Add(reservation);
            db.DiningTables.Add(new DiningTable { TableID = 3, TableName = "Table 3", DiningTableStatus = RMS.Domain.Enum.DiningTableStatusEnum.Available });
            await db.SaveChangesAsync();

            var mockTenant = new Mock<RMS.Domain.Interfaces.ITenantService>();
            mockTenant.Setup(t => t.BranchID).Returns(1);

            var mapper = GetMapper();
            var unitOfWork = new UnitOfWork(db, mockTenant.Object);
            var orderRepo = new OrderRepository(db, mockTenant.Object);
            var productRepo = new ProductRepository(db, mockTenant.Object);
            var inventoryRepo = new InventoryRepository(db, mockTenant.Object);
            var saleRepo = new SaleRepository(db, mockTenant.Object);
            var diningTableRepo = new DiningTableRepository(db, mockTenant.Object);

            var createValidator = new CreateOrderDtoValidator();
            var updateValidator = new UpdateOrderDtoValidator();
            var paymentValidator = new ProcessPaymentForOrderDtoValidator();

            var mockPublish = new Mock<IPublishEndpoint>();
            var mockNotification = new Mock<INotificationService>();
            var mockAudit = new Mock<IAuditLogService>();
            var mockPurchase = new Mock<IPurchaseService>();
            var mockProduct = new Mock<IProductService>();
            var mockSplit = new Mock<ISplitPaymentService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            var orderService = new OrderService(
                orderRepo,
                productRepo,
                inventoryRepo,
                saleRepo,
                unitOfWork,
                mapper,
                createValidator,
                updateValidator,
                paymentValidator,
                mockAudit.Object,
                mockLogger.Object,
                mockNotification.Object,
                mockSplit.Object,
                mockPurchase.Object,
                mockProduct.Object,
                diningTableRepo,
                mockPublish.Object
            );

            var createOrderDto = new CreateOrderDto
            {
                OrderDate = DateTime.UtcNow,
                OrderType = "DineIn",
                OrderStatus = "Pending",
                TableName = "Table 3",
                WaiterName = "Alice",
                OrderTime = "12:30",
                TableReservationId = reservationId,
                DepositDeducted = 40.00m,
                Total = 120.00m,
                OrderDetails = new List<CreateOrderDetailDto>
                {
                    new CreateOrderDetailDto { ProductID = 1, Quantity = 2, Price = 60.00m, Amount = 120.00m }
                }
            };

            // Act
            var result = await orderService.CreateOrderAsync(createOrderDto);

            // Assert
            Assert.True(result.IsSuccess, result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(reservationId, result.Data.TableReservationId);
            Assert.Equal(40.00m, result.Data.DepositDeducted);

            // Verify TableReservation status transitioned to Seated (AC-2)
            var updatedReservation = await db.TableReservations.FindAsync(reservationId);
            Assert.NotNull(updatedReservation);
            Assert.Equal("Seated", updatedReservation.ReservationStatus);
        }

        [Fact]
        public async Task ProcessPayment_WithDepositDeducted_CalculatesNetPayableAndCompletesReservation_SatisfiesAC3AndAC4()
        {
            // Arrange
            var db = GetInMemoryDbContext(Guid.NewGuid().ToString());
            var reservationId = Guid.NewGuid();
            var reservation = new TableReservation
            {
                Id = reservationId,
                DiningTableId = 2,
                CustomerId = 105,
                BranchID = 1,
                ReservationStartTime = DateTime.UtcNow,
                ReservationEndTime = DateTime.UtcNow.AddHours(2),
                HoldExpiresAt = DateTime.UtcNow.AddHours(2),
                ReservationStatus = "Seated",
                DepositAmount = 30.00m
            };
            db.TableReservations.Add(reservation);

            var order = new Order
            {
                OrderID = 501,
                OrderDate = DateTime.UtcNow,
                OrderTime = "12:00",
                OrderStatus = "Pending",
                OrderType = "DineIn",
                TableName = "Table 2",
                Total = 100.00m,
                TaxAmount = 10.00m,
                ServiceChargeAmount = 5.00m,
                DiscountAmount = 0m,
                TipAmount = 5.00m,
                DepositDeducted = 30.00m,
                TableReservationId = reservationId,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail { OrderDetailID = 1, OrderID = 501, ProductID = 10, Quantity = 2, Price = 50.00m, Amount = 100.00m }
                }
            };
            db.Orders.Add(order);
            await db.SaveChangesAsync();

            var mockTenant = new Mock<RMS.Domain.Interfaces.ITenantService>();
            mockTenant.Setup(t => t.BranchID).Returns(1);

            var mapper = GetMapper();
            var unitOfWork = new UnitOfWork(db, mockTenant.Object);
            var orderRepo = new OrderRepository(db, mockTenant.Object);
            var productRepo = new ProductRepository(db, mockTenant.Object);
            var inventoryRepo = new InventoryRepository(db, mockTenant.Object);
            var saleRepo = new SaleRepository(db, mockTenant.Object);
            var diningTableRepo = new DiningTableRepository(db, mockTenant.Object);

            var createValidator = new CreateOrderDtoValidator();
            var updateValidator = new UpdateOrderDtoValidator();
            var paymentValidator = new ProcessPaymentForOrderDtoValidator();

            var mockPublish = new Mock<IPublishEndpoint>();
            var mockNotification = new Mock<INotificationService>();
            var mockAudit = new Mock<IAuditLogService>();
            var mockPurchase = new Mock<IPurchaseService>();
            var mockProduct = new Mock<IProductService>();
            var mockSplit = new Mock<ISplitPaymentService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            var orderService = new OrderService(
                orderRepo,
                productRepo,
                inventoryRepo,
                saleRepo,
                unitOfWork,
                mapper,
                createValidator,
                updateValidator,
                paymentValidator,
                mockAudit.Object,
                mockLogger.Object,
                mockNotification.Object,
                mockSplit.Object,
                mockPurchase.Object,
                mockProduct.Object,
                diningTableRepo,
                mockPublish.Object
            );

            // Gross = 100 + 10 + 5 + 5 = 120.00. Net Payable = 120.00 - 30.00 (deposit) = 90.00.
            var paymentDto = new ProcessPaymentForOrderDto
            {
                OrderID = 501,
                AmountReceived = 100.00m,
                AmountPaid = 90.00m,
                ChangeAmount = 10.00m,
                DiscountAmount = 0m,
                TipAmount = 5.00m,
                PaymentMethod = "Cash"
            };

            // Act
            var result = await orderService.ProcessPaymentForOrderAsync(paymentDto);

            // Assert
            Assert.True(result.IsSuccess);

            // Verify Sale record created with deposit deduction (AC-4)
            var sale = await db.Sales.FirstOrDefaultAsync(s => s.TableReservationId == reservationId);
            Assert.NotNull(sale);
            Assert.Equal(30.00m, sale.DepositDeducted);
            Assert.Equal(90.00m, sale.FinalAmount); // Net Payable after deposit deduction
            Assert.Equal(reservationId, sale.TableReservationId);

            // Verify TableReservation marked Completed (AC-4)
            var updatedReservation = await db.TableReservations.FindAsync(reservationId);
            Assert.NotNull(updatedReservation);
            Assert.Equal("Completed", updatedReservation.ReservationStatus);
        }
    }
}
