using AutoMapper;
using CorgiShop.Application.Abstractions;
using CorgiShop.Application.CQRS.Base;
using CorgiShop.Common.Exceptions;
using CorgiShop.Domain.Abstractions;

namespace CorgiShop.Application.Base;

public abstract class GetListPaginatedQueryHandlerBase<TDto, TRepo>
    where TDto : class, IDtoEntity 
    where TRepo : class, IRepositoryEntity
{
    private readonly IRepository<TRepo> _repository;
    private readonly IMapper _mapper;
    private readonly int _maxPageSizeLimit = 0;

    public GetListPaginatedQueryHandlerBase(
        IRepository<TRepo> repository,
        IMapper mapper,
        int maxPageSizeLimit = 200)
    {
        _repository = repository;
        _mapper = mapper;
        _maxPageSizeLimit = maxPageSizeLimit;
    }

    public async Task<PaginatedResultsDto<TDto>> Handle(GetListPaginatedQueryBase<TDto> request, CancellationToken cancellationToken)
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