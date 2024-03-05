
using System.Diagnostics.CodeAnalysis;

namespace URLShortener.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class UrlDTO
    {
        public string ShortenedUrl { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime ExpirationDate { get; set; }

        public UrlDTO(string shortenedUrl, string originalUrl, DateTime expirationDate)
        {
            ShortenedUrl = shortenedUrl;
            OriginalUrl = originalUrl;
            ExpirationDate = expirationDate;
        }
    }
}
