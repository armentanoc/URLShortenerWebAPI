
using System.Diagnostics.CodeAnalysis;

namespace URLShortener.Domain
{
    [ExcludeFromCodeCoverage]
    public class Url : BaseEntity
    {
        public string OriginalUrl { get; private set; }
        public string ShortenedUrl { get; private set; }
        public string Slug { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        public Url()
        {
            // Required by EF
        }

        public Url(string originalUrl, string shortenedUrl, DateTime expirationDate, string slug)
        {
            OriginalUrl = originalUrl;
            ShortenedUrl = shortenedUrl;
            ExpirationDate = expirationDate;
            Slug = slug;
        }

        public Url(DateTime expirationDate)
        {
            // Required by test
            ExpirationDate = expirationDate;
        }

        public override string ToString()
        {
            return "OriginalUrl: " + OriginalUrl + ", ShortenedUrl: " + ShortenedUrl + ", ExpirationDate: " + ExpirationDate;
        }
    }
}
