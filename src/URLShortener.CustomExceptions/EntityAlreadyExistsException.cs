using System.Runtime.Serialization;

namespace URLShortener.Infra.Repositories
{
    [Serializable]
    public class EntityAlreadyExistsException : Exception
    {
        public string SpecificEntity { get; private set; }
        public EntityAlreadyExistsException()
        {
        }

        public EntityAlreadyExistsException(string specificEntity)
           : base($"Entity {specificEntity} with properties described already exists.")
        {
            SpecificEntity = specificEntity;
        }

        public EntityAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}