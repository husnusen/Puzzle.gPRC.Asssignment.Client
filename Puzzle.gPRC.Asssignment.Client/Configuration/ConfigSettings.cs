using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle.gPRC.Asssignment.Client.Configuration
{
    internal class ConfigSettings : IConfigSettings
    {
        private readonly IConfiguration _config;

        public ConfigSettings(IConfiguration configuration)
        {
            _config = configuration;
        }
        public string ServerUrl => _config.GetValue<string>("ServerUrl");

        public int ServerPort => _config.GetValue<int>("ServerPort");
    }
}
