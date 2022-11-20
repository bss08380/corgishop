using AutoMapper;
using CorgiShop.Application.Requests.Base;
using CorgiShop.Common.Exceptions;
using CorgiShop.Domain;
using CorgiShop.Domain.Model;
using MediatR;

namespace CorgiShop.Application.Requests.Products;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsDto>
{
    private const int MAX_PAGE_SIZE = 200;

    private readonly IProductsRepository _corgiShopRepo;
    private readonly IMapper _mapper;

    public GetProductsQueryHandler(
        IProductsRepository corgiShopRepo,
        IMapper mapper)
    {
        _corgiShopRepo = corgiShopRepo;
        _mapper = mapper;
    }

    public async Task<GetProductsDto> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        if (request.Limit > MAX_PAGE_SIZE) throw DetailedException.FromFailedVerification(nameof(request.Limit), $"Maximum limit size is {MAX_PAGE_SIZE}");

        int total = await _corgiShopRepo.GetTotalAvailable();
        var results = (await _corgiShopRepo.GetPaginated(request.Limit, request.Offset)).Select(p => _mapper.Map<Product, ProductDto>(p));

        return new GetProductsDto()
        {
            Page = QueryPageDto.FromCurrentPage(total, request.Limit, request.Offset),
            TotalAvailable = total,
            TotalReturned = results?.Count() ?? 0,
            Results = results
        };
    }
}
