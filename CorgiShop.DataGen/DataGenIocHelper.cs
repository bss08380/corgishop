using CorgiShop.Common.Ioc;
using CorgiShop.DataGen.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CorgiShop.DataGen;

public class DataGenIocHelper : IIocHelper
{
    public void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IProductDataGenService, ProductDataGenService>();
    }
}
