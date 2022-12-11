using AutoMapper;
using CorgiShop.Common.Exceptions;
using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Requests;
using CorgiShop.Pipeline.Model.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base.Handlers;

public class UpdateCommandHandler<TDto, TEntity> : IRequestHandler<UpdateCommand<TDto>, TDto>
    where TDto : class, IDtoEntity
    where TEntity : class, IRepositoryEntity
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public UpdateCommandHandler(
        IRepository<TEntity> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<TDto> Handle(UpdateCommand<TDto> request, CancellationToken cancellationToken)
    {
        var entityToUpdate = _mapper.Map<TEntity>(request.entity);
        if (entityToUpdate == null) throw DetailedException.FromFailedVerification("entity", "Could not process provided object for update");

        var updatedEntity = await _repository.Update(entityToUpdate);
        return _mapper.Map<TDto>(updatedEntity);
    }
}
