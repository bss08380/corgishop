using CorgiShop.Pipeline.Model.Abstractions;
using CorgiShop.Pipeline.Model.Base;
using MediatR;

namespace CorgiShop.Pipeline.Base.Requests;

public record GetListPaginatedQuery<TDto>(int Limit, int Offset) : IRequest<PaginatedResultsDto<TDto>> where TDto : class, IDtoEntity;
