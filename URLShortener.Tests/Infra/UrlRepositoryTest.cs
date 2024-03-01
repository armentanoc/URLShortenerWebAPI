
using Microsoft.EntityFrameworkCore;
using Moq;
using URLShortener.Application.Interfaces;
using URLShortener.Domain;
using URLShortener.Infra.Context;
using URLShortener.Infra.Interfaces;
using URLShortener.Infra.Repositories;

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
                            .ReturnsAsync(new Url("https://example.com", "shortened-url", DateTime.Now.AddMinutes(2)));

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
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var mockContext = new AppDbContext(options);
            var repository = new UrlRepository(mockContext);

            var shortenedUrlToAdd = new Url(
                "https://www.example.com",
                "abc123",
                DateTime.Now.AddMinutes(2)
            );

            // Act
            var result = await repository.AddAsync(shortenedUrlToAdd);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(shortenedUrlToAdd, result);
        }
    }
}
