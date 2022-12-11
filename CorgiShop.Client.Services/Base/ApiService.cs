using CorgiShop.Client.Services.Abstractions;
using CorgiShop.Client.Services.Http;

namespace CorgiShop.Client.Services.Base;

public abstract class ApiService
{
	private readonly IAuthedApiClient _authedApiClient;

	public ApiService(IAuthedApiClient authedApiClient)
	{
        _authedApiClient = authedApiClient;
    }

    protected IApiClient GetClient(bool secure) => secure ? _authedApiClient.WithAuth : _authedApiClient;
}
