using AutoMapper;
using CorgiShop.Common.Exceptions;
using CorgiShop.Domain.Model;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Handlers;
using CorgiShop.Pipeline.Base.Requests;
using CorgiShop.Tests.Base;
using Moq;

namespace CorgiShop.Tests.Pipeline.Base.Queries;

public class GetByIdQueryHandlerTests : TestBase
{
    private readonly Mock<IMapper> _mockedMapper;
    private readonly Mock<IRepository<Testo>> _mockedRepo;

    private readonly Testo _testoInstance = new Testo()
    {
        Id = 1,
        TestingId = 99
    };

    public GetByIdQueryHandlerTests()
    {
        _mockedMapper = RigMockedMapper();
        _mockedRepo = RigMockedProductRepo();
    }

    [Fact]
    public async Task Handle()
    {
        //Arrange
        var uut = GetUut();
        var query = new GetByIdQuery<TestoDto>(_testoInstance.Id);
        //Act
        var response = await uut.Handle(query, CancellationToken.None);
        //Assert
        _mockedMapper.VerifyAll();
        _mockedRepo.VerifyAll();
        Assert.Equal(_testoInstance.Id, response.Id);
        Assert.Equal(_testoInstance.TestingId, response.TestingId);
    }

    private GetByIdQueryHandler<TestoDto, Testo> GetUut() => new GetByIdQueryHandler<TestoDto, Testo>(_mockedRepo.Object, _mockedMapper.Object);

    private Mock<IMapper> RigMockedMapper()
    {
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(m => m.Map<Testo, TestoDto>(_testoInstance)).Returns(new TestoDto(1, 99));
        return mockedMapper;
    }

    private Mock<IRepository<Testo>> RigMockedProductRepo()
    {
        var mockedRepo = new Mock<IRepository<Testo>>();
        mockedRepo.Setup(r => r.GetById(1)).ReturnsAsync(_testoInstance);
        return mockedRepo;
    }

    private GetListPaginatedQuery<TestoDto> GetRequestQuery(int limit, int offset) => new GetListPaginatedQuery<TestoDto>(limit, offset);
}
