using System.Runtime.Serialization;

namespace URLShortener.Application.Interfaces
{
    [Serializable]
    public class FailedToParseMinutesToUintException : Exception
    {
        public FailedToParseMinutesToUintException()
        {
        }

        public FailedToParseMinutesToUintException(string? message= "Invalid configuration for expiration minutes. Check appsettings.json file.") : base(message)
        {
        }

        public FailedToParseMinutesToUintException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected FailedToParseMinutesToUintException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}