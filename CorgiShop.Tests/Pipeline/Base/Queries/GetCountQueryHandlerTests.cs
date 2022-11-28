using AutoMapper;
using CorgiShop.Common.Exceptions;
using CorgiShop.Domain.Model;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base;
using CorgiShop.Pipeline.Base.Handlers;
using CorgiShop.Pipeline.Base.Requests;
using CorgiShop.Tests.Base;
using Moq;

namespace CorgiShop.Tests.Pipeline.Base.Queries;

public class GetCountQueryHandlerTests : TestBase
{
    private readonly Mock<IRepository<Testo>> _mockedRepo;

    public GetCountQueryHandlerTests()
    {
        _mockedRepo = RigMockedProductRepo();
    }

    [Fact]
    public async Task Handle_RepoCalled()
    {
        //Arrange
        var uut = GetUut();
        var query = new GetCountQuery<TestoDto>();
        //Act
        var response = await uut.Handle(query, CancellationToken.None);
        //Assert
        _mockedRepo.VerifyAll();
        Assert.Equal(99, response);
    }

    private GetCountQueryHandler<TestoDto, Testo> GetUut() => new GetCountQueryHandler<TestoDto, Testo>(_mockedRepo.Object);

    private Mock<IRepository<Testo>> RigMockedProductRepo()
    {
        var mockedRepo = new Mock<IRepository<Testo>>();
        mockedRepo.Setup(r => r.Count()).ReturnsAsync(99);
        return mockedRepo;
    }
}
