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

    protected virtual async Task<ActionResult> CrudGet(GetListPaginatedQueryBase<TDto> query) => Ok(await _mediator.Send(query));
}
