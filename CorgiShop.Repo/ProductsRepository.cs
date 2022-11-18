using CorgiShop.Repo.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Repo
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly CorgiShopDbContext _corgiShopDbContext;

        public ProductsRepository(CorgiShopDbContext corgiShopDbContext)
        {
            _corgiShopDbContext = corgiShopDbContext;
        }

        public async Task<IEnumerable<Product>> GetAll(string nameFilter)
        {
            return await _corgiShopDbContext.Products
                .Where(p => p.IsDeleted == false)
                .ToListAsync();
        }

        public async Task Delete(int productId)
        {
            var product = await _corgiShopDbContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null) throw new InvalidOperationException("Invalid product ID - product not found");

            product.IsDeleted = true;
            await _corgiShopDbContext.SaveChangesAsync();
        }
    }
}
