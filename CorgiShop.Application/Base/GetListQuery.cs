using CorgiShop.Application.Abstractions;
using MediatR;

namespace CorgiShop.Application.Base;

public class GetListQuery<TDto> : IRequest
    where TDto : class, IDtoEntity
{
}
