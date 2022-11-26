using CorgiShop.Application.Features.Products;
using CorgiShop.Application.Features.Products.Commands.GenerateProducts;
using CorgiShop.Pipeline.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CorgiShop.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ProductsController : CrudControllerBase<ProductDto>
{
    private readonly ISender _mediator;

    public ProductsController(ISender sender)
        : base(sender)
    {
        _mediator = sender;
    }

    /// <summary>
    /// Generates the provided number of fake corgi-themed products to be added for sale in the CorgiShop
    /// </summary>
    /// <response code="200">If the generation operation completed successfully</response>
    [HttpPost("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Generate([FromQuery] GenerateProductsCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}
