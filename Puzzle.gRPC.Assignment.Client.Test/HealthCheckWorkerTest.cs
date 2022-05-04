using GrpcHealthChecksClient;
using Microsoft.Extensions.Logging;
using Moq;
using Puzzle.gPRC.Asssignment.Client;
using Puzzle.gPRC.Asssignment.Client.BackgroundServices;
using Puzzle.gPRC.Asssignment.Client.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xbehave;
using Xunit;

namespace Puzzle.gRPC.Assignment.Client.Test
{
    public class HealthCheckWorkerTest
    {
        private readonly Mock<IHealthChecksService> _healthChecksServiceMock;
        private Mock<ILogger<HealthCheckWorker>> _loggerMock;
        private HealthCheckWorker _healthCheckWorker;
        public HealthCheckWorkerTest()
        {
            _healthChecksServiceMock = new Mock<IHealthChecksService>();
            _loggerMock = new Mock<ILogger<HealthCheckWorker>>();
        }

        [Scenario]
        public async Task HealthCheck_Service_Called_Successfully()
        {
            "Set up the call".x(() => {
                _healthChecksServiceMock.Setup(_ => _.CheckHealthAsync()).ReturnsAsync(HealthReportMock);
                _healthCheckWorker = new HealthCheckWorker(_loggerMock.Object, _healthChecksServiceMock.Object);

            });
            "Call service ".x(async () =>
            {
                await _healthCheckWorker.StartAsync(CancellationToken.None);
            });
            "Verify ".x(() =>
            {
                _healthChecksServiceMock.Verify(_ => _.CheckHealthAsync(), Times.Once);
            });

        }

        private HealthReport HealthReportMock
        {
            get
            {
                var healthReport = new HealthReport
                {
                    HealthStatus = "Healthy",
                    ProcessId = Guid.NewGuid().ToString(),
                    TotalDuration = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(TimeSpan.FromMilliseconds(9)),
                    TotalDurationInMilliSeconds = 9,

                };
                healthReport.Entries.Add("Entry1");
                return healthReport;
            }
        }
         

    }
}
