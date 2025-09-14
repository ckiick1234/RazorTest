using Microsoft.EntityFrameworkCore;
using RazorTest.Models;

namespace RazorTest.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }

        public DbSet<RazorTest.Models.Product>? Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductContext>();
        }
    }
}

