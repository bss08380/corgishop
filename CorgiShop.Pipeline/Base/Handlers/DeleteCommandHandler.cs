using CorgiShop.Pipeline.Abstractions;
using CorgiShop.Pipeline.Base.Requests;
using MediatR;

namespace CorgiShop.Pipeline.Base.Handlers;

public class DeleteCommandHandler<TDto, TEntity> : IRequestHandler<DeleteCommand<TDto>, Unit>
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
        switch (request.Mode)
        {
            case DeleteMode.Hard:
                await _repository.Delete(request.Id);
                break;

            case DeleteMode.Soft:
                await _repository.SoftDelete(request.Id);
                break;
        }
        return Unit.Value;
    }
}
