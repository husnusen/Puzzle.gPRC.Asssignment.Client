using GrpcGetDataClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.gPRC.Asssignment.Client.Services
{
   public interface IDataServiceWrapper
    {
        Task<GetDataResponse> GetDataAsync(GetDataRequest request);
    }
}
