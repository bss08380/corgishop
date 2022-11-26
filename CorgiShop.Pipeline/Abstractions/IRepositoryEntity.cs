namespace CorgiShop.Pipeline.Abstractions;

public interface IRepositoryEntity
{
    int Id { get; set; }
    bool IsDeleted { get; set; }
}
