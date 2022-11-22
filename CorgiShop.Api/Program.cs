using System.IdentityModel.Tokens.Jwt;
using CorgiShop.Api.Infrastructure;
using CorgiShop.Api.Middleware;

namespace CorgiShop.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureDeploymentConfiguration();
        builder.ConfigureLogging();
        builder.ConfigureDbContexts();
        builder.ConfigureCacheServices();
        builder.ConfigureAutoMapper();
        builder.ConfigureMediatrServices();
        builder.ConfigureCorgiShopServices();
        builder.ConfigureWebServices();

        var app = builder.Build();
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        if (app.Environment.IsDevelopment()) app.UseWebAssemblyDebugging();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.ConfigureExceptionHandler();
        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}