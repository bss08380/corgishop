using CorgiShop.Common.Model;

namespace CorgiShop.Common.Exceptions;

public interface IDetailedException
{
    ErrorDetails Details { get; }
}
