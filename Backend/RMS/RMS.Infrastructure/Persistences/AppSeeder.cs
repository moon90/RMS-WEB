using Microsoft.EntityFrameworkCore;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Persistences
{
    public static class AppSeeder
    {
        public static async Task SeedAllAsync(RestaurantDbContext context)
        {
            await SeedCategoriesAsync(context);
            await SeedSuppliersAsync(context);
            await SeedManufacturersAsync(context);
            await SeedProductsAsync(context);
            await SeedDiningTablesAsync(context);
            await SeedStaffAsync(context);
            await SeedCustomersAsync(context);
            await SeedOrdersAsync(context);
            await SeedInventoryAsync(context);
            
            await context.SaveChangesAsync();
        }

        private static async Task SeedCategoriesAsync(RestaurantDbContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            var categories = new List<Category>
            {
                new Category { CategoryName = "Burgers", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Category { CategoryName = "Steaks", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Category { CategoryName = "Pastas", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Category { CategoryName = "Beverages", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Category { CategoryName = "Desserts", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow },
                new Category { CategoryName = "Salads", Status = true, CreatedBy = "system", CreatedDate = DateTime.UtcNow }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedSuppliersAsync(RestaurantDbContext context)
        {
            if (await context.Suppliers.AnyAsync()) return;

            var suppliers = new List<Supplier>
            {
                new Supplier { SupplierName = "Global Food Distro", ContactPerson = "Alice Smith", Email = "alice@globalfood.com", Phone = "123-456-7890", Status = true, CreatedBy = "system" },
                new Supplier { SupplierName = "Fresh Farms Ltd", ContactPerson = "Bob Jones", Email = "bob@freshfarms.com", Phone = "234-567-8901", Status = true, CreatedBy = "system" }
            };

            await context.Suppliers.AddRangeAsync(suppliers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedManufacturersAsync(RestaurantDbContext context)
        {
            if (await context.Manufacturers.AnyAsync()) return;

            var manufacturers = new List<Manufacturer>
            {
                new Manufacturer { ManufacturerName = "Catering Pros", Status = true, CreatedBy = "system" },
                new Manufacturer { ManufacturerName = "Gourmet Gear", Status = true, CreatedBy = "system" }
            };

            await context.Manufacturers.AddRangeAsync(manufacturers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedProductsAsync(RestaurantDbContext context)
        {
            if (await context.Products.AnyAsync()) return;

            var categoryIds = await context.Categories.Select(c => c.CategoryID).ToListAsync();
            var supplierIds = await context.Suppliers.Select(s => s.Id).ToListAsync();
            var manufacturerIds = await context.Manufacturers.Select(m => m.Id).ToListAsync();

            var products = new List<Product>
            {
                new Product { ProductName = "Classic Beef Burger", ProductPrice = 12.99m, CostPrice = 5.00m, CategoryID = categoryIds[0], SupplierID = supplierIds[0], ManufacturerID = manufacturerIds[0], Status = true, CreatedBy = "system" },
                new Product { ProductName = "Cheeseburger Deluxe", ProductPrice = 14.99m, CostPrice = 6.00m, CategoryID = categoryIds[0], SupplierID = supplierIds[0], ManufacturerID = manufacturerIds[0], Status = true, CreatedBy = "system" },
                new Product { ProductName = "Ribeye Steak", ProductPrice = 24.99m, CostPrice = 10.00m, CategoryID = categoryIds[1], SupplierID = supplierIds[1], ManufacturerID = manufacturerIds[1], Status = true, CreatedBy = "system" },
                new Product { ProductName = "Filet Mignon", ProductPrice = 29.99m, CostPrice = 12.00m, CategoryID = categoryIds[1], SupplierID = supplierIds[1], ManufacturerID = manufacturerIds[1], Status = true, CreatedBy = "system" },
                new Product { ProductName = "Spaghetti Carbonara", ProductPrice = 15.99m, CostPrice = 4.50m, CategoryID = categoryIds[2], SupplierID = supplierIds[0], ManufacturerID = manufacturerIds[1], Status = true, CreatedBy = "system" },
                new Product { ProductName = "Penne Arrabbiata", ProductPrice = 13.99m, CostPrice = 4.00m, CategoryID = categoryIds[2], SupplierID = supplierIds[0], ManufacturerID = manufacturerIds[1], Status = true, CreatedBy = "system" },
                new Product { ProductName = "Craft Cola", ProductPrice = 3.50m, CostPrice = 0.50m, CategoryID = categoryIds[3], SupplierID = supplierIds[0], ManufacturerID = manufacturerIds[0], Status = true, CreatedBy = "system" },
                new Product { ProductName = "Iced Tea", ProductPrice = 2.99m, CostPrice = 0.30m, CategoryID = categoryIds[3], SupplierID = supplierIds[0], ManufacturerID = manufacturerIds[0], Status = true, CreatedBy = "system" },
                new Product { ProductName = "Chocolate Lava Cake", ProductPrice = 8.99m, CostPrice = 2.50m, CategoryID = categoryIds[4], SupplierID = supplierIds[1], ManufacturerID = manufacturerIds[1], Status = true, CreatedBy = "system" },
                new Product { ProductName = "Caesar Salad", ProductPrice = 10.99m, CostPrice = 3.00m, CategoryID = categoryIds[5], SupplierID = supplierIds[1], ManufacturerID = manufacturerIds[1], Status = true, CreatedBy = "system" }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        private static async Task SeedDiningTablesAsync(RestaurantDbContext context)
        {
            if (await context.DiningTables.AnyAsync()) return;

            var tables = new List<DiningTable>();
            for (int i = 1; i <= 15; i++)
            {
                tables.Add(new DiningTable 
                { 
                    TableName = $"Table {i}", 
                    DiningTableStatus = RMS.Domain.Enum.DiningTableStatusEnum.Available,
                    Status = true, 
                    CreatedBy = "system" 
                });
            }

            await context.DiningTables.AddRangeAsync(tables);
            await context.SaveChangesAsync();
        }

        private static async Task SeedStaffAsync(RestaurantDbContext context)
        {
            if (await context.Staff.AnyAsync()) return;

            var staff = new List<Staff>
            {
                new Staff { StaffName = "Michael Scott", StaffRole = "Manager", StaffPhone = "555-1111", Status = true, CreatedBy = "system" },
                new Staff { StaffName = "Jim Halpert", StaffRole = "Waiter", StaffPhone = "555-2222", Status = true, CreatedBy = "system" },
                new Staff { StaffName = "Pam Beesly", StaffRole = "Hostess", StaffPhone = "555-3333", Status = true, CreatedBy = "system" },
                new Staff { StaffName = "Dwight Schrute", StaffRole = "Head Chef", StaffPhone = "555-4444", Status = true, CreatedBy = "system" }
            };

            await context.Staff.AddRangeAsync(staff);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCustomersAsync(RestaurantDbContext context)
        {
            if (await context.Customers.AnyAsync()) return;

            var customers = new List<Customer>
            {
                new Customer { CustomerName = "John Doe", CustomerPhone = "555-0001", CustomerEmail = "john@example.com", Address = "123 Maple St", CreatedBy = "system" },
                new Customer { CustomerName = "Jane Smith", CustomerPhone = "555-0002", CustomerEmail = "jane@example.com", Address = "456 Oak Ave", CreatedBy = "system" },
                new Customer { CustomerName = "Bob Wilson", CustomerPhone = "555-0003", CustomerEmail = "bob@example.com", Address = "789 Pine Rd", CreatedBy = "system" }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        private static async Task SeedOrdersAsync(RestaurantDbContext context)
        {
            if (await context.Orders.AnyAsync()) return;

            var products = await context.Products.ToListAsync();
            var staff = await context.Staff.ToListAsync();
            var tables = await context.DiningTables.ToListAsync();
            var customers = await context.Customers.ToListAsync();

            var random = new Random();
            var orders = new List<Order>();

            for (int i = 1; i <= 50; i++)
            {
                var orderDate = DateTime.UtcNow.AddDays(-random.Next(0, 30));
                var orderType = random.Next(0, 3) switch
                {
                    0 => "DineIn",
                    1 => "TakeOut",
                    _ => "Delivery"
                };

                var waiter = staff[random.Next(staff.Count)];

                var order = new Order
                {
                    OrderDate = orderDate,
                    OrderTime = orderDate.ToString("HH:mm"),
                    OrderType = orderType,
                    OrderStatus = random.Next(0, 10) > 2 ? "Paid" : "Pending",
                    TableName = orderType == "DineIn" ? tables[random.Next(tables.Count)].TableName : "N/A",
                    WaiterName = waiter.StaffName,
                    StaffID = waiter.StaffID,
                    CustomerID = orderType == "Delivery" ? customers[random.Next(customers.Count)].CustomerID : (int?)null,
                    BranchID = 1,
                    CreatedBy = "system",
                    CreatedDate = orderDate,
                    OrderDetails = new List<OrderDetail>()
                };

                int itemCount = random.Next(1, 5);
                decimal total = 0;

                for (int j = 0; j < itemCount; j++)
                {
                    var product = products[random.Next(products.Count)];
                    var qty = random.Next(1, 4);
                    var detail = new OrderDetail
                    {
                        ProductID = product.Id,
                        Quantity = qty,
                        Price = product.ProductPrice,
                        Amount = product.ProductPrice * qty,
                        CreatedBy = "system",
                        CreatedDate = orderDate
                    };
                    order.OrderDetails.Add(detail);
                    total += detail.Amount;
                }

                order.Total = total;
                order.Received = order.OrderStatus == "Paid" ? total : 0;
                order.AmountPaid = order.OrderStatus == "Paid" ? total : 0;
                order.PaymentStatus = order.OrderStatus == "Paid" ? "Paid" : "Unpaid";
                order.PaymentMethod = order.OrderStatus == "Paid" ? (random.Next(0, 2) == 0 ? "Cash" : "Card") : null;
                
                orders.Add(order);
            }

            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();
        }

        private static async Task SeedInventoryAsync(RestaurantDbContext context)
        {
            if (await context.Inventory.AnyAsync()) return;

            var products = await context.Products.ToListAsync();
            var inventory = products.Select(p => new Inventory
            {
                ProductID = p.Id,
                InitialStock = 100,
                CurrentStock = 100,
                MinStockLevel = 10,
                LastUpdated = DateTime.UtcNow,
                Status = true,
                CreatedBy = "system",
                CreatedDate = DateTime.UtcNow
            }).ToList();

            await context.Inventory.AddRangeAsync(inventory);
            await context.SaveChangesAsync();
        }
    }
}
