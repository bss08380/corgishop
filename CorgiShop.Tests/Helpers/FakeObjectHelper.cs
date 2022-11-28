using CorgiShop.Pipeline.Base;
using CorgiShop.Tests.Base;

namespace CorgiShop.Tests.Helpers;

public static class FakeObjectHelper
{

    public static PaginatedResultsDto<TestoDto> GetPaginatedResultsDto()
    {
        return new PaginatedResultsDto<TestoDto>()
        {
            Page = new QueryPageDto()
            {
                CanGoBackward = true,
                CanGoForward = true,
                CurrentLimit = 0,
                CurrentOffset = 0,
                NextOffset = 0,
                PreviousOffset = 0
            },
            Results = null,
            TotalAvailable = 0,
            TotalReturned = 0
        };
    }

    public static TestoDto GetTestDto(int id = 0, int testingId = 0) => new(id, testingId);

}
