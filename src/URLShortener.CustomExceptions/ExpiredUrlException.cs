using System.Runtime.Serialization;

namespace URLShortener.Application.Interfaces
{
    [Serializable]
    public class ExpiredUrlException : Exception
    {
        public ExpiredUrlException()
        {
        }

        public ExpiredUrlException(string? message= "This URL has expired.") : base(message)
        {
        }

        public ExpiredUrlException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ExpiredUrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}