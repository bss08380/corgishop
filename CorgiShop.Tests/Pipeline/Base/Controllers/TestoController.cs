using CorgiShop.Pipeline.Base.Controllers;
using CorgiShop.Tests.Base;
using MediatR;

namespace CorgiShop.Tests.Pipeline.Base.Controllers;

internal class TestoController : CrudController<TestoDto>
{
    public TestoController(
        ISender sender,
        CrudConfiguration configuration)
        : base(sender, configuration)
    {
    }
}
