using CorgiShop.Pipeline.Base.Requests;

namespace CorgiShop.Pipeline.Base.Controllers;

public class CrudConfiguration
{
    public DeleteMode DeleteMode { get; set; } = DeleteMode.Soft;

    public bool DeleteEnabled { get; set; } = true;
    public bool ListEnabled { get; set; } = true;
    public bool CreateEnabled { get; set; } = true;
    public bool UpdateEnabled { get; set; } = true;
    public bool GetByIdEnabled { get; set; } = true;
}
