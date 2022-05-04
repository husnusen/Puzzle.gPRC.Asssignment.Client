using GrpcGetDataClient;
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
    public class DataWorker : BackgroundService
    {
        private readonly ILogger<DataWorker> _logger;
        private readonly IDataServiceWrapper _serviceWrapper;
        public DataWorker(ILogger<DataWorker> logger, IDataServiceWrapper serviceWrapper)
        {
            _logger= logger;
            _serviceWrapper = serviceWrapper;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var count = 0;
                while (!stoppingToken.IsCancellationRequested)
                {
                    count++;

                    var request = new GetDataRequest();
                    request.PageStart = 0;
                    request.Columns.AddRange(new[] { "Column1", "Column2", "Column2" });
                    request.TableName = "Firm";

                    request.Filters.AddRange(new Filter[] { new Filter {
                    ColumnName = "MARKET_CAP",
                    Value = "90",
                    Op= OperationType.GraterThan
                } });
                    var response = await _serviceWrapper.GetDataAsync(request).ConfigureAwait(false);

                    Console.WriteLine("----------------------------------------------------------------------------------------");
                    Console.WriteLine($"Query : {response.Query} | Total Count : {response.TotalCount}   |   Current Count : {response.CurrentCount} | Schema : [{string.Join(',', response.Schema)} ]");
                    Console.WriteLine("----------------------------------------------------------------------------------------");
                    Console.WriteLine("Data ");
                    Console.WriteLine("----------------------------------------------------------------------------------------");

                    foreach (var data in response.Data)
                    {
                        Console.WriteLine($"[{string.Join(',', data.ColumnValue)}] ");
                    }


                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                                 $"Error occured, Source:{nameof(DataWorker)}");
            }
        }
    }
}
