﻿using AutoMapper;
using CorgiShop.Application.Requests.Products;
using CorgiShop.Domain;
using CorgiShop.Domain.Model;
using CorgiShop.Tests.Base;
using Moq;

namespace CorgiShop.Tests.Application.Requests.Products;

public class GetProductsQueryHandlerTests : TestBase
{
    private readonly Mock<IMapper> _mockedMapper;
    private readonly Mock<IProductsRepository> _mockedRepo;

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
    public async Task Handle_PageSizeLimit()
    {
        //Arrange
        var uut = GetUut();
        var query = GetRequestQuery(500, 1);
        //Act
        var response = await uut.Handle(query, CancellationToken.None);
        //Assert
        Assert.Equal(100, response.Page.CurrentLimit);
    }

    private GetProductsQueryHandler GetUut() => new GetProductsQueryHandler(_mockedRepo.Object, _mockedMapper.Object);

    private Mock<IMapper> RigMockedMapper()
    {
        var mockedMapper = new Mock<IMapper>();
        mockedMapper.Setup(m => m.Map<Product, ProductDto>(It.IsAny<Product>())).Returns(new ProductDto(0, "", "", 0.0M, 0)).Verifiable();
        return mockedMapper;
    }

    private Mock<IProductsRepository> RigMockedProductRepo()
    {
        var productList = new List<Product>()
            {
                new Product(),
                new Product(),
                new Product(),
                new Product(),
                new Product()
            };
        var mockedRepo = new Mock<IProductsRepository>();
        mockedRepo.Setup(r => r.GetPaginated(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(productList.Take(3)).Verifiable();
        mockedRepo.Setup(r => r.GetTotalAvailable()).ReturnsAsync(productList.Count()).Verifiable();
        return mockedRepo;
    }

    private GetProductsQuery GetRequestQuery(int limit, int offset)
    {
        return new GetProductsQuery()
        {
            Limit = limit,
            Offset = offset
        };
    }
}