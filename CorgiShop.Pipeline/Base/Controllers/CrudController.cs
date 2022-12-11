﻿using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Requests;
using CorgiShop.Pipeline.Model.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorgiShop.Pipeline.Base.Controllers;

public abstract class CrudController<TDto> : Controller
    where TDto : class, IDtoEntity
{
    private readonly ISender _mediator;
    private readonly CrudConfiguration _configuration;

    public CrudController(ISender sender, CrudConfiguration configuration)
    {
        _mediator = sender;
        _configuration = configuration;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Get([FromQuery] GetListPaginatedQuery<TDto> query)
    {
        if (!_configuration.ListEnabled) throw new HttpRequestException("Listing of this entity type is not allowed");
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetById([FromRoute] GetByIdQuery<TDto> query)
    {
        if (!_configuration.GetByIdEnabled) throw new HttpRequestException("Individual entity retrieval of this entity type is not allowed");
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Count()
    {
        if (!_configuration.GetCountEnabled) throw new HttpRequestException("Total count of this entity type is not allowed");
        return Ok(await _mediator.Send(GetCountQuery<TDto>.Instance));
    }

    [HttpDelete("{Id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete([FromRoute] DeleteCommand<TDto> command)
    {
        if (!_configuration.DeleteEnabled) throw new HttpRequestException("Deltion of this entity type is not allowed");
        command.Mode = _configuration.DeleteMode;
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Create([FromBody] TDto newEntity)
    {
        if (!_configuration.CreateEnabled) throw new HttpRequestException("Creating new instances for this entity type is not allowed");
        return Ok(await _mediator.Send(new CreateCommand<TDto>(newEntity)));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Update([FromBody] TDto entity)
    {
        if (!_configuration.UpdateEnabled) throw new HttpRequestException("Updating instances of this entity type is not allowed");
        return Ok(await _mediator.Send(new UpdateCommand<TDto>(entity)));
    }
}
