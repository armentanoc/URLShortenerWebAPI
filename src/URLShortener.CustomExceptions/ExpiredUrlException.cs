
namespace URLShortener.Application.Interfaces
{
    public class ExpiredUrlException : Exception
    {
        public ExpiredUrlException(string? url)
            : base($"This URL {url} has expired.")
        {
            
        }
    }
}