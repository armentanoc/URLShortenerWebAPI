
using URLShortener.Domain;

namespace URLShortener.Infra.Interfaces
{
    public interface IUrlRepository
    {
        Task<uint> GetByIdAsync(uint id); //general repository
        Task<Url> GetByUrlAsync(string shortenedUrl);
        Task AddAsync(Url url);
    }
}
