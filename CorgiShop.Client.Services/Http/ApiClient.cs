using CorgiShop.Client.Services.Abstractions;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;

namespace CorgiShop.Client.Services.Http;

public class ApiClient : IApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _clientName;

	private HttpClient? _httpClient = null;

    private HttpClient Client => _httpClient ??= _httpClientFactory.CreateClient(_clientName);

    public ApiClient(IHttpClientFactory httpClientFactory, string clientName)
	{
        _httpClientFactory = httpClientFactory;
        _clientName = clientName;
	}

    public async Task<T?> GetDtoAsync<T>(string baseUri, NameValueCollection? queryParameters = null) => await GetDtoAsync<T>(baseUri, CreateQueryString(queryParameters));

    public async Task<T?> GetDtoAsync<T>(string baseUri, string queryString)
    {
        var response = await Client.GetAsync($"{baseUri}{queryString}");

        var dto = await response.Content.ReadAsStringAsync();
        if (dto == null) throw new Exception("Unable to retrieve data from API");

        return JsonSerializer.Deserialize<T>(dto);
    }

    public async Task PostDtoAsync(string uri, object body)
    {
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        await Client.PostAsync(uri, content);
    }

    public async Task PutDtoAsync(string uri, object body)
    {
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
        await Client.PutAsync(uri, content);
    }

    public async Task DeleteDtoAsync(string uri)
    {
        await Client.DeleteAsync(uri);
    }

    private string CreateQueryString(NameValueCollection? queryParameters)
    {
        if (queryParameters == null) return string.Empty;
        if (queryParameters.Count <= 0) return string.Empty;
        return $"?{string.Join('&', queryParameters.AllKeys.Select(k => $"{k}={queryParameters[k]}"))}";
    }
}
