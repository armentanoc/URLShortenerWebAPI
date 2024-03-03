
using Microsoft.AspNetCore.Http;
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
        Console.WriteLine($"Exception: {ex.Message}");
        var response = context.HttpContext.Response;
        response.ContentType = "application/json";

        switch (ex)
        {
            case InvalidOperationException invalidOperationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var result = JsonSerializer.Serialize(new { error = ex.Message, statusCode = response.StatusCode
        });
        _logger.LogError(result);
        //await response.WriteAsync(result);
        await Task.CompletedTask;
    }
}
