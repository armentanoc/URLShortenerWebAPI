
using URLShortener.Domain;
using URLShortener.Infra.Interfaces;
using URLShortener.ViewModels;

namespace URLShortener.Application.Interfaces
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _repository;
        public UrlService(IUrlRepository repository)
        {
            _repository = repository;
        }
        public Task<UrlDTO> GetOriginalUrlAsync(string shortenedUrl)
        {
            throw new NotImplementedException();
        }

        public Task<Url> ShortenUrlAsync(string originalUrl)
        {
            throw new NotImplementedException();
        }
    }
}
