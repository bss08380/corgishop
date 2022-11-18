using CorgiShop.Repo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Repo
{
    public interface IProductsRepository
    {
        Task<int> GetTotalAvailable();
        Task<IEnumerable<Product>> GetPaginated(int limit, int offset);
        Task Delete(int productId);
    }
}
