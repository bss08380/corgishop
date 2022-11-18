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
This creates a dev cert for use with ASP.NET Core and places it in your userprofile/.aspnet/https directory. This pfx cert filt is then used during the docker process later down the line to provide the docker container with an HTTPS cert to use.

### Docker

Run the following command in bash/gitbash from the root project directory:
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

## Troubleshooting

### CSS files aren't fetched by the WASM app after docker deployment

Fixed this by deleting /bin and /obj binaries in the Api and App projects. Something gets mixed up during publish and results in missing or mis-referenced files/objects. May need to add these folders to a .dockerignore file at some point.