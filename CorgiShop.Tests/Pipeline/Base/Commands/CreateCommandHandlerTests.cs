using AutoMapper;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Handlers;
using CorgiShop.Pipeline.Base.Requests;
using CorgiShop.Tests.Base;
using Moq;

namespace CorgiShop.Tests.Pipeline.Base.Commands;

public class CreateCommandHandlerTests
{
    private Mock<IRepository<Testo>> _mockedRepository;
    private Mock<IMapper> _mockedMapper;

    private readonly TestoDto _testoDto = new TestoDto(0, 99);
    private readonly Testo _testoEntity = new Testo() { TestingId = 99 };

    public CreateCommandHandlerTests()
    {
        _mockedRepository = new Mock<IRepository<Testo>>();
        _mockedMapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task Handle_RepoCalled()
    {
        //Arrange
        _mockedRepository.Setup(r => r.Create(_testoEntity)).ReturnsAsync(_testoEntity);
        _mockedMapper.Setup(m => m.Map<TestoDto>(_testoEntity)).Returns(_testoDto);
        _mockedMapper.Setup(m => m.Map<Testo>(_testoDto)).Returns(_testoEntity);
        var cmd = new CreateCommand<TestoDto>(_testoDto);
        var uut = GetUut();
        //Act
        var unitValue = await uut.Handle(cmd, CancellationToken.None);
        //Assert
        _mockedMapper.VerifyAll();
        _mockedRepository.VerifyAll();
    }

    private CreateCommandHandler<TestoDto, Testo> GetUut() => new CreateCommandHandler<TestoDto, Testo>(_mockedRepository.Object, _mockedMapper.Object);
}
