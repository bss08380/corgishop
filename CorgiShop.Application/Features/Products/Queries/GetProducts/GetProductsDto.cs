using CorgiShop.Application.CQRS.Base;

namespace CorgiShop.Application.Features.Products.Queries.GetProducts;

public record GetProductsDto : PaginatedResultsDto<ProductDto>;
