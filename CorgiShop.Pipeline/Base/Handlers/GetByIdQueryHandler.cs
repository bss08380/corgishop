using AutoMapper;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Requests;
using MediatR;

namespace CorgiShop.Pipeline.Base.Handlers;

public class GetByIdQueryHandler<TDto, TRepo> : IRequestHandler<GetByIdQuery<TDto>, TDto>
    where TDto : class, IDtoEntity
    where TRepo : class, IRepositoryEntity
{
    private readonly IRepository<TRepo> _repository;
    private readonly IMapper _mapper;

    public GetByIdQueryHandler(
        IRepository<TRepo> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TDto> Handle(GetByIdQuery<TDto> request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id);
        return _mapper.Map<TRepo, TDto>(result);
    }
}