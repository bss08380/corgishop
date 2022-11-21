namespace CorgiShop.Application.CQRS.Base;

public abstract record PaginatedResultsDto<T>
{
    public required QueryPageDto Page { get; set; }
    public required int TotalAvailable { get; set; }
    public required int TotalReturned { get; set; }
    public required IEnumerable<T>? Results { get; set; }
}
