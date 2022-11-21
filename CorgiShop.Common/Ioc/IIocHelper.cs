using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Common.Ioc;

public interface IIocHelper
{
    void RegisterServices(IServiceCollection serviceCollection);
}
