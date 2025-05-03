using System.Data.Entity;
using BooksAPI.Infrastructure.BooksDB.Entities;

namespace BooksAPI.Infrastructure
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options) { }

        public DbSet<Books> Books { get; set; }
    }
    
}
