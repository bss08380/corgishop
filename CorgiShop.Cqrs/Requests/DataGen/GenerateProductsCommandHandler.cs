using CorgiShop.DataGen.Services;
using CorgiShop.Repo.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Biz.Requests.DataGen
{
    public class GenerateProductsCommandHandler : IRequestHandler<GenerateProductsCommand>
    {
        private readonly CorgiShopDbContext _corgiShopDbContext;
        private readonly IProductDataGenService _productDataGenService;

        public GenerateProductsCommandHandler(
            CorgiShopDbContext corgiShopDbContext,
            IProductDataGenService productDataGenService)
        {
            _corgiShopDbContext = corgiShopDbContext;
            _productDataGenService = productDataGenService;
        }

        public async Task<Unit> Handle(GenerateProductsCommand request, CancellationToken cancellationToken)
        {
            await _productDataGenService.GenerateProducts(_corgiShopDbContext, request.NumberToGenerate);
            return Unit.Value;
        }
    }
}
