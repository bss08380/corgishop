using CorgiShop.Common.Exceptions;
using CorgiShop.Domain.Model;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base;
using CorgiShop.Tests.Base;

namespace CorgiShop.Tests.Pipeline.Base;

/*
 * Product entities are used for testing here
 * But all operations in RepositoryBase are generic using IRepositoryEntity
 * So any IRepositoryEntity behaves the same
 */
public class RepositoryBaseTests : TestBase
{
    [Fact]
    public async Task GetPaginated_NoneDeleted()
    {
        //Arrange
        var uut = await GetUut(10, 0);
        //Act
        var entities = await uut.ListPaginated(100, 0);
        //Assert
        Assert.Equal(10, entities.Count());
    }

    [Fact]
    public async Task GetPaginated_SomeDeleted()
    {
        //Arrange
        var uut = await GetUut(10, 5);
        //Act
        var entities = await uut.ListPaginated(100, 0);
        //Assert
        Assert.Equal(10, entities.Count());
    }

    [Fact]
    public async Task GetPaginated_AllDeleted()
    {
        //Arrange
        var uut = await GetUut(0, 15);
        //Act
        var entities = await uut.ListPaginated(100, 0);
        //Assert
        Assert.Empty(entities);
    }

    [Fact]
    public async Task GetPaginated_DbMaxFetchLimit()
    {
        //Arrange
        var uut = await GetUut(500, 0);
        //Act
        var entities = await uut.ListPaginated(1000, 0);
        //Assert
        Assert.Equal(200, entities.Count());
    }

    [Fact]
    public async Task GetPaginated_LimitOffset_CorrectAmt()
    {
        //Arrange
        var uut = await GetUut(100, 0);
        //Act
        var entities = await uut.ListPaginated(10, 0);
        //Assert
        Assert.Equal(10, entities.Count());
    }

    [Fact]
    public async Task GetPaginated_LimitOffset_SequentialResultsFromZero()
    {
        //Arrange
        var uut = await GetUut(20, 0);
        //Act
        var entities = await uut.ListPaginated(10, 0);
        //Assert
        Assert.True(VerifySequentialCollectionByPidName(entities, 0));
    }

    [Fact]
    public async Task GetPaginated_LimitOffset_SequentialResultsFromMidpoint()
    {
        //Arrange
        var uut = await GetUut(20, 0);
        //Act
        var entities = await uut.ListPaginated(10, 10);
        //Assert
        Assert.True(VerifySequentialCollectionByPidName(entities, 10));
    }

    [Fact]
    public async Task Delete_SoftDeleted()
    {
        //Arrange
        var uut = await GetUut(10, 0);
        var firstTestoId = UnitTestDbContext!.Testos.FirstOrDefault()!.Id;
        //Act
        await uut.SoftDelete(firstTestoId);
        //Assert
        Assert.Equal(10, UnitTestDbContext.Testos.Count());
        Assert.Single(UnitTestDbContext.Testos.Where(p => p.IsDeleted));
        Assert.True(UnitTestDbContext!.Testos.FirstOrDefault(p => p.Id == firstTestoId)!.IsDeleted);
    }

    [Fact]
    public async Task Delete_InvalidId_Exception()
    {
        //Arrange
        var uut = await GetUut(10, 0);
        //Act
        var action = async () => await uut.Delete(0); //0 is never assigned by EF
        //Assert
        await Assert.ThrowsAsync<DetailedException>(action);
    }

    private async Task<IRepository<Testo>> GetUut(int testoCnt, int deletedCnt)
    {
        int testingId = 0;
        var dbContext = await RigUnitTestDbContext(db =>
        {
            for (var i = 0; i < testoCnt; i++)
            {
                db.Testos.Add(NewProduct(false, testingId));
                testingId++;
            }
            for (var i = 0; i < deletedCnt; i++)
            {
                db.Testos.Add(NewProduct(true, testingId));
                testingId++;
            }
        });
        return new RepositoryBase<Testo>(dbContext);
    }

    private Testo NewProduct(bool isDeleted, int testingId)
    {
        return new Testo()
        {
            IsDeleted = isDeleted,
            TestingId = testingId
        };
    }

    private bool VerifySequentialCollectionByPidName(IEnumerable<Testo> products, int startPid)
    {
        int pid = startPid;
        foreach (var product in products)
        {
            if (!product.IsDeleted && product.TestingId != pid)
                return false;
            pid++;
        }
        return true;
    }

}
