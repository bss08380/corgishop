using System.Collections.Specialized;

namespace CorgiShop.Client.Services.Abstractions;

public interface IApiClient
{
    Task<T?> GetDtoAsync<T>(string baseUri, string queryString);
    Task<T?> GetDtoAsync<T>(string baseUri, NameValueCollection? queryParameters = null);

    Task PutDtoAsync(string baseUri, object body);
    Task PostDtoAsync(string baseUri, object body);

    Task DeleteDtoAsync(string baseUri);
}
