using CorgiShop.Application.Abstractions;
using MediatR;

namespace CorgiShop.Application.Base;

public class GetListQueryBase<TDto> : IRequest
    where TDto : class, IDtoEntity
{
}
