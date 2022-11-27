using CorgiShop.Pipeline.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base;

public record GetCountQuery<TDto> : IRequest<int> where TDto : class, IDtoEntity
{
    private static GetCountQuery<TDto> _instance = new GetCountQuery<TDto>();

    public static GetCountQuery<TDto> Instance => _instance;
}
