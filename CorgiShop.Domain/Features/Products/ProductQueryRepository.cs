using CorgiShop.Domain.Base;
using CorgiShop.Domain.Model;

namespace CorgiShop.Domain.Features.Products;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(CorgiShopDbContext corgiShopDbContext)
        : base(corgiShopDbContext)
    {
        MaxPageSizeLimit = 200;
    }
}
