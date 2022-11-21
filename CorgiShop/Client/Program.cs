using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CorgiShop;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        ConfigureOpenIdConnect(builder);

        //HttpClient instnaces scoped to base address
        //Also registers message handler for handling auth attachment
        builder.Services.AddHttpClient("CorgiShop.BackendApi", client =>
        {
            client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        }).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        //Created clients use the correct configuration so that auth flows to backend API
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("CorgiShop.BackendApi"));

        await builder.Build().RunAsync();
    }

    private static void ConfigureOpenIdConnect(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddOidcAuthentication(options =>
        {
            //Configuration present in appsettings.json
            builder.Configuration.Bind("OpenIdConnect", options.ProviderOptions);
            //Ensure API audience is provided since we're talking to the backend here
            options.ProviderOptions.DefaultScopes.Add("profile");
            options.ProviderOptions.DefaultScopes.Add("email");
            options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["OpenIdConnect:Audience"]);
        });
    }
}