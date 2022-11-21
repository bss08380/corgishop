using MediatR;

namespace CorgiShop.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery(int Limit, int Offset) : IRequest<GetProductsDto>;
