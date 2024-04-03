
using URLShortener.Domain;
using URLShortener.Infra.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace URLShortener.Application.Interfaces
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly string _domain;
        public UrlService(IUrlRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
            _domain =  _configuration.GetSection("AppSettings:ShortenedUrlDomain").Value ?? throw new Exception("Host not configured.");
        }
        public async Task<Url> GetOriginalUrlAsync(string slug)
        {
            string shortenedUrl = $"{_domain}/{slug}";
            string decodedUrl = System.Net.WebUtility.UrlDecode(shortenedUrl);
            Url retrievedUrl = await _repository.GetByUrlAsync(decodedUrl);

            if(!UrlIsExpired(retrievedUrl))
                return retrievedUrl;

            throw new ExpiredUrlException(decodedUrl);   
        }
        public async Task<Url> ShortenUrlAsync(string originalUrl)
        {
            string shortenedUrl = await GenerateShortenedUrl();
            string slug = shortenedUrl.Split('/').Last();
            DateTime expirationDate = GenerateRandomDuration();
            Url newUrl = new Url(originalUrl, shortenedUrl, expirationDate, slug);
            await _repository.AddAsync(newUrl);
            return newUrl;
        }
        public async Task<string> GenerateShortenedUrl()
        {
            string uniqueIdentifier = await GenerateUniqueIdentifier();
            return $"{_domain}/{uniqueIdentifier}";
        }
        public DateTime GenerateRandomDuration()
        {
            var minMinutesConfig = _configuration.GetSection("AppSettings:MinMinutesToExpire").Value;
            var maxMinutesConfig = _configuration.GetSection("AppSettings:MaxMinutesToExpire").Value;
            
            if (!uint.TryParse(minMinutesConfig, out uint minMinutes) || !uint.TryParse(maxMinutesConfig, out uint maxMinutes))
                throw new FailedToParseMinutesToUintException();

            if (minMinutes >= maxMinutes)
                throw new MinMinutesIsGreaterOrEqualThanMaxMinutesException();

            var randomMinutes = new Random().Next((int)minMinutes, (int)maxMinutes);
            return DateTime.Now.AddMinutes(randomMinutes);
        }
        public async Task<string> GenerateUniqueIdentifier()
        {
            var urlRegisters = await _repository.GetAllAsync();
            Url? lastUrl = urlRegisters.LastOrDefault();
            uint id = lastUrl?.Id ?? 0;
            return WebEncoders.Base64UrlEncode(BitConverter.GetBytes(id));
        }
        public bool UrlIsExpired(Url retrievedUrl)
        {
            if (retrievedUrl.ExpirationDate > DateTime.Now || retrievedUrl is null)
                return false;
            throw new ExpiredUrlException(retrievedUrl.ShortenedUrl);
        }
        public async Task<IEnumerable<Url>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}