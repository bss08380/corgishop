using CorgiShop.Biz.Requests.Base;

namespace CorgiShop.Biz.Requests.Products
{
    public record GetProductsDto : PaginatedResultsDto<ProductDto>;
}
