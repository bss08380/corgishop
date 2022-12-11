using CorgiShop.Pipeline.Model.Abstractions;

namespace CorgiShop.Application.Model.Features.Products;

public record ProductDto(int Id, string Name, string Description, decimal Price, int Stock) : IDtoEntity;
