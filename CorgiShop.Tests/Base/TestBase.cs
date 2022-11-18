using CorgiShop.Repo.Model;
using Microsoft.EntityFrameworkCore;

namespace CorgiShop.Tests.Base
{
    public abstract class TestBase
    {
        protected CorgiShopDbContext? DbContext { get; private set; }

        public TestBase()
        {
        }

        protected async Task<CorgiShopDbContext> RigCorgiShopDbContext(Action<CorgiShopDbContext> loadAction)
        {
            var dbOptions = new DbContextOptionsBuilder<CorgiShopDbContext>().UseInMemoryDatabase(databaseName: $"CorgiShopFauxDb_{Guid.NewGuid()}").Options;
            DbContext = new CorgiShopDbContext(dbOptions);
            loadAction(DbContext);
            await DbContext.SaveChangesAsync();
            return DbContext;
        }

    }
}
