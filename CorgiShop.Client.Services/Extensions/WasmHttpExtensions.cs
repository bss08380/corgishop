using CorgiShop.Client.Services.Abstractions;
using CorgiShop.Client.Services.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CorgiShop.Client.Services.Extensions;

public static class WasmHttpExtensions
{
    public static void AddAuthAndHttpClients(this WebAssemblyHostBuilder builder)
    {
        var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

        //OpenId Connect configuration
        builder.Services.AddOidcAuthentication(options =>
        {
            //Configuration present in appsettings.json
            builder.Configuration.Bind("OpenIdConnect", options.ProviderOptions);
            //Ensure API audience is provided since we're talking to the backend here
            options.ProviderOptions.DefaultScopes.Add("profile");
            options.ProviderOptions.DefaultScopes.Add("email");
            options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["OpenIdConnect:Audience"]);
        });

        //HttpClient instnaces scoped to base address
        //Also registers message handler for handling auth attachment
        builder.Services.AddHttpClient(HttpClientType.SecureClient.FactoryName, client =>
        {
            client.BaseAddress = baseAddress;
        }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        //Unauthed client for accessing public resources
        builder.Services.AddHttpClient(HttpClientType.PublicClient.FactoryName, client => { client.BaseAddress = baseAddress; });
        builder.Services.AddScoped<IApiClient, AuthedApiClient>();
    }
}
