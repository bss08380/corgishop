using CorgiShop.Application.Abstractions;
using MediatR;

namespace CorgiShop.Application.Base;

public abstract record GetByIdQuery<TDto>(int Id) : IRequest where TDto : class, IDtoEntity;
