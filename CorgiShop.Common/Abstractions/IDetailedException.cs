using CorgiShop.Common.Model;

namespace CorgiShop.Common.Abstractions;

public interface IDetailedException
{
    ErrorDetails Details { get; }
}
