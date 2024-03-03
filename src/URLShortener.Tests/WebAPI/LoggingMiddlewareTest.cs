
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using URLShortener.WebAPI.Middlewares;

namespace URLShortener.Tests.WebAPI
{
    public class LoggingMiddlewareTest
    {
        [Fact]
        public async Task InvokeAsync_ShouldLogTwiceForBeginAndEndOfRequest()
        {
            // Arrange
            var EXPECTED_TIMES_TO_LOG = 2;
            var iLoggerMock = new Mock<ILogger<LoggingMiddleware>>();
            var middleware = new LoggingMiddleware(next: (context) => Task.CompletedTask, iLoggerMock.Object);
            var context = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            iLoggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Exactly(EXPECTED_TIMES_TO_LOG)); 
        }
    }
}

