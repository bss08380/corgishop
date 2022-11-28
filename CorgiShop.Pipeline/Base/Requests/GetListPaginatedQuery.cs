using CorgiShop.Pipeline.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base.Requests;

public record GetListPaginatedQuery<TDto>(int Limit, int Offset) : IRequest<PaginatedResultsDto<TDto>> where TDto : class, IDtoEntity;
