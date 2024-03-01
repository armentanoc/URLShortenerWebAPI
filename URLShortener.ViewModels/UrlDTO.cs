
namespace URLShortener.ViewModels
{
    public class UrlDTO
    {
        public string ShortenedUrl { get; set; }
        public string OriginalUrl { get; set; }
        public int ValidSeconds { get; set; }

        public UrlDTO(string shortenedUrl, string originalUrl, int validSeconds)
        {
            ShortenedUrl = shortenedUrl;
            OriginalUrl = originalUrl;
            ValidSeconds = validSeconds;
        }

        public UrlDTO()
        {
            // Required by EF
        }
    }
}
