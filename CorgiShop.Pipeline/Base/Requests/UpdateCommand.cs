using CorgiShop.Pipeline.Model.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base.Requests;

public record UpdateCommand<TDto>(TDto entity) : IRequest<TDto> where TDto : class, IDtoEntity;
