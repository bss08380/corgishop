using CorgiShop.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace CorgiShop.Domain;

public class ProductsRepository : IProductsRepository
{
    private const int DB_FETCH_LIMIT = 200;

    private readonly CorgiShopDbContext _corgiShopDbContext;

    public ProductsRepository(CorgiShopDbContext corgiShopDbContext)
    {
        _corgiShopDbContext = corgiShopDbContext;
    }

    public async Task<int> GetTotalAvailable()
    {
        return await _corgiShopDbContext.Products.CountAsync();
    }

    public async Task<IEnumerable<Product>> GetPaginated(int limit, int offset)
    {
        return await _corgiShopDbContext.Products
            .Where(p => p.IsDeleted == false)
            .OrderBy(p => p.Name)
            .Skip(offset)
            .Take(Math.Min(limit, DB_FETCH_LIMIT))//simple db protection - lower limits are in place further up as well
            .ToListAsync();
    }

    public async Task Delete(int productId)
    {
        var product = await _corgiShopDbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        if (product == null) throw new InvalidOperationException("Invalid product ID - product not found");

        product.IsDeleted = true;
        await _corgiShopDbContext.SaveChangesAsync();
    }
}
