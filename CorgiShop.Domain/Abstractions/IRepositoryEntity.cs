namespace CorgiShop.Domain.Abstractions;

public interface IRepositoryEntity
{
    int Id { get; set; }
    bool IsDeleted { get; set; }
}
