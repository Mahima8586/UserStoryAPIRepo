using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
