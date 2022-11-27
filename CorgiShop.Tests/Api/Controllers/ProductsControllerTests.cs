using CorgiShop.Api.Controllers;
using CorgiShop.Application.Features.Products.Commands.GenerateProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CorgiShop.Tests.Api.Controllers;

public class ProductsControllerTests
{
    private Mock<ISender> MockSender = new Mock<ISender>();

    [Fact]
    public async Task Generate_CommandSent()
    {
        //Arrange
        var uut = GetUut();
        var cmd = new GenerateProductsCommand(0);
        //Act
        var actionResult = await uut.Generate(cmd);
        var okResult = actionResult as OkResult;
        //Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        MockSender.Verify(s => s.Send(cmd, default));
    }

    private ProductsController GetUut() => new ProductsController(MockSender.Object);

}
