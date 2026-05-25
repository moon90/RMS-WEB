
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace RMS.Infrastructure.Persistences
{
    public class RestaurantDbContextFactory : IDesignTimeDbContextFactory<RestaurantDbContext>
    {
        public RestaurantDbContext CreateDbContext(string[] args)
        {
            // Get environment which is passed as an argument
            string environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // Build config
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../RMS.WebApi"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<RestaurantDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
            
            // Dummy Tenant Service for Design-Time
            var dummyTenantService = new DesignTimeTenantService();
            
            return new RestaurantDbContext(optionsBuilder.Options, dummyTenantService);
        }
    }

    internal class DesignTimeTenantService : RMS.Domain.Interfaces.ITenantService
    {
        public int? BranchID { get; set; }
    }
}

