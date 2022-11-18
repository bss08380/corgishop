using CorgiShop.Repo;
using CorgiShop.Repo.Model;
using CorgiShop.Tests.Base;

namespace CorgiShop.Tests.Repo
{
    public class ProductsRepositoryTests : TestBase
    {
        private Random _rand = new Random();

        [Fact]
        public async Task GetAll_NoneDeleted()
        {
            //Arrange
            var uut = await GetUut(10, 0);
            //Act
            var products = await uut.GetAll("");
            //Assert
            Assert.Equal(10, products.Count());
        }

        [Fact]
        public async Task GetAll_SomeDeleted()
        {
            //Arrange
            var uut = await GetUut(10, 5);
            //Act
            var products = await uut.GetAll("");
            //Assert
            Assert.Equal(10, products.Count());
        }

        [Fact]
        public async Task GetAll_AllDeleted()
        {
            //Arrange
            var uut = await GetUut(0, 15);
            //Act
            var products = await uut.GetAll("");
            //Assert
            Assert.Empty(products);
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
            var dbContext = await RigCorgiShopDbContext(db =>
            {
                for (var i = 0; i < productCnt; i++)
                {
                    db.Products.Add(NewProduct(false));
                }

                for (var i = 0; i < deletedCnt; i++)
                {
                    db.Products.Add(NewProduct(true));
                }
            });
            return new ProductsRepository(dbContext);
        }

        private Product NewProduct(bool isDeleted)
        {
            return new Product() {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = 0.0M,
                Stock = 0,
                IsDeleted = isDeleted
                };
        }

    }
}
