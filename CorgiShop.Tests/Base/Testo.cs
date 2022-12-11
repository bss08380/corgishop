using CorgiShop.Pipeline.Model.Abstractions;

namespace CorgiShop.Tests.Base;

public record Testo() : IRepositoryEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }

    public int TestingId { get; set; }
}
