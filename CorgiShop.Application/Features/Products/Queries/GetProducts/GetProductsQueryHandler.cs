using AutoMapper;
using CorgiShop.Application.CQRS.Base;
using CorgiShop.Common.Exceptions;
using CorgiShop.Domain;
using CorgiShop.Domain.Abstractions;
using CorgiShop.Domain.Features.Products;
using CorgiShop.Domain.Model;
using MediatR;

namespace CorgiShop.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsDto>
{
    private const int MAX_PAGE_SIZE = 200;

    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GetProductsDto> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        if (request.Limit > MAX_PAGE_SIZE) throw DetailedException.FromFailedVerification(nameof(request.Limit), $"Maximum limit size is {MAX_PAGE_SIZE}");

        int total = await _productRepository.Count();
        var results = (await _productRepository.ListPaginated(request.Limit, request.Offset)).Select(p => _mapper.Map<Product, ProductDto>(p));

        return new GetProductsDto()
        {
            Page = QueryPageDto.FromCurrentPage(total, request.Limit, request.Offset),
            TotalAvailable = total,
            TotalReturned = results?.Count() ?? 0,
            Results = results
        };
    }
}
