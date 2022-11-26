using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorgiShop.Pipeline.Base.Controllers;

public abstract class CrudControllerBase<TDto> : Controller
    where TDto : class, IDtoEntity
{
    private readonly ISender _mediator;
    private readonly CrudConfiguration _configuration;

    public CrudControllerBase(ISender sender, CrudConfiguration configuration)
    {
        _mediator = sender;
        _configuration = configuration;
    }

    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete([FromRoute] DeleteCommand<TDto> command)
    {
        if (!_configuration.DeleteEnabled) throw new HttpRequestException("Deltion of this entity type is not allowed");
        command.Mode = _configuration.DeleteMode;
        return Ok(await _mediator.Send(command));
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetById([FromRoute] GetByIdQuery<TDto> query)
    {
        if (!_configuration.GetByIdEnabled) throw new HttpRequestException("Individual entity retrieval of this entity type is not allowed");
        return Ok(await _mediator.Send(query));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Get([FromQuery] GetListPaginatedQuery<TDto> query)
    {
        if (!_configuration.ListEnabled) throw new HttpRequestException("Listing of this entity type is not allowed");
        return Ok(await _mediator.Send(query));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create([FromQuery] TDto newEntity)
    {
        if (!_configuration.CreateEnabled) throw new HttpRequestException("Creating new instances for this entity type is not allowed");
        return Ok(await _mediator.Send(new CreateCommand<TDto>(newEntity)));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Update([FromQuery] TDto entity)
    {
        if (!_configuration.UpdateEnabled) throw new HttpRequestException("Updating instances of this entity type is not allowed");
        return Ok(await _mediator.Send(new UpdateCommand<TDto>(entity)));
    }
}
