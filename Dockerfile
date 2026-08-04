# Build de la Api (multi-stage). Uso: ver docker-compose.full.yml
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CommunityReports.Domain/*.csproj CommunityReports.Domain/
COPY CommunityReports.Application/*.csproj CommunityReports.Application/
COPY CommunityReports.Infrastructure/*.csproj CommunityReports.Infrastructure/
COPY CommunityReports.Api/*.csproj CommunityReports.Api/
RUN dotnet restore CommunityReports.Api/CommunityReports.Api.csproj

COPY . .
RUN dotnet publish CommunityReports.Api/CommunityReports.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CommunityReports.Api.dll"]
