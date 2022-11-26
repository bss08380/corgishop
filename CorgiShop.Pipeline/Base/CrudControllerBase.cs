using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorgiShop.Pipeline.Base;

public abstract class CrudControllerBase<TDto> : Controller
    where TDto : class, IDtoEntity
{
    private readonly ISender _mediator;

    public CrudControllerBase(ISender sender)
    {
        _mediator = sender;
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete([FromQuery] DeleteCommand<TDto> query) => Ok(await _mediator.Send(query));

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Get([FromQuery] GetListPaginatedQuery<TDto> query) => Ok(await _mediator.Send(query));
}
