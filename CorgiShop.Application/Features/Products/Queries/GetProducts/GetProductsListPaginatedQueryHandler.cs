using AutoMapper;
using CorgiShop.Application.Base;
using CorgiShop.Application.CQRS.Base;
using CorgiShop.Domain.Features.Products;
using CorgiShop.Domain.Model;
using MediatR;

namespace CorgiShop.Application.Features.Products.Queries.GetProducts;

public class GetProductsListPaginatedQueryHandler : GetListPaginatedQueryHandler<ProductDto, Product>, IRequestHandler<GetProductsListPaginatedQuery, PaginatedResultsDto<ProductDto>>
{
    public GetProductsListPaginatedQueryHandler(
        IProductRepository productRepository,
        IMapper mapper)
        : base(productRepository, mapper)
    {
    }

    public async Task<PaginatedResultsDto<ProductDto>> Handle(GetProductsListPaginatedQuery request, CancellationToken cancellationToken) => await base.Handle(request, cancellationToken);
}
