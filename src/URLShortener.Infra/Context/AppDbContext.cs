using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using URLShortener.Domain;

namespace URLShortener.Infra.Context
{
    [ExcludeFromCodeCoverage]
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
