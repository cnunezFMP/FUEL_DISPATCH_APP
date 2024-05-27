using Serilog;
namespace FUEL_DISPATCH_API.Utils
{
    public static class LoggerClass
    {
        private static readonly string newId = Guid.NewGuid().ToString();
        private static readonly string dirPath = "C:\\LogsDirectory";
        private static readonly string log = Path.Combine(dirPath, "log" + newId + ".txt");
        private static readonly ILogger _logger = new LoggerConfiguration()
                                                      .WriteTo.File(log)
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
