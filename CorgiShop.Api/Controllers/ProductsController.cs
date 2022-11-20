using CorgiShop.Application.Requests.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult> Get([FromQuery] GetProductsQuery query) => Ok(await _mediator.Send(query));

    [HttpDelete("{productId}")]
    public async Task<ActionResult> Delete([FromQuery] DeleteProductCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("generate")]
    public async Task<ActionResult> Generate([FromQuery] GenerateProductsCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}
