using Microsoft.Extensions.DependencyInjection;
using RMS.Domain.Interfaces;
using RMS.Infrastructure.Interfaces;
using RMS.Infrastructure.IRepositories;
using RMS.Infrastructure.Persistences;
using RMS.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Infrastructure
{
    public static class ConfigureInfrastructureServices
    {
        //public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        //{
        //    services.AddScoped<IUnitOfWork, UnitOfWork>();

        //    services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        //    services.AddScoped<IMenuRepository, MenuRepository>();
        //    services.AddScoped<IUserRepository, UserRepository>();
        //    services.AddScoped<IRoleRepository, RoleRepository>();
        //    services.AddScoped<IPermissionRepository, PermissionRepository>();
        //    services.AddScoped<IRoleMenuRepository, RoleMenuRepository>();
        //    services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();


        //    return services;
        //}
    }
}
