using CorgiShop.Domain.Model;
using CorgiShop.Pipeline.Base.Repositories;

namespace CorgiShop.Domain.Features.Products;

public class ProductRepository : RepositoryBase<Product>, IProductRepository
{
    public ProductRepository(CorgiShopDbContext corgiShopDbContext)
        : base(corgiShopDbContext)
    {
        MaxPageSizeLimit = 200;
    }
}
