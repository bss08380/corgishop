using CorgiShop.Pipeline.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base;

public record DeleteCommand<TDto>(int Id) : IRequest where TDto : class, IDtoEntity;
