using URLShortener.Domain;
using URLShortener.ViewModels;

namespace URLShortener.Application.Interfaces
{
    public interface IUrlService
    {
        Task<UrlDTO> GetOriginalUrlAsync(string shortenedUrl);
        Task<Url> ShortenUrlAsync(string originalUrl);
    }
}
