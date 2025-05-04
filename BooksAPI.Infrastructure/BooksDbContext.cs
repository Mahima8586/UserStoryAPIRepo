using BooksAPI.Infrastructure.BooksDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace BooksAPI.Infrastructure
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options)
            : base(options) { }

        public DbSet<Books> Books { get; set; }
    }
    
}

