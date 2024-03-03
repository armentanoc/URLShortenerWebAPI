
using Microsoft.EntityFrameworkCore;
using URLShortener.Domain;
using URLShortener.Infra.Context;
using URLShortener.Infra.Repositories;

namespace URLShortener.Tests.Infra
{
    public class UrlRepositoryTest
    {

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

        [Fact]
        public async Task GetAsync_ShouldRetrieveOriginalUrl()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase_GetAsync")
                .Options;

            var mockContext = new AppDbContext(options);
            var repository = new UrlRepository(mockContext);

            var originalUrl = "https://www.example.com";
            var shortenedUrl = "abc123";
            var expirationDate = DateTime.Now.AddMinutes(2);

            var urlToAdd = new Url(originalUrl, shortenedUrl, expirationDate);
            var addedUrl = await repository.AddAsync(urlToAdd);

            // Act
            var retrievedUrl = await repository.GetAsync(addedUrl.Id);

            // Assert
            Assert.NotNull(retrievedUrl);
            Assert.Equal(originalUrl, retrievedUrl.OriginalUrl);
            Assert.Equal(shortenedUrl, retrievedUrl.ShortenedUrl);
            Assert.Equal(expirationDate, retrievedUrl.ExpirationDate);
        }
    }
}
