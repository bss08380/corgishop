using CorgiShop.Api.Authorization;
using CorgiShop.Application.Caching;
using CorgiShop.Application.Features.Products.Queries.GetProducts;
using CorgiShop.Application.Middleware;
using CorgiShop.Common.Settings;
using CorgiShop.DataGen.Services;
using CorgiShop.Domain.Features.Products;
using CorgiShop.Domain.Model;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using System.Reflection;

namespace CorgiShop.Api.Infrastructure;

public static class BuilderExtensions
{
    /*
     * Below is the "deployment configuration" loading
     * 
     * I build this into the API because ASP.NET Core Blazor hosting does not working outside of
     * bare bones Development/Production configurations
     * 
     * I do not like this and have not decided how BEST to handle the constraints, SO
     * I have added this little doodad to provide another level of abstraction to
     * which deployment configuration I load
     */
    public static void ConfigureDeploymentConfiguration(this WebApplicationBuilder? builder)
    {
        if (builder == null) return;

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("deploy.json");
        configBuilder.AddJsonFile($"deploy.{DeploymentConfig.Configuration}.json");

        builder.Configuration.AddConfiguration(configBuilder.Build());
    }

    /*
     * NLog is used for logging in this project
     * See the nlog.config XML file for details
     * 
     * Currently logs are written out to the database Logs table via a proc (all in the migration scripts)
     */
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddNLog();
    }

    public static void ConfigureDbContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<CorgiShopDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase"), options =>
            {
                options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
        });
    }

    public static void ConfigureCacheServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = builder.Configuration.GetConnectionString("Redis"); });
        builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));
    }

    public static void ConfigureAutoMapper(this WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(typeof(GetProductsQuery).Assembly);
    }

    public static void ConfigureMediatrServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(typeof(Program).Assembly);
        builder.Services.AddMediatR(typeof(GetProductsQuery).Assembly);
    }

    public static void ConfigureCorgiShopServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        builder.Services.AddScoped<ICachingService, CachingService>();

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.Decorate<IProductRepository, CachedProductRepository>();

        builder.Services.AddTransient<IProductDataGenService, ProductDataGenService>();
    }

    public static void ConfigureWebServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CorgiShop API",
                Version = "v1",
                Description = "A fantastically simple, utterly fake, and undeniably CORGI online shopping API used for learning, testing design paradigms, and general professional development"
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        //OpenId Connect Auth via Auth0 (for demo)
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = builder.Configuration["OpenIdConnect:Authority"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                    ValidAudience = builder.Configuration["OpenIdConnect:Audience"],
                    ValidIssuer = builder.Configuration["OpenIdConnect:Authority"]
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "read:weather", //not used, kept in place for future reference once I start adding scopes/auth into a controller or two
                policy => policy.Requirements.Add(
                  new HasScopeRequirement("read:weather", builder.Configuration["OpenIdConnect:Authority"]!)
                )
              );
        });

        builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
    }
}
