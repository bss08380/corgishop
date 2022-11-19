using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Application.Requests.Products
{
    public  record DeleteProductCommand(int ProductId) : IRequest;
}
