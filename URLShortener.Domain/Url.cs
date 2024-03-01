
namespace URLShortener.Domain
{
    public class Url : BaseEntity
    {
        public string OriginalUrl { get; private set; }
        public string ShortenedUrl { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public object Assert { get; set; }

        public Url()
        {
            // Required by EF
        }

        public Url(string originalUrl, string shortenedUrl, DateTime expirationDate)
        {
            OriginalUrl = originalUrl;
            ShortenedUrl = shortenedUrl;
            ExpirationDate = expirationDate;
        }
    }
}
