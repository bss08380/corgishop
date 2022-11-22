using CorgiShop.Application.Features.Products.Commands.DeleteProduct;
using CorgiShop.Application.Features.Products.Commands.GenerateProducts;
using CorgiShop.Application.Features.Products.Queries.GetProducts;
using CorgiShop.Common.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CorgiShop.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ProductsController : Controller
{
    private readonly ISender _mediator;

    public ProductsController(ISender sender)
    {
        _mediator = sender;
    }

    /// <summary>
    /// Gets products (paginated) which are available in the CorgiShop
    /// </summary>
    /// <returns>Paginated product data, including page information</returns>
    /// <response code="200">Returns the product data and pagination details</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Get([FromQuery] GetProductsQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    /// <summary>
    /// Deletes a product which is available in the CorgiShop
    /// </summary>
    /// <response code="200">If the deletion was successful</response>
    /// <response code="400">If the provided ProductID is not found in the database</response>
    [HttpDelete("{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
    public async Task<ActionResult> Delete([FromRoute] DeleteProductCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Generates the provided number of fake corgi-themed products to be added for sale in the CorgiShop
    /// </summary>
    /// <returns>Paginated product data, including page information</returns>
    /// <response code="200">If the generation operation completed successfully</response>
    [HttpPost("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Generate([FromQuery] GenerateProductsCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }
}
