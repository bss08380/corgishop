using CorgiShop.Application.Abstractions;
using CorgiShop.Application.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CorgiShop.Api.Base;

public abstract class CrudControllerBase<TDto> : Controller
    where TDto : class, IDtoEntity
{
    private readonly ISender _mediator;

    public CrudControllerBase(ISender sender)
    {
        _mediator = sender;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Get([FromQuery] GetListPaginatedQuery<TDto> query) => Ok(await _mediator.Send(query));
}
