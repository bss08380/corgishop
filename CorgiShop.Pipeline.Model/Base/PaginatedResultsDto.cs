namespace CorgiShop.Pipeline.Model.Base;

public record PaginatedResultsDto<T>
{
    public required QueryPageDto Page { get; set; }
    public required int TotalAvailable { get; set; }
    public required int TotalReturned { get; set; }
    public required IEnumerable<T>? Results { get; set; }
}
