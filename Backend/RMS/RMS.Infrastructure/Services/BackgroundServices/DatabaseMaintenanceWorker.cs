using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RMS.Infrastructure.Persistences;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Services.BackgroundServices
{
    public class DatabaseMaintenanceWorker : BackgroundService
    {
        private readonly ILogger<DatabaseMaintenanceWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DatabaseMaintenanceWorker(ILogger<DatabaseMaintenanceWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Database Maintenance Worker is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Run once a week
                    await Task.Delay(TimeSpan.FromDays(7), stoppingToken);

                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();

                    _logger.LogInformation("Starting weekly index maintenance...");
                    await dbContext.Database.ExecuteSqlRawAsync("EXEC [dbo].[MaintainIndexes]", stoppingToken);
                    _logger.LogInformation("Index maintenance completed successfully.");

                    // Added from Phase 6.2
                    _logger.LogInformation("Starting data archiving...");
                    await dbContext.Database.ExecuteSqlRawAsync("EXEC [dbo].[ArchiveOldOrders]", stoppingToken);
                    _logger.LogInformation("Data archiving completed successfully.");
                }
                catch (TaskCanceledException)
                {
                    // Ignore, service is stopping
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing Database Maintenance Worker.");
                }
            }
        }
    }
}
