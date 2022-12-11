using CorgiShop.Pipeline.Model.Abstractions;

namespace CorgiShop.Tests.Base;

public record TestoDto(int Id, int TestingId = 0) : IDtoEntity;
