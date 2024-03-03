
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text.Json;

public class ExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var ex = context.Exception;
        var response = context.HttpContext.Response;
        response.ContentType = "application/json";
        _logger.LogError(ex, "Unhandled Exception");

        switch (ex)
        {
            case InvalidOperationException invalidOperationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var result = JsonSerializer.Serialize(new { error = ex.Message, statusCode = response.StatusCode });
        _logger.LogError(result);
        await response.WriteAsync(result);
        context.ExceptionHandled = true;
    }
}
