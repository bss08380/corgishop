using Microsoft.EntityFrameworkCore;

namespace CorgiShop.Domain.Model;

public class CorgiShopDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public CorgiShopDbContext(DbContextOptions<CorgiShopDbContext> options)
        : base(options)
    {
    }
}
