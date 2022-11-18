using AutoMapper;
using CorgiShop.Biz.Requests.Base;
using CorgiShop.Repo;
using CorgiShop.Repo.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Biz.Requests.Products;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, GetProductsDto>
{
    private const int MAX_PAGE_SIZE = 100;

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
        //Quasi-auto-correct of page size if something sends huge limit to API
        //Db protections are also in place further down
        int clampedPageSize = Math.Min(request.Limit, MAX_PAGE_SIZE);

        int total = await _corgiShopRepo.GetTotalAvailable();
        var results = (await _corgiShopRepo.GetPaginated(clampedPageSize, request.Offset)).Select(p => _mapper.Map<Product, ProductDto>(p));

        return new GetProductsDto()
        {
            Page = QueryPageDto.FromCurrentPage(total, clampedPageSize, request.Offset),
            TotalAvailable = total,
            TotalReturned = results?.Count() ?? 0,
            Results = results
        };
    }
}
