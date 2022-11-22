using CorgiShop.Domain;
using CorgiShop.Domain.Abstractions;
using CorgiShop.Domain.Features.Products;
using CorgiShop.Domain.Model;
using MediatR;

namespace CorgiShop.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _productRepository.Delete(request.ProductId);
        return Unit.Value;
    }
}
