using CorgiShop.Application.Abstractions;
using CorgiShop.Domain.Model;

namespace CorgiShop.Application.Features.Products;

public record ProductDto(int Id, string Name, string Description, decimal Price, int Stock) : IDtoEntity;
