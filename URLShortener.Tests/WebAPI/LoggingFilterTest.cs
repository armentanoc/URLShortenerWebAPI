
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using URLShortener.WebAPI.Filters;
using Microsoft.AspNetCore.Routing;

namespace URLShortener.Tests.WebAPI
{
    public class LoggingFilterTest
    {
        [Fact]
        public async Task Invoke_ShouldLogRequestAndResponse()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<LoggingFilter>>();
            var filter = new LoggingFilter(loggerMock.Object);

            var context = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor()
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object
            );

            var executionContext = new ActionExecutedContext(
                context,
                new List<IFilterMetadata>(),
                new Mock<Controller>().Object
            );

            // Act
            filter.OnActionExecuting(context);
            filter.OnActionExecuted(executionContext);

            // Assert
            loggerMock.Verify(logger => logger.LogInformation(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
