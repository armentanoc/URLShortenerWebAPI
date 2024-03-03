
namespace URLShortener.Domain
{
    public class BaseEntity
    {
        public uint Id { get; private set; }

        public void SetId(uint id)
        {
            Id = id;
        }
    }
}
