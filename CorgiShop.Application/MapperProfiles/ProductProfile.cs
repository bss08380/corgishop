using AutoMapper;
using CorgiShop.Application.Requests.Products;
using CorgiShop.Domain.Model;

namespace CorgiShop.Application.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
