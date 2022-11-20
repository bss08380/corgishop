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

Run the powershell command:
```
.\Setup\config_dev_certs.ps1
```
This creates a dev cert for use with ASP.NET Core and places it in your userprofile/.aspnet/https directory. This pfx cert file is then used during the docker process later down the line to provide the docker container with an HTTPS cert to use.

### Docker

Run the following command from the root project directory:
```
docker compose up -d --build
```
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

In contrast, when hosting Blazor/WASM, it would appear that one is limited to "Development" and "Production" environments, otherwise the WASM app is no longer served up correctly and you're hit with 404s. I haven't quite worked out how BEST to work around this change, but I have come up with a band-aid solution: the "DeploymentConfig." I added an extention method for WebApplicationBuilder to add JSON config files that follow a simple naming scheme: ```deploy.{Environment}.json```, where ```{Environment}``` is an enumerated value that defaults to "Local" but can be configured out by using build constants defined during publish. The logic is located in CorgiShop.Api/Infrastructure/DeploymentConfig.cs:

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

Certainly not ideal - I would rather control this with the ASPDOTNET_ENVIRONMENT environment variable as has been done in the past, but since I am limited to "Development" and "Production" on that front, I am going to use the above solution until some better standard comes along or I decide otherwise.

## Troubleshooting

### CSS files aren't fetched by the WASM app after docker deployment

Fixed this by deleting /bin and /obj binaries in the Api and App projects. Something gets mixed up during publish and results in missing or mis-referenced files/objects. May need to add these folders to a .dockerignore file at some point.