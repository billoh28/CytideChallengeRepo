using CytidelChallenge.Server.Model;
using System.Text;

namespace CytidelChallenge.Server.Services
{
    public class LoggingMiddlewareService
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddlewareService> _logger;
        private readonly string _logFilePath;
        private readonly string _highPrioityLogFilePath;

        public LoggingMiddlewareService(RequestDelegate next, ILogger<LoggingMiddlewareService> logger)
        {
            _next = next;
            _logger = logger;
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "api_requests.log");
            _highPrioityLogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "api_requests_high_priority.log");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var logMessage = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {request.Method} {request.Path}{request.QueryString}";

            if (isHighPriority(request))
            {
                await File.AppendAllTextAsync(_highPrioityLogFilePath, logMessage + Environment.NewLine);
            }
            else
            {
                await File.AppendAllTextAsync(_logFilePath, logMessage + Environment.NewLine);
            }

            _logger.LogInformation(logMessage);

            await _next(context);
        }

        public bool isHighPriority(HttpRequest request)
        {
            if (request.Method == "POST" || request.Method == "PUT")
            {
                try
                {
                    return request.Headers.ContainsKey("High-Priority") && request.Headers["High-Priority"].Equals("true");
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }
    }

    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddlewareService>();
        }
    }
}
