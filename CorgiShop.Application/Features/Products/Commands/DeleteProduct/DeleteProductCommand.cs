using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CorgiShop.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand([Required] int ProductId) : IRequest;
