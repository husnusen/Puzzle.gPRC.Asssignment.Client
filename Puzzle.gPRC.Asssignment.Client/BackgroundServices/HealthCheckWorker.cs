using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Puzzle.gPRC.Asssignment.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.gPRC.Asssignment.Client.BackgroundServices
{
    public class HealthCheckWorker : BackgroundService
    {
        private readonly ILogger<HealthCheckWorker> _logger;
        private readonly IHealthChecksService _healthCheckService;

        public HealthCheckWorker(ILogger<HealthCheckWorker> logger, IHealthChecksService healthCheckService)
        {
            _logger = logger;
            _healthCheckService = healthCheckService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var count = 0;
                while (!stoppingToken.IsCancellationRequested)
                {
                    count++;

                    var health = await _healthCheckService.CheckHealthAsync().ConfigureAwait(false);
                    Console.WriteLine($"Health Check Number of queries: {health.Entries.Count} ");
                    Console.WriteLine($"Health Check Status: {health.HealthStatus} ");
                    Console.WriteLine($"Health Check Duration in ms: {health.TotalDurationInMilliSeconds / health.Entries.Count} ");
                    Console.WriteLine($"Health Check Process Id: {health.ProcessId} ");
                    Console.ReadLine();

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                                 $"Error occured, Source:{nameof(HealthCheckWorker)}");
            }
        }
    }
}
