
using URLShortener.Domain;

namespace URLShortener.Infra.Interfaces
{
    public interface IUrlRepository : IRepository<Url>
    {
        Task<Url> GetByUrlAsync(string shortenedUrl);
        new Task<Url> AddAsync(Url newEntity);
    }
}
