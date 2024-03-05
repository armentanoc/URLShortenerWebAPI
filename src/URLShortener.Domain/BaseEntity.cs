
using System.Diagnostics.CodeAnalysis;

namespace URLShortener.Domain
{
    [ExcludeFromCodeCoverage]
    public class BaseEntity
    {
        public uint Id { get; private set; }

        public void SetId(uint id)
        {
            Id = id;
        }
    }
}
