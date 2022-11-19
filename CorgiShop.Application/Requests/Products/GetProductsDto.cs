using CorgiShop.Application.Requests.Base;

namespace CorgiShop.Application.Requests.Products
{
    public record GetProductsDto : PaginatedResultsDto<ProductDto>;
}
