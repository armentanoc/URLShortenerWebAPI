
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using URLShortener.Domain;

namespace URLShortener.Application.Interfaces
{
    public interface IUrlService
    {
        Task<Url> GetOriginalUrlAsync(string slug);
        Task<Url> ShortenUrlAsync(string originalUrl);
        Task<string> GenerateShortenedUrl();
        Task<string> GenerateUniqueIdentifier();
        DateTime GenerateRandomDuration();
        bool UrlIsExpired(Url retrievedUrl);
        Task<IEnumerable<Url>> GetAllAsync();
    }
}
