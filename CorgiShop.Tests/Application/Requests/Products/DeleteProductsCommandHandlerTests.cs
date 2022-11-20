using CorgiShop.Application.Requests.Products;
using CorgiShop.Domain.Model;
using CorgiShop.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CorgiShop.Tests.Application.Requests.Products;

public class DeleteProductsCommandHandlerTests
{
    private Mock<IProductsRepository> _mockedRepository;

    public DeleteProductsCommandHandlerTests()
    {
        _mockedRepository = new Mock<IProductsRepository>();
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

    private DeleteProductCommandHandler GetUut() => new DeleteProductCommandHandler(_mockedRepository.Object);

    private DeleteProductCommand GetDeleteCommand(int productId) => new DeleteProductCommand(productId);
}
