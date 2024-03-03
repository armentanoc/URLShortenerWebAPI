
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using URLShortener.Application.Interfaces;
using URLShortener.Domain;
using URLShortener.Infra.Interfaces;

namespace URLShortener.Tests.Application
{
    public class UrlServiceTest
    {
        [Fact]
        public async Task GenerateShortenedUrl_ShouldReturnCorrectUrl()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("AppSettings:ShortenedUrlDomain").Value).Returns("https://localhost:1234");

            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            // Act
            var shortenedUrl = await urlService.GenerateShortenedUrl();

            // Assert
            Assert.NotNull(shortenedUrl);
            Assert.StartsWith("https://localhost:1234/", shortenedUrl);
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldRetrieveOriginalUrl()
        {
            // Arrange
            var urlRepositoryMock = new Mock<IUrlRepository>();
            var configurationMock = new Mock<IConfiguration>();
            var urlService = new UrlService(urlRepositoryMock.Object, configurationMock.Object);

            // Mock the behavior of GetByUrlAsync
            urlRepositoryMock.Setup(repo => repo.GetByUrlAsync("url/short"))
                             .ReturnsAsync(new Url("https://exemplo.com", "url/short", DateTime.Now.AddDays(1), "short"));

            // Act
            var response = await urlService.GetOriginalUrlAsync("url/short");
            var originalUrl = response.OriginalUrl;

            // Assert
            Assert.Equal("https://exemplo.com", originalUrl);
        }

        [Fact]
        public async Task ShortenUrlAsync_ShouldCreateShortenedUrl()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            var urlService = new UrlService(urlRepository.Object, configuration.Object);
            configuration.Setup(x => x.GetSection("AppSettings:ShortenedUrlDomain").Value).Returns("https://localhost:1234");

            // Act
            var shortenedUrlObject = await urlService.ShortenUrlAsync("https://example.com");
            var shortenedUrlString = shortenedUrlObject.ShortenedUrl;

            // Assert
            Assert.NotNull(shortenedUrlString);
        }

        [Fact]
        public void GetShortenedUrlDomain_ShouldReturnConfiguredDomain()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("AppSettings:ShortenedUrlDomain").Value).Returns("https://localhost:1234");

            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            // Act
            var domain = urlService.GetShortenedUrlDomain();

            // Assert
            Assert.Equal("https://localhost:1234", domain);
        }

        [Fact]
        public void GenerateRandomDuration_ShouldReturnFutureDateTime()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            // Act
            var result = urlService.GenerateRandomDuration();

            // Assert
            Assert.True(result > DateTime.Now);
        }

        [Fact]
        public async Task GenerateUniqueIdentifier_ShouldReturnBase64EncodedId()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            // Act
            var result = await urlService.GenerateUniqueIdentifier();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void UrlIsExpired_ShouldNotThrowExceptionForFutureDate()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            var futureDate = DateTime.Now.AddDays(1);
            var url = new Url(futureDate);

            // Act & Assert
            urlService.Invoking(service => service.UrlIsExpired(url))
                .Should()
                .NotThrow<Exception>();
        }

        [Fact]
        public void UrlIsExpired_ShouldThrowExceptionForExpiredDate()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            var pastDate = DateTime.Now.AddDays(-1);
            var url = new Url(pastDate);

            // Act & Assert
            urlService.Invoking(service => service.UrlIsExpired(url))
                .Should()
                .Throw<Exception>()
                .WithMessage("This URL has expired.");
        }
    }
}