namespace CorgiShop.Domain.Abstractions;

public  interface IRepository<T> : IQueryRepository<T>, ICommandRepository<T>
    where T : class, IRepositoryEntity
{
}
