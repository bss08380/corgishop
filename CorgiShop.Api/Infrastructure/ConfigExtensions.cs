using Microsoft.Extensions.Configuration;

namespace CorgiShop.Api.Infrastructure
{
    public static class ConfigExtensions
    {
        public static void AddDeploymentConfiguration(this WebApplicationBuilder? builder)
        {
            if (builder == null) return;

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile("deploy.json");
            configBuilder.AddJsonFile($"deploy.{DeploymentConfig.Configuration}.json");

            builder.Configuration.AddConfiguration(configBuilder.Build());
        }
    }
}
