using MediatR;

namespace CorgiShop.Biz.Requests.Products;

public class GetProductsQuery : IRequest<GetProductsDto>
{
    public int Limit { get; set; }
    public int Offset { get; set; }

    public string Filter { get; set; } = string.Empty;
}
