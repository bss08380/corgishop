using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.WebAssembly.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using CorgiShop.Domain.Model;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CorgiShop.Application.Requests.Products;
using CorgiShop.Application;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using CorgiShop.Api.Infrastructure;
using CorgiShop.Common.Ioc;
using CorgiShop.Domain;
using CorgiShop.DataGen;
using NLog.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace CorgiShop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            builder.AddDeploymentConfiguration();

            /*
             * NLog is used for logging in this project
             * See the nlog.config XML file for details
             * 
             * Currently logs are written out to the database Logs table via a proc (all in the migration scripts)
             */
            builder.Logging.ClearProviders();
            builder.Logging.AddNLog();

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
            //See Auth.txt in root directory for info
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
                    "read:weather",
                    policy => policy.Requirements.Add(
                      new HasScopeRequirement("read:weather", builder.Configuration["OpenIdConnect:Authority"]!)
                    )
                  );
            });

            builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            builder.Services.AddDbContext<CorgiShopDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase"), options =>
                {
                    options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            RegisterLibServices(builder.Services);
            builder.Services.AddMediatR(typeof(Program).Assembly);
            builder.Services.AddMediatR(typeof(ApplicationIocHelper).Assembly);

            var app = builder.Build();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }

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

        private static void RegisterLibServices(IServiceCollection serviceCollection)
        {
            var iocHelpers = new List<IIocHelper>
            {
                new ApplicationIocHelper(),
                new DomainIocHelper(),
                new DataGenIocHelper()
            };

            foreach (var helper in iocHelpers)
            {
                helper?.RegisterServices(serviceCollection);
            }
        }
    }

    public class HasScopeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Scope { get; }

        public HasScopeRequirement(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }

    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');

            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}