using CorgiShop.Application.Model.Features.Products;
using CorgiShop.Pipeline.Model.Abstractions;
using CorgiShop.Pipeline.Model.Base;

namespace CorgiShop.Client.Services.Abstractions;

public interface ICrudService<T>
    where T : class, IDtoEntity
{
    Task<PaginatedResultsDto<ProductDto>?> GetPaginated(int limit, int offset);
    Task<ProductDto?> GetById(int id);
    Task<int?> Count();

    Task Create(T dto);
    Task Update(T dto);
    Task Delete(int id);
}
