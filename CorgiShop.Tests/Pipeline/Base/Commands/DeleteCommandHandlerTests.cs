using CorgiShop.Domain.Model;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Handlers;
using CorgiShop.Pipeline.Base.Requests;
using CorgiShop.Tests.Base;
using MediatR;
using Moq;

namespace CorgiShop.Tests.Pipeline.Base.Commands;

public class DeleteCommandHandlerTests
{
    private Mock<IRepository<Testo>> _mockedRepository;

    public DeleteCommandHandlerTests()
    {
        _mockedRepository = new Mock<IRepository<Testo>>();
    }

    [Fact]
    public async Task Handle_HardDelete_RepoCalled()
    {
        //Arrange
        _mockedRepository.Setup(r => r.Delete(0));
        var cmd = new DeleteCommand<TestoDto>(0) { Mode = DeleteMode.Hard };
        var uut = GetUut();
        //Act
        var unitValue = await uut.Handle(cmd, CancellationToken.None);
        //Assert
        Assert.Equal(Unit.Value, unitValue);
        _mockedRepository.VerifyAll();
    }

    [Fact]
    public async Task Handle_SoftDelete_RepoCalled()
    {
        //Arrange
        _mockedRepository.Setup(r => r.SoftDelete(0));
        var cmd = new DeleteCommand<TestoDto>(0) { Mode = DeleteMode.Soft };
        var uut = GetUut();
        //Act
        var unitValue = await uut.Handle(cmd, CancellationToken.None);
        //Assert
        Assert.Equal(Unit.Value, unitValue);
        _mockedRepository.VerifyAll();
    }

    private DeleteCommandHandler<TestoDto, Testo> GetUut() => new DeleteCommandHandler<TestoDto, Testo>(_mockedRepository.Object);
}
