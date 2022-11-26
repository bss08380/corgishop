using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CorgiShop.Application.Extensions;

public static class ApplicationIocExtensions
{
    public static void AddMediatrForAppLayer(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ApplicationIocExtensions).Assembly);
    }

    public static void AddAutoMapperForAppLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationIocExtensions).Assembly);
    }
}
