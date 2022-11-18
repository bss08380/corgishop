using AutoMapper;
using CorgiShop.Biz.Requests.Products;
using CorgiShop.Repo.Model;

namespace CorgiShop.Biz.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
