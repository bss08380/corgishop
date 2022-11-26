using AutoMapper;
using CorgiShop.Common.Exceptions;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Requests;
using MediatR;

namespace CorgiShop.Pipeline.Base.Handlers;

public class GetListPaginatedQueryHandler<TDto, TRepo> : IRequestHandler<GetListPaginatedQuery<TDto>, PaginatedResultsDto<TDto>>
    where TDto : class, IDtoEntity
    where TRepo : class, IRepositoryEntity
{
    private readonly IRepository<TRepo> _repository;
    private readonly IMapper _mapper;
    private readonly int _maxPageSizeLimit = 0;

    public GetListPaginatedQueryHandler(
        IRepository<TRepo> repository,
        IMapper mapper,
        int maxPageSizeLimit = 200)
    {
        _repository = repository;
        _mapper = mapper;
        _maxPageSizeLimit = maxPageSizeLimit;
    }

    public async Task<PaginatedResultsDto<TDto>> Handle(GetListPaginatedQuery<TDto> request, CancellationToken cancellationToken)
    {
        if (request.Limit > _maxPageSizeLimit) throw DetailedException.FromFailedVerification(nameof(request.Limit), $"Maximum limit size is {_maxPageSizeLimit}");

        int total = await _repository.Count();
        var results = (await _repository.ListPaginated(request.Limit, request.Offset)).Select(p => _mapper.Map<TRepo, TDto>(p));

        return new PaginatedResultsDto<TDto>()
        {
            Page = QueryPageDto.FromCurrentPage(total, request.Limit, request.Offset),
            TotalAvailable = total,
            TotalReturned = results?.Count() ?? 0,
            Results = results
        };
    }
}