using CorgiShop.Pipeline.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base;

public abstract record GetByIdQuery<TDto>(int Id) : IRequest where TDto : class, IDtoEntity;
