using CorgiShop.Pipeline.Abstractions;

namespace CorgiShop.Tests.Base;

public record TestoDto(int Id, int TestingId = 0) : IDtoEntity;
