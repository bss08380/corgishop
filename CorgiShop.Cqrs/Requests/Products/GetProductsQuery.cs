using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Biz.Requests.Products
{
    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string Filter { get; set; } = string.Empty;
    }
}
