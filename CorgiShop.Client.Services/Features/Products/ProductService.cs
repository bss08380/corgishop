using CorgiShop.Application.Model.Features.Products;
using CorgiShop.Client.Services.Abstractions;
using CorgiShop.Client.Services.Base;

namespace CorgiShop.Client.Services.Features.Products;

public class ProductService : CrudService<ProductDto>, IProductService
{
    private readonly IAuthedApiClient _apiClient;

    public ProductService(IAuthedApiClient apiClient)
        : base(apiClient, CrudServiceAuthConfiguration.ReadOnlyWithoutAuth())
	{
        _apiClient = apiClient;
    }

}
