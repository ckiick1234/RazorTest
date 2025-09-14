using Microsoft.EntityFrameworkCore;

namespace RazorTest.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }
        public DbSet<RazorTest.Models.Product>? Products { get; set; }
    }

}

