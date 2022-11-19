using CorgiShop.Repo.Model;

namespace CorgiShop.DataGen.Services;

public interface IProductDataGenService
{
    Task GenerateProducts(CorgiShopDbContext dbContext, int productCount);
}
