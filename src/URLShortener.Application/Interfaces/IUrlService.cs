
using URLShortener.Domain;

namespace URLShortener.Application.Interfaces
{
    public interface IUrlService
    {
        Task<Url> GetOriginalUrlAsync(string slug);
        Task<Url> ShortenUrlAsync(string originalUrl);
        string GetShortenedUrlDomain();
        Task<IEnumerable<Url>> GetAllAsync();
    }
}
