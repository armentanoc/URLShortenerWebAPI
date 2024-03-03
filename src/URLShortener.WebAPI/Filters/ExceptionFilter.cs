
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using URLShortener.Application.Interfaces;
using URLShortener.Infra.Repositories;

public class ExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        context.ExceptionHandled = false;
        var ex = context.Exception;
        int statusCode;

        var response = context.HttpContext.Response;
        response.ContentType = "application/json";

        switch (ex)
        {
            case EntityAlreadyExistsException _:
                statusCode = StatusCodes.Status409Conflict;
                break;

            case EntityNotFoundException _:
                statusCode = StatusCodes.Status404NotFound;
                break;

            case ExpiredUrlException _:
                statusCode = StatusCodes.Status410Gone;
                break;

            case FailedToParseMinutesToUintException _:
            case MinMinutesIsGreaterOrEqualThanMaxMinutesException _:
            case InvalidOperationException _:
                statusCode = StatusCodes.Status400BadRequest;
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        var objectResponse = new
        {
            Error = new
            {
                message = context.Exception.Message,
                statusCode = statusCode
            }
        };

        context.Result = new ObjectResult(objectResponse)
        {
            StatusCode = statusCode
        };

        _logger.LogError(objectResponse.ToString());
        
        await Task.CompletedTask;
    }
}
