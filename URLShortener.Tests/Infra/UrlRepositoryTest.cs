
using Moq;
using URLShortener.Application.Interfaces;
using URLShortener.Domain;
using URLShortener.Infra.Interfaces;

namespace URLShortener.Tests.Infra
{
    public class UrlRepositoryTest
    {
        [Fact]
        public async Task GetAsync_ShouldRetrieveOriginalUrl()
        {
            // Arrange
            var urlRepositoryMock = new Mock<IUrlRepository>();
            urlRepositoryMock.Setup(repo => repo.GetByUrlAsync("shortened-url"))
                            .ReturnsAsync(new Url ("https://example.com", "shortened-url", DateTime.Now.AddMinutes(2)));

            var urlService = new UrlService(urlRepositoryMock.Object);

            // Act
            var result = await urlService.GetOriginalUrlAsync("shortened-url");
            var originalUrl = result.OriginalUrl;

            // Assert
            Assert.NotNull(originalUrl);
            Assert.Equal("https://example.com", originalUrl);
        }

        [Fact]
        public async Task AddAsync_ShouldCreateShortenedUrl()
        {
            // Arrange
            var urlRepositoryMock = new Mock<IUrlRepository>();
            var urlService = new UrlService(urlRepositoryMock.Object);

            // Act
            await urlService.ShortenUrlAsync("https://example.com");

            // Assert
            urlRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Url>()), Times.Once);
        }
    }
}
