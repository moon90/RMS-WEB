using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;
using RMS.Infrastructure.Repositories;

namespace RMS.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<RestaurantDbContext>(options =>
            {
                if (connectionString != null && (connectionString.Contains(".db") || connectionString.Contains("DataSource=")))
                {
                    options.UseSqlite(connectionString,
                        b => b.MigrationsAssembly(typeof(RestaurantDbContext).Assembly.FullName));
                }
                else
                {
                    options.UseSqlServer(connectionString,
                        b => b.MigrationsAssembly(typeof(RestaurantDbContext).Assembly.FullName));
                }
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IStockTransactionRepository, StockTransactionRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IProductIngredientRepository, ProductIngredientRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IDiningTableRepository, DiningTableRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<IUnitConversionRepository, UnitConversionRepository>();
            services.AddScoped<ISplitPaymentRepository, SplitPaymentRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
