using CorgiShop.Pipeline.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base;

public record GetCountQuery<TDto> : IRequest where TDto : class, IDtoEntity;
