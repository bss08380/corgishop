# Corgi Shop Demo/Learning Project!

## Project Setup

### Basics

Grab the following:
- Visual Studio 2022 w/ .NET 7
- Docker Desktop
- Git (and a bash solution, Gitbash works in Windows)
- OpenID/Oauth provider (I use Auth0 by Okta)
- A love of corgis

### Create development HTTPS certs

Run the powershell command: ```.\Setup\config_dev_certs.ps1```

This creates a dev cert for use with ASP.NET Core and places it in your userprofile/.aspnet/https directory. This pfx cert file is then used during the docker process later down the line to provide the docker container with an HTTPS cert to use.

### Docker

Run the following command from the root project directory: ```docker compose up -d --build```

This sets up all of the various containers, builds the asp.net project, etc. The --build switch will ensure that the asp.net project is always rebuilt during docker deployment.

Current docker containers consist of:

- MS SQL Server 2022 (latest) w/ CorgiShop database added via script (see Dockerfile in /SQL)
- Redgate Flyway for db migrations (/SQL/FlywayMigrations/*) for migration scripts
- Redis Cache
- CorgiShop API (build from asp.net source)

They all reside within a single bridge network with manually assigned IP addresses. The CorgiShop API appconfig references these IPs (for the LocalDocker environment) so they must remain static.

The SQL Server and Redis instances can be accessed via localhost ports as configured in docker-compose.yml.

To turn it all off and drop the containers in docker, run this:
```
docker compose down
```

## Blazor Hosting and Environment Handling

MS has gone with a strange/new approach when it comes to environments with the .NET 7 API hosting model for Blazor WASM apps. In the past, I was used to creating a miriad of environment configurations (say, dev, qa, uat, uat2, local, prod, etc.) to support various deployment and troubleshooting/testing scenarios. This typically just meant different connection strings for various systems. This worked fine and still does for a typical ASP.NET Core app.

In contrast, when hosting Blazor/WASM, it would appear that one is limited to "Development" and "Production" environments, otherwise the WASM app is no longer served up correctly and you're hit with 404s. I haven't quite worked out how BEST to work around this change, but I have come up with a band-aid solution: the "DeploymentConfig." I added an extention method for WebApplicationBuilder to add JSON config files that follow a simple naming scheme: `deploy.{Environment}.json`, where `{Environment}` is an enumerated value that defaults to "Local" but can be configured out by using build constants defined during publish. The logic is located in CorgiShop.Api/Infrastructure/DeploymentConfig.cs:

```
public enum DeploymentConfigType
{
    LocalDev,
    LocalDocker
}

public static class DeploymentConfig
{
    public static DeploymentConfigType Configuration
    {
        get
        {
#if LOCALDOCKER
        return DeploymentConfigType.LocalDocker;
#endif
            return DeploymentConfigType.LocalDev;
        }
    }
}
```

Certainly not ideal - I would rather control this with the `ASPDOTNET_ENVIRONMENT` environment variable as has been done in the past, but since I am limited to "Development" and "Production" on that front, I am going to use the above solution until some better standard comes along or I decide otherwise.

## Domain and Repositories

Domain objects are located in the CorgiShop.Domain project, and that mostly consists of repositories in associated data storage code. There are a few key abstractions to note and those are in the /Abstractions folder:

```
                        IRepository<T>
                        /           \
        ICommandRepository<T>   IQueryRepository<T>
```

The Command/Query separation is not done completely here for lack of project and repository complexity (at this time). For example, the core of Product queries and commands are simple enough CRUD operations that it doesn't make sense to have separate implementation objects for query and command at that level. Thus, `IProductRepository` implements `IRepository<Product>` rather than the alternative dual implementations - though of course it is implementing both of those interfaces by implementing `Repository`.

The core implementation of IRepository is in `RepositoryBase<T>`, and this generic implementation over a `DbContext` handles most typical CRUD operations. For `IProductRepository`, this is literally ALL of them (at the time of writing this readme). The strength in this is (obviously) that a single set of unit tests can cover all CRUD operations for the entire project, *regardless of how many repositories and entities are added.*

Note that `<T>` in these cases is required to be an `IRepositoryEntity`, which is simply an object with properties of "Id" and "IsDeleted"" - thus the notion of 'soft deleting' things is baked into the repositories' base implementation.

## Caching and the Decorator Pattern

Caching is done at the application level in CorgiShop.Application. The major objects are located in the /Caching folder, notably `ICachingService`, `CachingService` and the main class that uses them, `CachedRepository`. As with the repositories, all typical caching scenarios are handled in a single base class, `CachedRepository`, but can be easily extended in derived types like `CachedProductRepository`.

Caching itself is done with a simple, injected `IDistributedCache`, which is configured to access a Redis instance (whcih is set up in the docker compose). Since accessing this particular abstraction (which is an extremely simple caching interface) may need to change in the future for more advanced use, `ICachingService` is used and a CorgiShop-specific caching API is set up to prevent any duplicate logic in the caching repositories (see below).

The caching layer makes use of the "Decorator Pattern," which, in this case, is a way of layering an implementation piecemeal upon a single abstraction/interface. For example, the `ProductRepository` class implements `IProductRepository` as one would expect, but the `CachedProductRepository` ALSO implementds `IProductRepository` - not only that, it also has an `IProductRepository` injected into its constructor. This is because `CachedProductRepository` is DECORATING `ProductRepository` - it provides a new layer of logic on top of its own implementation, but they both implement the same interface/API.

A combination of standard DI and a NuGet package called Scrutor make this a lot easier to do - otherwise I would need to have a quasi-factory which (when a service requires an `IProductRepository` first constructs the instance of `ProductRepository`, and then injects that into an instance of `CachedProductRepository`. I did indeed do this at first, saw how ugly it was, wrote my own generic abstraction methods, but then moved to Scrutor so that I could cut down on the amount of strange, hard to test code!

Why go through all of this? To make it simple to expand the number of repositories, and in this case, the number of cached repositories. The majority of transactions through the API are going to be CRUD and the generic interfaces/base classes provide a single implementation to rely on regardless of the number of different kinds of entities are introduced into the domain. More importantly, in my opinion, it DRASTICALLY cuts down on unit testing. With this implementation, a single file of tests can lock down the basic CRUD stuff and I never have to worry about testing that again.

## Troubleshooting

### CSS files aren't fetched by the WASM app after docker deployment

Fixed this by deleting /bin and /obj binaries in the Api and App projects. Something gets mixed up during publish and results in missing or mis-referenced files/objects. May need to add these folders to a .dockerignore file at some point. Until then, a `clear.ps1` script is ready to roll in the root directory.