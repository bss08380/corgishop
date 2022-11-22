using CorgiShop.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Domain;

public interface IProductsRepository
{
    Task<int> GetTotalAvailable();
    Task<IEnumerable<Product>> GetPaginated(int limit, int offset);
    Task Delete(int productId);
}
