using CorgiShop.Application.Model.Features.Products;
using CorgiShop.Client.Services.Abstractions;

namespace CorgiShop.Client.Services.Features.Products;

public interface IProductService : ICrudService<ProductDto>
{
}
