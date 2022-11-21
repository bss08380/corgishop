using CorgiShop.Application.Middleware;
using CorgiShop.Common.Ioc;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Application;

public class ApplicationIocHelper : IIocHelper
{
    public void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddAutoMapper(typeof(ApplicationIocHelper).Assembly);
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    }
}
