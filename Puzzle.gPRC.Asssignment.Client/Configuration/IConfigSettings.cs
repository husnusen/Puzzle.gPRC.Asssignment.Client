using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.gPRC.Asssignment.Client.Configuration
{
    public interface IConfigSettings
    {
        string ServerUrl { get; }
        int ServerPort { get; }
    }
}
