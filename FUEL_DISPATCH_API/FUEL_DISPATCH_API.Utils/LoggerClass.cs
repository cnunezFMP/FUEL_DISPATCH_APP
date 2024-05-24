using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.Utils
{
    public static class LoggerClass
    {
        private static readonly string path = @"C:\LogsDirectory\logg.txt";
        private static readonly ILogger _logger = new LoggerConfiguration()
                                                      .WriteTo.File(path)
                                                      .CreateLogger();
        public static void LogInfo(string message)
        {
            _logger.Information(message);
        }
        public static void LogWarning(string message)
        {
            _logger.Warning(message);
        }
        public static void LogError(string message)
        {
            _logger.Error(message);
        }
    }
}
