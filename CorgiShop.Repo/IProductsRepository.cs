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
        Task<IEnumerable<Product>> GetAll(string nameFilter);
        Task Delete(int productId);
    }
}
