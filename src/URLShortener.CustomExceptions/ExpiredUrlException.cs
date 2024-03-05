
using System.Diagnostics.CodeAnalysis;

namespace URLShortener.Application.Interfaces
{
    [ExcludeFromCodeCoverage]
    public class ExpiredUrlException : Exception
    {
        public ExpiredUrlException(string? url)
            : base($"This URL {url} has expired.")
        {
            
        }
    }
}