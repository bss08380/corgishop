using CorgiShop.Client.Services.Abstractions;

namespace CorgiShop.Client.Services.Http;

public abstract class AuthedApiClient : ApiClient, IAuthedApiClient
{
    public IApiClient WithAuth { get; }

    public AuthedApiClient(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory, HttpClientType.PublicClient.FactoryName)
	{
        WithAuth = new ApiClient(httpClientFactory, HttpClientType.SecureClient.FactoryName);
    }
}
