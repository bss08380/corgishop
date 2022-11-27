namespace CorgiShop.Pipeline.Abstractions;

public interface ICommandRepository<T> where T : IRepositoryEntity
{
    Task<T> Create(T newEntity);
    Task<T> Update(T entity);
    Task Delete(int id);
    Task SoftDelete(int id);
}
