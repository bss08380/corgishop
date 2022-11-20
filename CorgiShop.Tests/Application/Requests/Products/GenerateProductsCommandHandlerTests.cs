using CorgiShop.Application.Requests.Products;
using CorgiShop.DataGen.Services;
using CorgiShop.Tests.Base;
using MediatR;
using Moq;

namespace CorgiShop.Tests.Application.Requests.Products;

public class GenerateProductsCommandHandlerTests : TestBase, IAsyncLifetime
{
    private readonly Mock<IProductDataGenService> _mockedProductDataGenService = new Mock<IProductDataGenService>();

    [Fact]
    public async Task Handle_GenServiceCalled()
    {
        //Arrange
        var cmd = GetGenerateCommand(10);
        var uut = await GetUut();
        //Act
        var unitValue = await uut.Handle(cmd, CancellationToken.None);
        //Assert
        Assert.Equal(Unit.Value, unitValue);
        _mockedProductDataGenService.VerifyAll();
    }

    private async Task<GenerateProductsCommandHandler> GetUut() => new GenerateProductsCommandHandler(DbContext!, _mockedProductDataGenService.Object);

    private GenerateProductsCommand GetGenerateCommand(int cnt) => new GenerateProductsCommand(cnt);

    public async Task InitializeAsync()
    {
        await RigCorgiShopDbContext((context) => { });
        _mockedProductDataGenService.Setup(s => s.GenerateProducts(DbContext!, 10));
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
