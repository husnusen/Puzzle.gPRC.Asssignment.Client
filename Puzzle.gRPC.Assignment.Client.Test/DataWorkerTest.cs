using GrpcGetDataClient;
using Microsoft.Extensions.Logging;
using Moq;
using Puzzle.gPRC.Asssignment.Client;
using Puzzle.gPRC.Asssignment.Client.BackgroundServices;
using Puzzle.gPRC.Asssignment.Client.Services;
using System.Threading;
using System.Threading.Tasks;
using Xbehave;
using Xunit;

namespace Puzzle.gRPC.Assignment.Client.Test
{
    public class DataWorkerTest
    {
        private Mock<IDataServiceWrapper> _serviceWrapperMock;
        private Mock<ILogger<DataWorker>> _loggerMock;
        private DataWorker _worker;
        public DataWorkerTest()
        {
            _serviceWrapperMock = new Mock<IDataServiceWrapper>();
            _loggerMock = new Mock<ILogger<DataWorker>>();
        }

        [Scenario]
        public async Task GetData_Service_Called_Successfully ()
        {
            "Set up the call".x(() => {
                _serviceWrapperMock.Setup(_ => _.GetDataAsync(It.IsAny<GetDataRequest>())).ReturnsAsync(GetDataResponseMock);
                _worker = new DataWorker(_loggerMock.Object, _serviceWrapperMock.Object);
               
            });
            "Call service ".x(async () =>
            {
                await _worker.StartAsync(CancellationToken.None);
            });
            "Verify ".x(() =>
            {
                _serviceWrapperMock.Verify(_ => _.GetDataAsync(It.IsAny<GetDataRequest>()), Times.Once);
            });

        }

        private GetDataResponse GetDataResponseMock
        {
            get
            {
                var columnValue = new[] { "value1" };
                var response = new GetDataResponse();
                response.Query = "";
                response.CurrentCount = 1;
                response.TotalCount = 2;
                response.Schema.AddRange(new[] { "Column1" });
                for (int i = 0; i < 2; i++)
                {
                    var responseData = new Data();
                    responseData.ColumnValue.Add(columnValue);
                    response.Data.Add(responseData);
                }
                return response;
            }
        }


    }
}