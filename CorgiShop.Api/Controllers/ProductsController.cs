using CorgiShop.Biz.Requests.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CorgiShop.Api.Controllers;

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
    public async Task<ActionResult<IEnumerable<ProductDto>>> Index([FromQuery] GetProductsQuery query) => Ok(await _mediator.Send(query));

    [HttpDelete("{productId}")]
    public async Task<ActionResult> Delete(int productId)
    {
        await _mediator.Send(new DeleteProductCommand(productId));
        return Ok();
    }
}
