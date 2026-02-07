using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RMS.Application.Implementations;
using RMS.Application.Interfaces;
using RMS.Application.Services.Processing;
using RMS.Application.Validators;
using RMS.Application.Validators.CustomerValidators;
using RMS.Application.Validators.DiningTableValidators;
using RMS.Application.Validators.IngredientValidators;
using RMS.Application.Validators.InventoryValidators;
using RMS.Application.Validators.ManufacturerValidators;
using RMS.Application.Validators.OrderValidators;
using RMS.Application.Validators.ProductIngredientValidators;
using RMS.Application.Validators.ProductValidators;
using RMS.Application.Validators.PurchaseValidators;
using RMS.Application.Validators.StaffValidators;
using RMS.Application.Validators.StockTransactionValidators;
using RMS.Application.Validators.SupplierValidators;
using RMS.Application.Validators.UnitValidators;
using RMS.Application.Validators.SaleValidators;
using RMS.Application.Validators.UnitConversionValidators;

namespace RMS.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application Services
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IManufacturerService, ManufacturerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IStockTransactionService, StockTransactionService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IProductIngredientService, ProductIngredientService>();
            services.AddScoped<IDiningTableService, DiningTableService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IImageProcessingService, ImageProcessingService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IAlertService, AlertService>();
            services.AddScoped<IUnitConversionService, UnitConversionService>();
            services.AddScoped<ISplitPaymentService, SplitPaymentService>();

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<CreatePurchaseDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<MenuCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<MenuUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<RoleCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<RoleUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UserCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UserUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<ResetPasswordDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<PermissionCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<PermissionUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoryCreateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CategoryUpdateDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateUnitDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateUnitDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateSupplierDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateSupplierDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateManufacturerDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateManufacturerDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProductDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateCustomerDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateCustomerDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateStaffDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateStaffDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateInventoryDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateInventoryDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateStockTransactionDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateStockTransactionDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateIngredientDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateIngredientDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProductIngredientDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProductIngredientDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateOrderDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateOrderDetailDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateOrderDetailDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<ProcessPaymentForOrderDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateDiningTableDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateDiningTableDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateDiningTableStatusDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreatePromotionDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdatePromotionDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateSaleDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateUnitConversionDtoValidator>();

            return services;
        }
    }
}
