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

    /*
    [Fact]
    public async Task Get_CommandSent()
    {
        //Arrange
        var uut = GetUut();
        var query = new GetProductsQuery() { Limit = 0, Offset = 0 };
        //Act
        var actionResult = await uut.Get(query);
        var okResult = actionResult as OkObjectResult;
        //Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        MockSender.Verify(s => s.Send(query, default));
    }

    [Fact]
    public async Task Delete_CommandSent()
    {
        //Arrange
        var uut = GetUut();
        var cmd = new DeleteProductCommand(0);
        //Act
        var actionResult = await uut.Delete(cmd);
        var okResult = actionResult as OkResult;
        //Assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        MockSender.Verify(s => s.Send(cmd, default));
    }
    */

    private ProductsController GetUut()
    {
        /*
        var getProductsReturn = new GetProductsListPaginatedDto()
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

        MockSender.Setup(s => s.Send(It.IsAny<GetProductsListPaginatedQuery>(), default)).ReturnsAsync(getProductsReturn);
        */
        return new ProductsController(MockSender.Object);
    }

}
