using CorgiShop.Repo;
using CorgiShop.Repo.Model;
using CorgiShop.Tests.Base;
using System.Security.Cryptography;

namespace CorgiShop.Tests.Repo
{
    public class ProductsRepositoryTests : TestBase
    {
        private Random _rand = new Random();

        [Fact]
        public async Task GetPaginated_NoneDeleted()
        {
            //Arrange
            var uut = await GetUut(10, 0);
            //Act
            var products = await uut.GetPaginated(100, 0);
            //Assert
            Assert.Equal(10, products.Count());
        }

        [Fact]
        public async Task GetPaginated_SomeDeleted()
        {
            //Arrange
            var uut = await GetUut(10, 5);
            //Act
            var products = await uut.GetPaginated(100, 0);
            //Assert
            Assert.Equal(10, products.Count());
        }

        [Fact]
        public async Task GetPaginated_AllDeleted()
        {
            //Arrange
            var uut = await GetUut(0, 15);
            //Act
            var products = await uut.GetPaginated(100, 0);
            //Assert
            Assert.Empty(products);
        }

        [Fact]
        public async Task GetPaginated_DbMaxFetchLimit()
        {
            //Arrange
            var uut = await GetUut(500, 0);
            //Act
            var products = await uut.GetPaginated(1000, 0);
            //Assert
            Assert.Equal(200, products.Count());
        }

        [Fact]
        public async Task GetPaginated_LimitOffset_CorrectAmt()
        {
            //Arrange
            var uut = await GetUut(100, 0);
            //Act
            var products = await uut.GetPaginated(10, 0);
            //Assert
            Assert.Equal(10, products.Count());
        }

        [Fact]
        public async Task GetPaginated_LimitOffset_SequentialResultsFromZero()
        {
            //Arrange
            var uut = await GetUut(20, 0);
            //Act
            var products = await uut.GetPaginated(10, 0);
            //Assert
            Assert.True(VerifySequentialCollectionByPidName(products, 0));
        }

        [Fact]
        public async Task GetPaginated_LimitOffset_SequentialResultsFromMidpoint()
        {
            //Arrange
            var uut = await GetUut(20, 0);
            //Act
            var products = await uut.GetPaginated(10, 10);
            //Assert
            Assert.True(VerifySequentialCollectionByPidName(products, 10));
        }

        [Fact]
        public async Task Delete_SoftDeleted()
        {
            //Arrange
            var uut = await GetUut(10, 0);
            var firstProductId = DbContext!.Products.FirstOrDefault()!.ProductId;
            //Act
            await uut.Delete(firstProductId);
            //Assert
            Assert.Equal(10, DbContext.Products.Count());
            Assert.Single(DbContext.Products.Where(p => p.IsDeleted));
            Assert.True(DbContext!.Products.FirstOrDefault(p => p.ProductId == firstProductId)!.IsDeleted);
        }

        [Fact]
        public async Task Delete_InvalidId_Exception()
        {
            //Arrange
            var uut = await GetUut(10, 0);
            //Act
            var action = async () => await uut.Delete(0); //0 is never assigned by EF
            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(action);
        }

        private async Task<IProductsRepository> GetUut(int productCnt, int deletedCnt)
        {
            int testPid = 0;
            var dbContext = await RigCorgiShopDbContext(db =>
            {
                for (var i = 0; i < productCnt; i++)
                {
                    db.Products.Add(NewProduct(false, testPid));
                    testPid++;
                }

                for (var i = 0; i < deletedCnt; i++)
                {
                    db.Products.Add(NewProduct(true, testPid));
                    testPid++;
                }
            });
            return new ProductsRepository(dbContext);
        }

        private Product NewProduct(bool isDeleted, int testingPid)
        {
            return new Product() {
                Name = GetTestingIdxString(testingPid),
                Description = testingPid.ToString(),
                Price = 0.0M,
                Stock = 0,
                IsDeleted = isDeleted
                };
        }

        private bool VerifySequentialCollectionByPidName(IEnumerable<Product> products, int startPid)
        {
            int pid = startPid;
            foreach (var product in products)
            {
                if (!product.IsDeleted && product.Name != GetTestingIdxString(pid))
                    return false;
                pid++;
            }
            return true;
        }

        private string GetTestingIdxString(int value)
        {
            return ((char)(0x41 + value)).ToString();
        }

    }
}
