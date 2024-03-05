
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace URLShortener.ViewModels
{
    [ExcludeFromCodeCoverage]
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
