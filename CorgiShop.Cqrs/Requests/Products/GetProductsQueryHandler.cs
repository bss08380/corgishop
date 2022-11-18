using AutoMapper;
using CorgiShop.Repo;
using CorgiShop.Repo.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Biz.Requests.Products
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductsRepository _corgiShopRepo;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(
            IProductsRepository corgiShopRepo,
            IMapper mapper)
        {
            _corgiShopRepo = corgiShopRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            return (await _corgiShopRepo.GetAll(request.Filter)).Select(p => _mapper.Map<Product, ProductDto>(p));
        }
    }
}
