using CorgiShop.Application.Abstractions;
using MediatR;
using System.Text.Json.Serialization;

namespace CorgiShop.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<GetProductsDto>, ICacheableQuery
{
    public required int Limit { get; init; }
    public required int Offset { get; init; }

    [JsonIgnore]
    public string CacheKey => $"CorgiShop:Products:Page-{Limit}-{Offset}";
    [JsonIgnore]
    public TimeSpan? TimeToLive => TimeSpan.FromMinutes(5);
}