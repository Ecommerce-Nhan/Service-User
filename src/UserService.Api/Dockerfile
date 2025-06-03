FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8082
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Development
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_USERNAME
ARG GITHUB_TOKEN

WORKDIR /src
COPY src/ .

RUN dotnet nuget add source \
    --username $GITHUB_USERNAME \
    --password $GITHUB_TOKEN \
    --store-password-in-clear-text \
    --name github "https://nuget.pkg.github.com/nhanne/index.json"

RUN dotnet restore "UserService.Api/UserService.Api.csproj"
WORKDIR "/src/UserService.Api"
RUN dotnet build "./UserService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.Api.dll"]