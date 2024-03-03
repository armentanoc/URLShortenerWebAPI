
using System.ComponentModel.DataAnnotations;

namespace URLShortener.ViewModels
{
    public class UrlRequest
    {
        [Required(ErrorMessage = "Original url is required.")]
        public string OriginalUrl { get; init; }
      
        public UrlRequest(string originalUrl)
        {
            OriginalUrl = originalUrl;
        }
    }
}
