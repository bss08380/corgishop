using CorgiShop.Common.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Repo
{
    public class RepoIocHelper : IIocHelper
    {
        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IProductsRepository, ProductsRepository>();
        }
    }
}
