using CorgiShop.Application.Abstractions;
using CorgiShop.Application.CQRS.Base;
using MediatR;

namespace CorgiShop.Application.Base;

public record GetListPaginatedQuery<TDto>(int Limit, int Offset) : IRequest<PaginatedResultsDto<TDto>> where TDto : class, IDtoEntity;
