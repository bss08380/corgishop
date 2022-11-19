using CorgiShop.Common.Ioc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Application
{
    public class ApplicationIocHelper : IIocHelper
    {
        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(ApplicationIocHelper).Assembly);
        }
    }
}
