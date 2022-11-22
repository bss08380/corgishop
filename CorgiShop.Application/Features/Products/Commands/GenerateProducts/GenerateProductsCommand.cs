using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CorgiShop.Application.Features.Products.Commands.GenerateProducts;

public record GenerateProductsCommand([Required] int NumberToGenerate) : IRequest;
