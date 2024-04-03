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
        #region Setup Mocks
        private Mock<IUrlRepository> SetupUrlRepository()
        {
            var urlRepository = new Mock<IUrlRepository>();
            return urlRepository;
        }

        private Mock<IConfiguration> SetupConfiguration(string shortenedUrlDomain)
        {
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection("AppSettings:ShortenedUrlDomain").Value)
                         .Returns(shortenedUrlDomain);
            return configuration;
        }
        #endregion

        #region Fact Tests
        [Fact]
        public async Task GenerateShortenedUrl_ShouldReturnUrlWithCorrectFormat()
        {
            // Arrange
            var urlRepository = SetupUrlRepository();
            var configuration = SetupConfiguration("https://localhost:1234");
            var sut = new UrlService(urlRepository.Object, configuration.Object);

            // Act
            var shortenedUrl = await sut.GenerateShortenedUrl();

            // Assert
            shortenedUrl.Should().NotBeNullOrEmpty();
            shortenedUrl.Should().StartWith("https://localhost:1234/");
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldRetrieveOriginalUrl()
        {
            // Arrange
            var urlRepository = SetupUrlRepository();
            var configuration = SetupConfiguration("https://localhost:1234");
            var sut = new UrlService(urlRepository.Object, configuration.Object);

            urlRepository.Setup(repo => repo.GetByUrlAsync("https://localhost:1234/short"))
                         .ReturnsAsync(new Url("https://example.com", "https://localhost:1234/short", DateTime.UtcNow.AddDays(1), "short"));

            // Act
            var response = await sut.GetOriginalUrlAsync("short");
            var originalUrl = response.OriginalUrl;

            // Assert
            originalUrl.Should().Be("https://example.com");
        }

        [Fact]
        public async Task GetOriginalUrlAsync_ShouldThrowExceptionWhenUrlIsExpired()
        {
            // Arrange
            var urlRepository = SetupUrlRepository();
            var configuration = SetupConfiguration("https://localhost:1234");
            var sut = new UrlService(urlRepository.Object, configuration.Object);

            urlRepository.Setup(repo => repo.GetByUrlAsync("https://localhost:1234/expired"))
                         .ReturnsAsync(new Url("https://example.com", "https://localhost:1234/expired", DateTime.UtcNow.AddDays(-1), "expired"));

            // Act & Assert
            Func<Task> act = async () => await sut.GetOriginalUrlAsync("expired");
            await act.Should().ThrowAsync<ExpiredUrlException>();
        }

        [Fact]
        public async Task ShortenUrlAsync_ShouldCreateShortenedUrl()
        {
            // Arrange
            var urlRepository = SetupUrlRepository();
            var configuration = SetupConfiguration("https://localhost:1234");
            var sut = new UrlService(urlRepository.Object, configuration.Object);
            configuration.Setup(x => x.GetSection("AppSettings:MinMinutesToExpire").Value).Returns("20");
            configuration.Setup(x => x.GetSection("AppSettings:MaxMinutesToExpire").Value).Returns("500");

            // Act
            var shortenedUrlObject = await sut.ShortenUrlAsync("https://example.com");

            // Assert
            shortenedUrlObject.ShortenedUrl.Should().NotBeNullOrEmpty();
            shortenedUrlObject.Slug.Should().MatchRegex("^[^0-9]{6}$");
        }

        [Fact]
        public void GenerateRandomDuration_ShouldReturnFutureDateTime()
        {
            // Arrange
            int MIN_MINUTES_TO_EXPIRE = 20;
            int MAX_MINUTES_TO_EXPIRE = 500;

            var urlRepository = SetupUrlRepository();
            var configuration = SetupConfiguration("https://localhost:1234");

            configuration.Setup(x => x.GetSection("AppSettings:MinMinutesToExpire").Value)
                         .Returns(MIN_MINUTES_TO_EXPIRE.ToString());

            configuration.Setup(x => x.GetSection("AppSettings:MaxMinutesToExpire").Value)
                         .Returns(MAX_MINUTES_TO_EXPIRE.ToString());

            var sut = new UrlService(urlRepository.Object, configuration.Object);

            // Act
            var result = sut.GenerateRandomDuration();

            // Assert
            result.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public void GenerateRandomDuration_ShouldNotThrowExceptionWhenValidConfiguration()
        {
            // Arrange
            uint UINT_MIN_MINUTES_TO_EXPIRE = 20;
            uint UINT_MAX_MINUTES_TO_EXPIRE = 30;

            var urlRepository = SetupUrlRepository();
            var configuration = SetupConfiguration("https://localhost:1234");

            configuration.Setup(x => x.GetSection("AppSettings:MinMinutesToExpire").Value)
                         .Returns(UINT_MIN_MINUTES_TO_EXPIRE.ToString());

            configuration.Setup(x => x.GetSection("AppSettings:MaxMinutesToExpire").Value)
                         .Returns(UINT_MAX_MINUTES_TO_EXPIRE.ToString());

            var sut = new UrlService(urlRepository.Object, configuration.Object);

            // Act & Assert
            sut.Invoking(service => service.GenerateRandomDuration())
                      .Should().NotThrow<FailedToParseMinutesToUintException>();
        }

        [Fact]
        public async Task GenerateUniqueIdentifier_ShouldReturnBase64EncodedId()
        {
            // Arrange
            var urlRepository = SetupUrlRepository();
            var configuration = SetupConfiguration("https://localhost:1234");
            var sut = new UrlService(urlRepository.Object, configuration.Object);

            // Act
            var result = await sut.GenerateUniqueIdentifier();

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty();
            result.Should().MatchRegex("^[^0-9]{6}$");
        }

        #endregion

        #region Theory With InlineData Tests
        [Theory]
        [InlineData("2", "4.58")]
        [InlineData("DOIS", "6")]
        [InlineData("-1", "30")]
        public void GenerateRandomDuration_ShouldThrowExceptionWhenInvalidConfiguration(string minMinutes, string maxMinutes)
        {
            // Arrange
            var urlRepository = SetupUrlRepository();
            var configuration = SetupConfiguration("https://localhost:1234");

            configuration.Setup(x => x.GetSection("AppSettings:MinMinutesToExpire").Value)
                         .Returns(minMinutes);

            configuration.Setup(x => x.GetSection("AppSettings:MaxMinutesToExpire").Value)
                         .Returns(maxMinutes);

            var sut = new UrlService(urlRepository.Object, configuration.Object);

            // Act & Assert
            sut.Invoking(service => service.GenerateRandomDuration())
                      .Should().Throw<FailedToParseMinutesToUintException>();
        }
        #endregion

        #region Theory With MemberData Tests
        [Theory]
        [MemberData(nameof(UrlExpiryTestData))]
        public void UrlIsExpired_ShouldThrowExceptionForExpiredDate(DateTime date, bool expectException)
        {
            // Arrange
            var urlRepository = SetupUrlRepository();
            var configuration = SetupConfiguration("https://localhost:1234");
            var sut = new UrlService(urlRepository.Object, configuration.Object);

            var url = new Url(date);

            // Act & Assert
            if (expectException)
            {
                sut.Invoking(service => service.UrlIsExpired(url))
                         .Should().Throw<ExpiredUrlException>();
            }
            else
            {
                sut.Invoking(service => service.UrlIsExpired(url))
                         .Should().NotThrow<ExpiredUrlException>();
            }
        }
        #endregion

        #region Test Data
        public static IEnumerable<object[]> UrlExpiryTestData()
        {
            yield return new object[] { DateTime.Now.AddDays(1), false };
            yield return new object[] { DateTime.Now.AddDays(-1), true };
        }
        #endregion
    }
}
