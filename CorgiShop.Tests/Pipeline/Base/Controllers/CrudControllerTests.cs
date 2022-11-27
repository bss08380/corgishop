using CorgiShop.Pipeline.Base;
using CorgiShop.Pipeline.Base.Controllers;
using CorgiShop.Pipeline.Base.Requests;
using CorgiShop.Tests.Base;
using CorgiShop.Tests.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CorgiShop.Tests.Pipeline.Base.Controllers;

public class CrudControllerTests
{
    private Mock<ISender> MockSender = new Mock<ISender>();

    [Fact]
    public async Task Get_PaginatedList_QuerySent()
    {
        //Arrange
        var query = new GetListPaginatedQuery<TestoDto>(0, 0);
        var fakeResults = FakeObjectHelper.GetPaginatedResultsDto();

        MockSender.Setup(s => s.Send(query, default)).ReturnsAsync(fakeResults);

        var config = GetConfiguration();
        config.ListEnabled = true;
        var uut = GetUut(config);
        //Act
        var result = await uut.Get(query) as OkObjectResult;
        //Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(result!.Value, fakeResults);
        MockSender.VerifyAll();
    }

    [Fact]
    public async Task GetById_QuerySent()
    {
        //Arrange
        var dto = FakeObjectHelper.GetTestDto();
        var query = new GetByIdQuery<TestoDto>(0);

        MockSender.Setup(s => s.Send(query, default)).ReturnsAsync(dto);

        var config = GetConfiguration();
        config.GetByIdEnabled = true;
        var uut = GetUut(config);
        //Act
        var result = await uut.GetById(query) as OkObjectResult;
        //Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(result!.Value, dto);
        MockSender.VerifyAll();
    }

    [Fact]
    public async Task GetCount_QuerySent()
    {
        //Arrange
        int cnt = 99;
        MockSender.Setup(s => s.Send(GetCountQuery<TestoDto>.Instance, default)).ReturnsAsync(cnt);

        var config = GetConfiguration();
        config.GetCountEnabled = true;
        var uut = GetUut(config);
        //Act
        var result = await uut.Count() as OkObjectResult;
        //Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(result!.Value, cnt);
        MockSender.VerifyAll();
    }

    [Fact]
    public async Task Delete_QuerySent()
    {
        //Arrange
        var cmd = new DeleteCommand<TestoDto>(0);
        MockSender.Setup(s => s.Send(cmd, default));

        var config = GetConfiguration();
        config.DeleteEnabled = true;
        var uut = GetUut(config);
        //Act
        var result = await uut.Delete(cmd);
        //Assert
        Assert.NotNull(result as OkResult);
        Assert.Equal(200, (result as OkResult).StatusCode);
        MockSender.VerifyAll();
    }

    [Fact]
    public async Task Create_QuerySent()
    {
        //Arrange
        var dto = FakeObjectHelper.GetTestDto();
        MockSender.Setup(s => s.Send(It.IsAny<CreateCommand<TestoDto>>(), default)).ReturnsAsync(dto);

        var config = GetConfiguration();
        config.CreateEnabled = true;
        var uut = GetUut(config);
        //Act
        var result = await uut.Create(dto) as OkObjectResult;
        //Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(result!.Value, dto);
        MockSender.VerifyAll();
    }

    [Fact]
    public async Task Update_QuerySent()
    {
        //Arrange
        var dto = FakeObjectHelper.GetTestDto();
        MockSender.Setup(s => s.Send(It.IsAny<UpdateCommand<TestoDto>>(), default)).ReturnsAsync(dto);

        var config = GetConfiguration();
        config.CreateEnabled = true;
        var uut = GetUut(config);
        //Act
        var result = await uut.Update(dto) as OkObjectResult;
        //Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        Assert.Equal(result!.Value, dto);
        MockSender.VerifyAll();
    }

    [Fact]
    public async Task GetPaginatedList_EndpointDisabled()
    {
        //Arrange
        var query = new GetListPaginatedQuery<TestoDto>(0, 0);
        var config = GetConfiguration();
        config.ListEnabled = false;
        var uut = GetUut(config);
        //Act/Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await uut.Get(query));
    }

    [Fact]
    public async Task GetById_EndpointDisabled()
    {
        //Arrange
        var query = new GetByIdQuery<TestoDto>(0);
        var config = GetConfiguration();
        config.GetByIdEnabled = false;
        var uut = GetUut(config);
        //Act/Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await uut.GetById(query));
    }

    [Fact]
    public async Task GetCount_EndpointDisabled()
    {
        //Arrange
        var config = GetConfiguration();
        config.GetCountEnabled = false;
        var uut = GetUut(config);
        //Act/Assert
        await Assert.ThrowsAsync<HttpRequestException>(uut.Count);
    }

    [Fact]
    public async Task Delete_EndpointDisabled()
    {
        //Arrange
        var query = new DeleteCommand<TestoDto>(0);
        var config = GetConfiguration();
        config.DeleteEnabled = false;
        var uut = GetUut(config);
        //Act/Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await uut.Delete(query));
    }

    [Fact]
    public async Task Create_EndpointDisabled()
    {
        //Arrange
        var config = GetConfiguration();
        config.CreateEnabled = false;
        var uut = GetUut(config);
        //Act/Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await uut.Create(new TestoDto(0, 0)));
    }

    [Fact]
    public async Task Update_EndpointDisabled()
    {
        //Arrange
        var config = GetConfiguration();
        config.UpdateEnabled = false;
        var uut = GetUut(config);
        //Act/Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await uut.Update(new TestoDto(0, 0)));
    }

    private CrudController<TestoDto> GetUut(CrudConfiguration configuration) => new TestoController(MockSender.Object, configuration);

    private CrudConfiguration GetConfiguration() => new CrudConfiguration();
}
