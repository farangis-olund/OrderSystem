using Serilog;
using System.Diagnostics;

namespace Shared.Utils
{
    public class Logger
    {
        private readonly ILogger _logger;

        public Logger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void LogInformation(string message)
        {
            _logger.Information($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [INFO] - {GetCurrentMethod()} - {message}");
        }

        public void LogError(string message)
        {
            _logger.Error($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [ERROR] - {GetCurrentMethod()} - {message}");
        }

        private string GetCurrentMethod()
        {
            var method = new StackFrame(2).GetMethod();
            return method?.Name ?? "UnknownMethod";
        }
    }
}
