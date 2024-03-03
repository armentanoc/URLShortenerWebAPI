
using URLShortener.Domain;
using URLShortener.Infra.Context;
using URLShortener.Infra.Interfaces;

namespace URLShortener.Infra.Repositories
{
    public class UrlRepository : Repository<Url>, IUrlRepository
    {
        public UrlRepository(AppDbContext context) : base(context)
        {
            //required by EF
        }
        public new async Task<Url> AddAsync(Url entityToAdd)
        {
            if (!await EntityExistsAsync(entityToAdd))
            {
                _context.Set<Url>().Add(entityToAdd);
                await _context.SaveChangesAsync();
                return entityToAdd;
            }

            throw new Exception($"Url with properties described already exists ({entityToAdd.ToString()}.");
        }
        public Task<Url> GetByUrlAsync(string shortenedUrl)
        {
            throw new NotImplementedException();
        }
    }
}
