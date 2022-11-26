using CorgiShop.Pipeline.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base;

public abstract record GetCountQuery<TDto> : IRequest where TDto : class, IDtoEntity;
