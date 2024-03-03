
using System.Runtime.Serialization;

namespace URLShortener.Infra.Repositories
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public string SpecificEntity { get; private set; }
        public uint Id { get; private set; }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string? message) : base(message)
        {
        }

        public EntityNotFoundException(string specificEntity, uint id)
            : base($"Entity {specificEntity} with id {id} doesn't exist.")
        {
            SpecificEntity = specificEntity;
            Id = id;
        }

        public EntityNotFoundException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
