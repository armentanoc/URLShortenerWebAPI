using System.Runtime.Serialization;

namespace URLShortener.Application.Interfaces
{
    [Serializable]
    public class MinMinutesIsGreaterOrEqualThanMaxMinutesException : Exception
    {
        public MinMinutesIsGreaterOrEqualThanMaxMinutesException()
        {
        }

        public MinMinutesIsGreaterOrEqualThanMaxMinutesException(string? message = "Invalid configuration. MinMinutesToExpire should be less than MaxMinutesToExpire.") : base(message)
        {
        }

        public MinMinutesIsGreaterOrEqualThanMaxMinutesException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MinMinutesIsGreaterOrEqualThanMaxMinutesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}