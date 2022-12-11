using CorgiShop.Application.Model.Features.Products;
using CorgiShop.Client.Services.Abstractions;
using CorgiShop.Pipeline.Model.Abstractions;
using CorgiShop.Pipeline.Model.Base;

namespace CorgiShop.Client.Services.Base;

public abstract class CrudService<T> : ApiService, ICrudService<T>
    where T : class, IDtoEntity
{
	private readonly string _urlBase;
	private readonly CrudServiceAuthConfiguration _authConfig;


    public CrudService(IAuthedApiClient apiClient, CrudServiceAuthConfiguration authConfig)
		: base(apiClient)
	{
        _urlBase = typeof(T).Name.ToLower();
		_authConfig = authConfig;
    }

	public CrudService(string urlBase, IAuthedApiClient apiClient, CrudServiceAuthConfiguration authConfig)
        : base(apiClient)
    {
		_urlBase= urlBase;
		_authConfig = authConfig;
	}

    public async Task<PaginatedResultsDto<ProductDto>?> GetPaginated(int limit, int offset) => 
		await GetClient(EndpointType.GetPaginated).GetDtoAsync<PaginatedResultsDto<ProductDto>>(_urlBase, $"?limit={limit}&offset={offset}");

    public async Task<ProductDto?> GetById(int id) =>
        await GetClient(EndpointType.GetById).GetDtoAsync<ProductDto>($"{_urlBase}/{id}");

    public async Task<int?> Count() =>
        await GetClient(EndpointType.Count).GetDtoAsync<int>($"{_urlBase}/count");

    public async Task Create(T dto) => 
        await GetClient(EndpointType.Create).PostDtoAsync($"{_urlBase}", dto);

    public async Task Update(T dto) =>
        await GetClient(EndpointType.Update).PutDtoAsync($"{_urlBase}", dto);

    public async Task Delete(int id) =>
        await GetClient(EndpointType.Delete).DeleteDtoAsync($"{_urlBase}/{id}");

    private IApiClient GetClient(EndpointType endpoint) => GetClient(_authConfig.AuthedEndpoints.Contains(endpoint));
}
