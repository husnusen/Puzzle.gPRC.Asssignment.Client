using Grpc.Net.Client;
using GrpcGetDataClient;
using Microsoft.Extensions.Logging;
using Puzzle.gPRC.Asssignment.Client.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.gPRC.Asssignment.Client.Services
{
    public class DataServiceWrapper : IDataServiceWrapper
    {
        private GetData.GetDataClient _client;
        private readonly ILogger<DataServiceWrapper> _logger;
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


        public DataServiceWrapper(ILogger<DataServiceWrapper> logger, IConfigSettings configSettings)
        {
            _logger = logger;
            _config = configSettings;
            // The port number must match the port of the gRPC server.
            try
            {
                var channel = GrpcChannel.ForAddress($"{_config.ServerUrl}:{_config.ServerPort}", new GrpcChannelOptions { HttpHandler = httpClientHandler });
                _client = new GetData.GetDataClient(channel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }
        public async Task<GetDataResponse> GetDataAsync(GetDataRequest request)
        {
            try
            {
                return await _client.GET_DATAAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured, Source:{nameof(DataServiceWrapper)}");
                throw;
            }
        }

    }
}
