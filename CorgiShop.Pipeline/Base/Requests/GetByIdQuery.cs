using CorgiShop.Pipeline.Model.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base.Requests;

public record GetByIdQuery<TDto>(int Id) : IRequest<TDto> where TDto : class, IDtoEntity;
