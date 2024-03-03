using Microsoft.Extensions.Configuration;
using URLShortener.Domain;
using URLShortener.ViewModels;

namespace URLShortener.Application.Interfaces
{
    public interface IUrlService
    {
        Task<Url> GetOriginalUrlAsync(string shortenedUrl);
        Task<Url> ShortenUrlAsync(string originalUrl);
        string GetShortenedUrlDomain();
        Task<IEnumerable<Url>> GetAllAsync();
    }
}
