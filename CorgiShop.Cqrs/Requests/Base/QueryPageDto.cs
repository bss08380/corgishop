namespace CorgiShop.Biz.Requests.Base;

public record QueryPageDto
{
    public required bool CanGoForward { get; set; }
    public required bool CanGoBackward { get; set; }

    public required int NextOffset { get; set; }
    public required int PreviousOffset { get; set; }

    public required int CurrentLimit { get; set; }
    public required int CurrentOffset { get; set; }

    public static QueryPageDto FromCurrentPage(int totalAvailable, int currentLimit, int currentOffset) => 
        new QueryPageDto()
        {
            CanGoForward = totalAvailable > (currentOffset + currentLimit),
            CanGoBackward = currentOffset > 0,

            NextOffset = 
                Math.Max(
                    0, 
                    Math.Min(
                        totalAvailable - currentLimit, 
                        currentOffset + currentLimit)),

            PreviousOffset = Math.Max(0, currentOffset - currentLimit),

            CurrentLimit = currentLimit,
            CurrentOffset = currentOffset
        };
}
