namespace CorgiShop.Application.Features.Products.Queries.GetProducts;

public record ProductDto(int Id, string Name, string Description, decimal Price, int Stock);
