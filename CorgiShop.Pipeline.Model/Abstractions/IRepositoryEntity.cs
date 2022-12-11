namespace CorgiShop.Pipeline.Model.Abstractions;

public interface IRepositoryEntity
{
    int Id { get; set; }
    bool IsDeleted { get; set; }
}
