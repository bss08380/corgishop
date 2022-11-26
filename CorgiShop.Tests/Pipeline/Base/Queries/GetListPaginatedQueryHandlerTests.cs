using AutoMapper;
using CorgiShop.Common.Exceptions;
using CorgiShop.Domain.Model;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Handlers;
using CorgiShop.Pipeline.Base.Requests;
using CorgiShop.Tests.Base;
using Moq;

namespace CorgiShop.Tests.Pipeline.Base.Queries;

public class GetProductsQueryHandlerTests : TestBase
{
    private readonly Mock<IMapper> _mockedMapper;
    private readonly Mock<IRepository<Testo>> _mockedRepo;

    public GetProductsQueryHandlerTests()
    {
        _mockedMapper = RigMockedMapper();
        _mockedRepo = RigMockedProductRepo();
    }

    [Fact]
    public async Task Handle()
    {
        //Arrange
        var uut = GetUut();
        var query = GetRequestQuery(3, 1);
        //Act
        var response = await uut.Handle(query, CancellationToken.None);
        //Assert
        _mockedMapper.VerifyAll();
        _mockedRepo.VerifyAll();

        Assert.Equal(3, response.Page.CurrentLimit);
        Assert.Equal(1, response.Page.CurrentOffset);
        Assert.Equal(3, response.TotalReturned);
        Assert.Equal(5, response.TotalAvailable);
        Assert.Equal(3, response.Results?.Count() ?? 0);
    }

    [Fact]
    public async Task Handle_PageSizeLimitException()
    {
        //Arrange
        var uut = GetUut();
        var query = GetRequestQuery(500, 1);
        //Act, Assert
        await Assert.ThrowsAsync<DetailedException>(async () => await uut.Handle(query, CancellationToken.None));
    }

    private GetListPaginatedQueryHandler<TestoDto, Testo> GetUut() => new GetListPaginatedQueryHandler<TestoDto, Testo>(_mockedRepo.Object, _mockedMapper.Object);

    private Mock<IMapper> RigMockedMapper()
    {
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(m => m.Map<Testo, TestoDto>(It.IsAny<Testo>())).Returns(new TestoDto(0));
        return mockedMapper;
    }

    private Mock<IRepository<Testo>> RigMockedProductRepo()
    {
        var productList = new List<Testo>()
            {
                new Testo(),
                new Testo(),
                new Testo(),
                new Testo(),
                new Testo()
            };
        var mockedRepo = new Mock<IRepository<Testo>>();
        mockedRepo.Setup(r => r.ListPaginated(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(productList.Take(3));
        mockedRepo.Setup(r => r.Count()).ReturnsAsync(productList.Count());
        return mockedRepo;
    }

    private GetListPaginatedQuery<TestoDto> GetRequestQuery(int limit, int offset) => new GetListPaginatedQuery<TestoDto>(limit, offset);
}
