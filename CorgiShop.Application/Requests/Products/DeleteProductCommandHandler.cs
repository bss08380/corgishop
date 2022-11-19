using CorgiShop.Domain;
using MediatR;

namespace CorgiShop.Application.Requests.Products
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductsRepository _productsRepository;

        public DeleteProductCommandHandler(
            IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _productsRepository.Delete(request.ProductId);
            return Unit.Value;
        }
    }
}
