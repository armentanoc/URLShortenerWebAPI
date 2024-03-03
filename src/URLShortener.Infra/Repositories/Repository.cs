
using Microsoft.EntityFrameworkCore;
using URLShortener.Domain;
using URLShortener.Infra.Context;
using URLShortener.Infra.Interfaces;

namespace URLShortener.Infra.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        internal readonly AppDbContext _context;
        private string _specificEntity = typeof(T).Name;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entityToAdd)
        {
            if (!await EntityExistsAsync(entityToAdd))
            {
                _context.Set<T>().Add(entityToAdd);
                await _context.SaveChangesAsync();
                return entityToAdd;
            }

            throw new Exception($"Entity {_specificEntity} with properties described already exists.");
        }

        public async Task<bool> DeleteAsync(uint id)
        {
            var entityToDelete = await GetAsync(id);
            _context.Set<T>().Remove(entityToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<T> GetAsync(uint id)
        {
            var entityToReturn = await _context.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id);

            if (entityToReturn != null)
            {
                entityToReturn.SetId(id);
                await _context.SaveChangesAsync();
                return entityToReturn;
            }

            throw new Exception($"Entity {_specificEntity} with id {id} doesn't exist.");
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> UpdateAsync(T newEntity)
        {
            var existingEntity = await GetAsync(newEntity.Id);

            foreach (var prop in typeof(T).GetProperties().Where(prop => prop.Name != "Id"))
            {
                var newValue = prop.GetValue(newEntity);
                prop.SetValue(existingEntity, newValue);
            }

            await _context.SaveChangesAsync();
            return existingEntity;
        }

        public async Task<bool> EntityExistsAsync(T entityToAdd)
        {
            return await _context.Set<T>()
                .AnyAsync(existingEntity => existingEntity.Id == entityToAdd.Id);
        }
    }
}
