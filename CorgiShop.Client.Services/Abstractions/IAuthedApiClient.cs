namespace CorgiShop.Client.Services.Abstractions;

public interface IAuthedApiClient : IApiClient
{
    IApiClient WithAuth { get; }
}
