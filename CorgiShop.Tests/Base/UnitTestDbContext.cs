using CorgiShop.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace CorgiShop.Tests.Base;

public class UnitTestDbContext : DbContext
{
    public DbSet<Testo> Testos { get; set; }

    public UnitTestDbContext(DbContextOptions<UnitTestDbContext> options)
        : base(options)
    {
    }
}
