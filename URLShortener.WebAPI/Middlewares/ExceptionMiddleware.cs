
using System.Net;
using System.Text.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        throw new NotImplementedException();
        //Console.WriteLine($"Exception: {ex.Message}");
        //var response = context.Response;
        //response.ContentType = "application/json";

        //switch (ex)
        //{
        //    case InvalidOperationException invalidOperationException:
        //        response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        break;

        //    default:
        //        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //        break;
        //}

        //var result = JsonSerializer.Serialize(new { error = ex.Message });
        //return response.WriteAsync(result);
    }
}
