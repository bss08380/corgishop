using AutoMapper;
using CorgiShop.Application.Features.Products;
using CorgiShop.Domain.Model;

namespace CorgiShop.Application.AutoMapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
    }
}
