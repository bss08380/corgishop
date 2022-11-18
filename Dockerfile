FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY . .
RUN dotnet restore

WORKDIR /app/CorgiShop.Api
RUN dotnet publish -c Debug /p:DefineConstants="LocalDocker" -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime-env
EXPOSE 4040
ENV ASPNETCORE_URLS=http://*:4040
WORKDIR /app

COPY --from=build-env /app/CorgiShop.Api/out ./
ENTRYPOINT ["dotnet", "CorgiShop.Api.dll", "--environment=LocalDocker"]