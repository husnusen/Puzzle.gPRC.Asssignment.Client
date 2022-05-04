
using GrpcGetDataClient;
using Grpc.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Puzzle.gPRC.Asssignment.Client.Services;
using Microsoft.Extensions.Hosting;
using Puzzle.gPRC.Asssignment.Client.BackgroundServices;
using Puzzle.gPRC.Asssignment.Client.Configuration;
using Microsoft.Extensions.Configuration;

namespace Puzzle.gPRC.Asssignment.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(app => {
                        app.AddJsonFile("appsettings.json");
                    })
                    .ConfigureServices((hostContext, services) =>
                    {

                        services.AddHostedService<DataWorker>();
                        services.AddHostedService<HealthCheckWorker>();
                        services.AddSingleton<IConfigSettings, ConfigSettings>();
                        services.AddScoped<IDataServiceWrapper, DataServiceWrapper>();
                        services.AddScoped<IHealthChecksService, HealthChecksService>();
                    });

    }
}