namespace CorgiShop.Application.Features.Products.Queries.GetProducts;

public record ProductDto(int ProductId, string Name, string Description, decimal Price, int Stock);
