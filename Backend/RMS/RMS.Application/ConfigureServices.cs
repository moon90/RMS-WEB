using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RMS.Application.Implementations;
using RMS.Application.Interfaces;
using RMS.Application.Validators;
using RMS.Application.Validators.ManufacturerValidators;
using RMS.Application.Validators.ProductValidators;
using RMS.Application.Validators.SupplierValidators;
using RMS.Application.Validators.UnitValidators;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;
using RMS.Infrastructure.Repositories;
using System.Reflection;

namespace RMS.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationAndInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Infrastructure Services
            services.AddDbContext<RestaurantDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(RestaurantDbContext).Assembly.FullName)));

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
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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

            // FluentValidation
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

            return services;
        }
    }
}
