using MediatR;

namespace CorgiShop.Application.Requests.Products;

public class GetProductsQuery : IRequest<GetProductsDto>
{
    public int Limit { get; set; }
    public int Offset { get; set; }

    public string Filter { get; set; } = string.Empty;
}
