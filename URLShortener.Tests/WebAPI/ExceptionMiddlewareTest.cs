
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace URLShortener.Tests.WebAPI
{
    public class ExceptionMiddlewareTest
    {
        [Fact]
        public async Task Invoke_ShouldReturnInternalServerError()
        {
           //Arrange
           var middleware = new ExceptionMiddleware((innerHttpContext) =>
           {
               throw new Exception("Test exception");
           });

           var context = new DefaultHttpContext();
           context.Response.Body = new MemoryStream();

           // Act
           await middleware.Invoke(context);

           // Assert
           context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
