﻿using CorgiShop.DataGen.Services;
using CorgiShop.Domain.Model;
using MediatR;

namespace CorgiShop.Application.Features.Products.Commands.GenerateProducts;

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
