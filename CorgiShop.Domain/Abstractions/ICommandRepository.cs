namespace CorgiShop.Domain.Abstractions;

public interface ICommandRepository<T> where T : IRepositoryEntity
{
    Task Delete(int id);
    Task SoftDelete(int id);
}
