using Microsoft.EntityFrameworkCore;
using URLShortener.Domain;

namespace URLShortener.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Url> Url { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>()
                .Ignore("Assert"); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
