
using Moq;
using URLShortener.Application.Interfaces;
using URLShortener.Domain;
using URLShortener.Infra.Interfaces;

namespace URLShortener.Tests.Application
{
    public class UrlServiceTest
    {
        [Fact]
        public async Task GetOriginalUrlAsync_ShouldRetrieveOriginalUrl()
        {
            // Arrange
            var urlRepositoryMock = new Mock<IUrlRepository>();
            var urlService = new UrlService(urlRepositoryMock.Object);

            // Mock the behavior of GetByUrlAsync
            urlRepositoryMock.Setup(repo => repo.GetByUrlAsync("short"))
                             .ReturnsAsync(new Url("https://exemplo.com", "short", DateTime.Now.AddDays(1)));

            // Act
            var response = await urlService.GetOriginalUrlAsync("short");
            var originalUrl = response.OriginalUrl;

            // Assert
            Assert.Equal("https://exemplo.com", originalUrl);
        }

        [Fact]
        public async Task ShortenUrlAsync_ShouldCreateShortenedUrl()
        {
            // Arrange
            var urlRepositoryMock = new Mock<IUrlRepository>();
            var urlService = new UrlService(urlRepositoryMock.Object);

            // Act
            var shortenedUrlObject = await urlService.ShortenUrlAsync("https://example.com");
            var shortenedUrlString = shortenedUrlObject.ShortenedUrl;

            // Extract the last 7 characters as the slug
            var slug = shortenedUrlString.Substring(Math.Max(0, shortenedUrlString.Length - 7));

            // Assert
            Assert.NotNull(shortenedUrlString);
            Assert.Equal(7, slug.Length);
        }
    }
}