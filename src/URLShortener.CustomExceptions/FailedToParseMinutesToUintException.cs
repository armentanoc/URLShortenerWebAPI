
using System.Diagnostics.CodeAnalysis;

namespace URLShortener.Application.Interfaces
{
    [ExcludeFromCodeCoverage]
    public class FailedToParseMinutesToUintException : Exception
    {
        public FailedToParseMinutesToUintException(string message = "Invalid configuration for expiration minutes. Check appsettings.json file.")
            : base(message)
        {
        }
    }
}