
namespace URLShortener.Application.Interfaces
{
    public class MinMinutesIsGreaterOrEqualThanMaxMinutesException : Exception
    { 
        public MinMinutesIsGreaterOrEqualThanMaxMinutesException(string? message = "Invalid configuration. MinMinutesToExpire should be less than MaxMinutesToExpire.") : base(message)
        {
        }
    }
}