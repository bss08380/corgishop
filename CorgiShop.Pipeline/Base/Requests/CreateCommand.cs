using CorgiShop.Pipeline.Model.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base.Requests;

public record CreateCommand<TDto>(TDto newEntity) : IRequest<TDto> where TDto : class, IDtoEntity;
