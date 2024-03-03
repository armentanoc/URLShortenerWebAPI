
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
        public async Task<UrlDTO> GetOriginalUrlAsync(string shortenedUrl)
        {
            var retrievedUrl = await _repository.GetByUrlAsync(shortenedUrl);

            if (retrievedUrl is Url url)
                new UrlDTO(retrievedUrl.OriginalUrl, retrievedUrl.ShortenedUrl, retrievedUrl.ExpirationDate);

            throw new Exception(shortenedUrl + " not found.");
        }

        public async Task<Url> ShortenUrlAsync(string originalUrl)
        {
            var shortenedUrl = await GenerateShortenedUrl();
            var expirationDate = GenerateRandomDuration();
            var newUrl = new Url(originalUrl, shortenedUrl, expirationDate);
            var addedUrl = await _repository.AddAsync(newUrl);
            return addedUrl;
        }

        private async Task<string> GenerateShortenedUrl()
        {
            string uniqueIdentifier = await GenerateUniqueIdentifier();
            string localhostAddress = "http://localhost:5000";
            string shortenedUrl = $"{localhostAddress}/{uniqueIdentifier}";
            return shortenedUrl;
        }

        private DateTime GenerateRandomDuration()
        {
            var random = new Random();
            var randomSeconds = random.Next(2, 11);
            return DateTime.Now.AddSeconds(randomSeconds);
        }
        private async Task<string> GenerateUniqueIdentifier()
        {
            var urlRegisters = await _repository.GetAllAsync();
            var lastUrl = urlRegisters.LastOrDefault();
            return lastUrl != null ? ConvertTo7LetterHash(lastUrl.Id) : "aaaaaaa"; 
        }
        private string ConvertTo7LetterHash(uint id)
        {
            return id.ToString("X").Substring(0, 7); 
        }
    }
}