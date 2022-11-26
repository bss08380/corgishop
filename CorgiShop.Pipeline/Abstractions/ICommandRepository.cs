namespace CorgiShop.Pipeline.Abstractions;

public interface ICommandRepository<T> where T : IRepositoryEntity
{
    Task Create(T newEntity);
    Task Update(T entity);
    Task Delete(int id);
    Task SoftDelete(int id);
}
