using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorgiShop.Application.Requests.Products
{
    public record GenerateProductsCommand([Required] int NumberToGenerate) : IRequest;
}
