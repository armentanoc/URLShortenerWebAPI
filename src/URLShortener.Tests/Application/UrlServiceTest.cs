
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
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
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("AppSettings:ShortenedUrlDomain").Value).Returns("https://localhost:1234");

            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            urlRepository.Setup(repo => repo.GetByUrlAsync("https://localhost:1234/short"))
                         .ReturnsAsync(new Url("https://example.com", "https://localhost:1234/short", DateTime.Now.AddDays(1), "short"));

            // Act
            var response = await urlService.GetOriginalUrlAsync("short");
            var originalUrl = response.OriginalUrl;

            // Assert
            Assert.Equal("https://example.com", originalUrl);
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldThrowExceptionWhenUrlIsExpired()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("AppSettings:ShortenedUrlDomain").Value).Returns("https://localhost:1234");

            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            urlRepository.Setup(repo => repo.GetByUrlAsync("https://localhost:1234/expired"))
                         .ReturnsAsync(new Url("https://example.com", "https://localhost:1234/expired", DateTime.Now.AddDays(-1), "expired"));

            // Act & Assert
            await Assert.ThrowsAsync<ExpiredUrlException>(async () => await urlService.GetOriginalUrlAsync("expired"));
        }

        [Fact]
        public async Task ShortenUrlAsync_ShouldCreateShortenedUrl()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            var urlService = new UrlService(urlRepository.Object, configuration.Object);
            configuration.Setup(x => x.GetSection("AppSettings:ShortenedUrlDomain").Value).Returns("https://localhost:1234");
            configuration.Setup(x => x.GetSection("AppSettings:MinMinutesToExpire").Value).Returns("20");
            configuration.Setup(x => x.GetSection("AppSettings:MaxMinutesToExpire").Value).Returns("500");

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
            configuration.Setup(x => x.GetSection("AppSettings:MinMinutesToExpire").Value).Returns("20");
            configuration.Setup(x => x.GetSection("AppSettings:MaxMinutesToExpire").Value).Returns("500");
            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            // Act
            var result = urlService.GenerateRandomDuration();

            // Assert
            Assert.True(result > DateTime.Now);
        }

        [Fact]
        public void GenerateRandomDuration_ShouldThrowExceptionWhenInvalidConfiguration()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("AppSettings:MinMinutesToExpire").Value).Returns("teste");
            configuration.Setup(x => x.GetSection("AppSettings:MaxMinutesToExpire").Value).Returns("teste");
            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            // Act & Assert

            urlService.Invoking(service => service.GenerateRandomDuration())
                 .Should()
                 .Throw<FailedToParseMinutesToUintException>();
        }

        [Fact]
        public void GenerateRandomDuration_ShouldNotThrowExceptionWhenValidConfiguration()
        {
            // Arrange
            var urlRepository = new Mock<IUrlRepository>();
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("AppSettings:MinMinutesToExpire").Value).Returns("20");
            configuration.Setup(x => x.GetSection("AppSettings:MaxMinutesToExpire").Value).Returns("500");
            var urlService = new UrlService(urlRepository.Object, configuration.Object);

            // Act & Assert

            urlService.Invoking(service => service.GenerateRandomDuration())
                 .Should()
                 .NotThrow<Exception>();
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
                .Throw<ExpiredUrlException>();
        }
    }
}