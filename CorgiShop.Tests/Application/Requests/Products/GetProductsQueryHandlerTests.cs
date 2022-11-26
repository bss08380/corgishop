using AutoMapper;
using CorgiShop.Application.Features.Products;
using CorgiShop.Application.Features.Products.Queries.GetProducts;
using CorgiShop.Common.Exceptions;
using CorgiShop.Domain;
using CorgiShop.Domain.Abstractions;
using CorgiShop.Domain.Features.Products;
using CorgiShop.Domain.Model;
using CorgiShop.Tests.Base;
using Moq;

namespace CorgiShop.Tests.Application.Requests.Products;

public class GetProductsQueryHandlerTests : TestBase
{
    private readonly Mock<IMapper> _mockedMapper;
    private readonly Mock<IProductRepository> _mockedRepo;

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

    private GetProductsListPaginatedQueryHandler GetUut() => new GetProductsListPaginatedQueryHandler(_mockedRepo.Object, _mockedMapper.Object);

    private Mock<IMapper> RigMockedMapper()
    {
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(m => m.Map<Product, ProductDto>(It.IsAny<Product>())).Returns(new ProductDto(0, "", "", 0.0M, 0));
        return mockedMapper;
    }

    private Mock<IProductRepository> RigMockedProductRepo()
    {
        var productList = new List<Product>()
            {
                new Product(),
                new Product(),
                new Product(),
                new Product(),
                new Product()
            };
        var mockedRepo = new Mock<IProductRepository>();
        mockedRepo.Setup(r => r.ListPaginated(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(productList.Take(3));
        mockedRepo.Setup(r => r.Count()).ReturnsAsync(productList.Count());
        return mockedRepo;
    }

    private GetProductsListPaginatedQuery GetRequestQuery(int limit, int offset) => new GetProductsListPaginatedQuery() { Limit = limit, Offset = offset };
}
