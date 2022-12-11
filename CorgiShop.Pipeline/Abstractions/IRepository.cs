using CorgiShop.Pipeline.Model.Abstractions;

namespace CorgiShop.Pipeline.Abstractions;

public  interface IRepository<T> : IQueryRepository<T>, ICommandRepository<T> where T : class, IRepositoryEntity
{
}
