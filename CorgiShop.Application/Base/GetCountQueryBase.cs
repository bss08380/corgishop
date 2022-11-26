using CorgiShop.Application.Abstractions;
using MediatR;

namespace CorgiShop.Application.Base;

public abstract record GetCountQueryBase<TDto> : IRequest where TDto : class, IDtoEntity;
