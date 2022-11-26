using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base;
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
        _mockedRepository.Setup(r => r.Delete(0));
    }

    [Fact]
    public async Task Handle_RepoCalled()
    {
        //Arrange
        var cmd = GetDeleteCommand(0);
        var uut = GetUut();
        //Act
        var unitValue = await uut.Handle(cmd, CancellationToken.None);
        //Assert
        Assert.Equal(Unit.Value, unitValue);
        _mockedRepository.VerifyAll();
    }

    private DeleteCommandHandler<TestoDto, Testo> GetUut() => new DeleteCommandHandler<TestoDto, Testo>(_mockedRepository.Object);

    private DeleteCommand<TestoDto> GetDeleteCommand(int productId) => new DeleteCommand<TestoDto>(productId);
}
