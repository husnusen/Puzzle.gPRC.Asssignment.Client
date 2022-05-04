using GrpcHealthChecksClient;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Puzzle.gPRC.Asssignment.Client.Configuration;
using Microsoft.Extensions.Logging;

namespace Puzzle.gPRC.Asssignment.Client.Services
{
    public class HealthChecksService : IHealthChecksService
    {
        private HealthChecks.HealthChecksClient _client;
        private readonly ILogger<HealthChecksService> _logger;
        private readonly IConfigSettings _config;

        private HttpClientHandler httpClientHandler
        {
            get
            {
                var httpHandler = new HttpClientHandler();
                // Return `true` to allow certificates that are untrusted/invalid
                httpHandler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                return httpHandler;
            }
        }


        public HealthChecksService(ILogger<HealthChecksService> logger, IConfigSettings configSettings)
        {
            _logger = logger;
            _config = configSettings;
            try
            {
                var channel = GrpcChannel.ForAddress($"{_config.ServerUrl}:{_config.ServerPort}", new GrpcChannelOptions { HttpHandler = httpClientHandler });
                _client = new HealthChecks.HealthChecksClient(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<HealthReport> CheckHealthAsync()
        {

            try
            {
                return await _client.CheckHealthAsync(new Google.Protobuf.WellKnownTypes.Empty());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured, Source:{nameof(HealthChecksService)}");
                throw;
            }
        }
        
    }
}
