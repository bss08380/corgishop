using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CorgiShop.Application.Requests.Products;

public record GetProductsQuery(int Limit, int Offset) : IRequest<GetProductsDto>;
