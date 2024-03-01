
using URLShortener.Domain;
using URLShortener.Infra.Interfaces;

namespace URLShortener.Infra.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        public Task AddAsync(Url url)
        {
            throw new NotImplementedException();
        }

        public Task<uint> GetByIdAsync(uint id)
        {
            throw new NotImplementedException();
        }

        public Task<Url> GetByUrlAsync(string shortenedUrl)
        {
            throw new NotImplementedException();
        }
    }
}
