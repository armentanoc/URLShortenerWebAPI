
using Microsoft.EntityFrameworkCore;
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
                _context.Url.Add(entityToAdd);
                await _context.SaveChangesAsync();
                return entityToAdd;
            }

            throw new Exception($"Url with properties described already exists ({entityToAdd.ToString()}.");
        }
        public async Task<Url> GetByUrlAsync(string shortenedUrl)
        {
            var entityToReturn = await _context.Url.FirstOrDefaultAsync(url => url.ShortenedUrl.Equals(shortenedUrl));

            if (entityToReturn is Url url)
                return entityToReturn;

            throw new Exception($"Original url with shortened url {shortenedUrl} doesn't exist.");
        }
    }
}
