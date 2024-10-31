using Microsoft.EntityFrameworkCore;
using sem3Tuesday3.Models;

namespace sem3Tuesday3.DbContexts
{
    public class ProductContext:DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
