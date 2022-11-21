using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Domain.Model;

public class CorgiShopDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public CorgiShopDbContext(DbContextOptions<CorgiShopDbContext> options)
        : base(options)
    {
    }
}
