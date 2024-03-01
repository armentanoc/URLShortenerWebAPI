
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using URLShortener.Application.Interfaces;
using URLShortener.ViewModels;
using URLShortener.WebAPI.Controllers;

namespace URLShortener.Tests.WebAPI
{
    public class UrlControllerTest
    {
        [Fact]
        public async Task Get_ShouldRetrieveOriginalUrl()
        {
            // Arrange
            var iLoggerMock = new Mock<ILogger<UrlController>>();
            var urlServiceMock = new Mock<IUrlService>();
            var controller = new UrlController(iLoggerMock.Object, urlServiceMock.Object);

            var shortenedUrl = "some-shortened-url";
            var originalUrl = "https://example.com";

            // Act
            var result = await controller.Get(shortenedUrl);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.Equal(200, okResult.StatusCode);

            // Add more assertions as needed
        }


        [Fact]
        public async Task Add_ShouldReturnTheUrlObjectCreated()
        {
            // Arrange
            var iLoggerMock = new Mock<ILogger<UrlController>>();
            var urlServiceMock = new Mock<IUrlService>();
            var controller = new UrlController(iLoggerMock.Object, urlServiceMock.Object);
            var urlDto = new UrlDTO { OriginalUrl = "https://example.com", ValidSeconds = 3600 };

            // Act
            var result = await controller.Add(urlDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result);
            var createdAtResult = (CreatedAtActionResult)result;
            Assert.Equal(201, createdAtResult.StatusCode);
        }
    }
}
