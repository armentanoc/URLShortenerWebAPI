using Microsoft.AspNetCore.Mvc.Filters;

namespace URLShortener.WebAPI.Filters
{
    public class LoggingFilter : IActionFilter
    {
        private readonly ILogger<LoggingFilter> _logger;

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Request: {context.HttpContext.Request.Method} {context.HttpContext.Request.Path}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Response: {context.HttpContext.Response.StatusCode}");
        }
    }
}
