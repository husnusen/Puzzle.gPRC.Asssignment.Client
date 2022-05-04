using GrpcHealthChecksClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.gPRC.Asssignment.Client.Services
{
    public interface IHealthChecksService
    {
        Task<HealthReport> CheckHealthAsync();
    }
}
