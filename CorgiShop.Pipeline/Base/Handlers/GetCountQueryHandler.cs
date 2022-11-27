using CorgiShop.Pipeline.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base.Handlers;

public class GetCountQueryHandler<TDto, TRepo> : IRequestHandler<GetCountQuery<TDto>, int>
    where TDto : class, IDtoEntity
    where TRepo : class, IRepositoryEntity
{
    private readonly IRepository<TRepo> _repository;

    public GetCountQueryHandler(
        IRepository<TRepo> repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(GetCountQuery<TDto> request, CancellationToken cancellationToken) => await _repository.Count();
}