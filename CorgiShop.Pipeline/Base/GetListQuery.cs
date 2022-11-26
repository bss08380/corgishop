using CorgiShop.Pipeline.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base;

public class GetListQuery<TDto> : IRequest
    where TDto : class, IDtoEntity
{
}
