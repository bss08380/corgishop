using CorgiShop.Application.Abstractions;
using CorgiShop.Application.Base;
using System.Text.Json.Serialization;

namespace CorgiShop.Application.Features.Products.Queries.GetProducts;

public record GetProductsListPaginatedQuery(int Limit, int Offset) : GetListPaginatedQuery<ProductDto>(Limit, Offset), ICacheableQuery
{
    [JsonIgnore]
    public string CacheKey => $"CorgiShop:Products:Page-{Limit}-{Offset}";
    [JsonIgnore]
    public TimeSpan? TimeToLive => TimeSpan.FromMinutes(5);
}