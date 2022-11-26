using CorgiShop.Api.Base;
using CorgiShop.Application.Features.Products;
using CorgiShop.Application.Features.Products.Commands.GenerateProducts;
using CorgiShop.Application.Features.Products.Queries.GetProducts;
using CorgiShop.Domain.Model;
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

    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] GetProductsListPaginatedQuery query) => await base.CrudGet(query);

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
