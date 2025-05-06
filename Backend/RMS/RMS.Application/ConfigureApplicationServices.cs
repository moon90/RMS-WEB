using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RMS.Application.Implementations;
using RMS.Application.Interfaces;
using RMS.Application.Validators;
using RMS.Domain.DTOs.MenuDTOs.InputDTOs;
using RMS.Domain.DTOs.RoleDTOs.InputDTOs;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application
{
    public static class ConfigureApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Business Services DI Regitration
            //services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            // Register services
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<IMenuService, MenuService>();

            //// Register FluentValidation
            //services.AddScoped<IValidator<MenuCreateDto>, MenuCreateDtoValidator>();
            //services.AddScoped<IValidator<MenuUpdateDto>, MenuUpdateDtoValidator>();
            //services.AddScoped<IValidator<RoleCreateDto>, RoleCreateDtoValidator>();
            //services.AddScoped<IValidator<RoleUpdateDto>, RoleUpdateDtoValidator>();
            //services.AddScoped<IValidator<UserCreateDto>, UserCreateDtoValidator>();
            //services.AddScoped<IValidator<UserUpdateDto>, UserUpdateDtoValidator>();
            //services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }
    }
}
