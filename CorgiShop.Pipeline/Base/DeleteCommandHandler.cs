using CorgiShop.Pipeline.Abstractions;
using MediatR;

namespace CorgiShop.Pipeline.Base;

public class DeleteCommandHandler<TDto, TEntity> : IRequestHandler<DeleteCommand<TDto>>
    where TDto : class, IDtoEntity
    where TEntity : class, IRepositoryEntity
{
    private readonly IRepository<TEntity> _repository;

    public DeleteCommandHandler(
        IRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteCommand<TDto> request, CancellationToken cancellationToken)
    {
        await _repository.Delete(request.Id);
        return Unit.Value;
    }
}
