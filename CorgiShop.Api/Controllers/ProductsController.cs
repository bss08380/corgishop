using CorgiShop.Application.Requests.DataGen;
using CorgiShop.Application.Requests.Products;
using CorgiShop.Domain.Model;
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

    [HttpPost("generate")]
    public async Task<ActionResult> Generate([FromQuery] GenerateProductsCommand command)
    {
        await _mediator.Send(command);
        return Ok();
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
