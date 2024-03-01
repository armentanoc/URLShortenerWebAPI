
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;

namespace URLShortener.Tests.WebAPI
{
    public class ExceptionFilterTest
    {
        [Fact]
        public async Task Invoke_ShouldReturnInternalServerError()
        {
            // Arrange
            var iLoggerMock = new Mock<ILogger<ExceptionFilter>>();
            var filter = new ExceptionFilter(iLoggerMock.Object);
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var exceptionContext = new ExceptionContext(new ActionContext(context, new RouteData(), new ActionDescriptor()), new List<IFilterMetadata>())
            {
                Exception = new Exception("Test exception"),
            };

            // Act
            await filter.OnExceptionAsync(exceptionContext);

            // Assert
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
