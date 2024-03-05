
using System.Diagnostics.CodeAnalysis;

namespace URLShortener.Application.Interfaces
{
    [ExcludeFromCodeCoverage]
    public class MinMinutesIsGreaterOrEqualThanMaxMinutesException : Exception
    { 
        public MinMinutesIsGreaterOrEqualThanMaxMinutesException(string? message = "Invalid configuration. MinMinutesToExpire should be less than MaxMinutesToExpire.") : base(message)
        {
        }
    }
}