using CorgiShop.Client.Services.Features.Products;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CorgiShop.Client.Services.Extensions;

public static class ClientIocExtensions
{
    public static void AddCorgiShopServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddTransient<IProductService, ProductService>();
    }
}
