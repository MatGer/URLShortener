using UrlShortener.Models;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Models
{
    public class LinksDbContext : DbContext
    {
        public LinksDbContext(DbContextOptions<LinksDbContext> options): base(options) { }
        public DbSet<Links> Links { get; set; }
    }
}
