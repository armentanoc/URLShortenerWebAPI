
namespace URLShortener.ViewModels
{
    public class UrlRequest
    {
        public string OriginalUrl { get; init; }
        public UrlRequest(string originalUrl)
        {
            OriginalUrl = originalUrl;
        }
    }
}
