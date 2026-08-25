using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RMS.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.WebApi.Services
{
    public class ReservationHoldExpiryWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReservationHoldExpiryWorker> _logger;

        public ReservationHoldExpiryWorker(
            IServiceProvider serviceProvider,
            ILogger<ReservationHoldExpiryWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reservation Hold Expiry Worker starting...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var reservationService = scope.ServiceProvider.GetRequiredService<ITableReservationService>();
                        var releasedCount = await reservationService.ReleaseExpiredHoldsAsync(stoppingToken);

                        if (releasedCount > 0)
                        {
                            _logger.LogInformation("Reservation Hold Expiry Worker released {Count} expired holds.", releasedCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing Reservation Hold Expiry Worker.");
                }

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}
