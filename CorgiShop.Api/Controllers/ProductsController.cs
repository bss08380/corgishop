using CorgiShop.Biz.Requests.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorgiShop.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly ISender _mediator;

        public ProductsController(ISender sender)
        {
            _mediator = sender;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Index()
        {
            var result = await _mediator.Send(new GetProductsQuery());
            return Ok(result);
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> Delete(int productId)
        {
            await _mediator.Send(new DeleteProductCommand(productId));
            return Ok();
        }
    }
}
