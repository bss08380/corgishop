using CorgiShop.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace CorgiShop.Tests.Base;

public abstract class TestBase
{
    protected UnitTestDbContext? UnitTestDbContext { get; private set; }

    protected CorgiShopDbContext? CorgiShopDbContext { get; private set; }

    public TestBase()
    {
    }

    protected async Task<UnitTestDbContext> RigUnitTestDbContext(Action<UnitTestDbContext> loadAction)
    {
        var dbOptions = new DbContextOptionsBuilder<UnitTestDbContext>().UseInMemoryDatabase(databaseName: $"UnitTestDbContext_{Guid.NewGuid()}").Options;
        UnitTestDbContext = new UnitTestDbContext(dbOptions);
        loadAction(UnitTestDbContext);
        await UnitTestDbContext.SaveChangesAsync();
        return UnitTestDbContext;
    }

    protected async Task<CorgiShopDbContext> RigCorgiShopDbContext(Action<CorgiShopDbContext> loadAction)
    {
        var dbOptions = new DbContextOptionsBuilder<CorgiShopDbContext>().UseInMemoryDatabase(databaseName: $"UnitTestDbContext_{Guid.NewGuid()}").Options;
        CorgiShopDbContext = new CorgiShopDbContext(dbOptions);
        loadAction(CorgiShopDbContext);
        await CorgiShopDbContext.SaveChangesAsync();
        return CorgiShopDbContext;
    }

}
