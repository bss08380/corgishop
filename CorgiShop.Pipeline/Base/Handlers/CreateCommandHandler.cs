using AutoMapper;
using CorgiShop.Common.Exceptions;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Requests;
using MediatR;

namespace CorgiShop.Pipeline.Base.Handlers;

public class CreateCommandHandler<TDto, TEntity> : IRequestHandler<CreateCommand<TDto>, TDto>
    where TDto : class, IDtoEntity
    where TEntity : class, IRepositoryEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public CreateCommandHandler(
        IRepository<TEntity> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TDto> Handle(CreateCommand<TDto> request, CancellationToken cancellationToken)
    {
        var newEntity = _mapper.Map<TEntity>(request.newEntity);
        if (newEntity == null) throw DetailedException.FromFailedVerification("newEntity", "Could not process provided object for storage");

        var createdEntity = await _repository.Create(newEntity);
        return _mapper.Map<TDto>(createdEntity);
    }
}
