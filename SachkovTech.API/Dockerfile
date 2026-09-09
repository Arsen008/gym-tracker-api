
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SachkovTech.API/SachkovTech.API.csproj", "SachkovTech.API/"]
COPY ["SachkovTech.Application/SachkovTech.Application.csproj", "SachkovTech.Application/"]
COPY ["SachkovTech.Domain/SachkovTech.Domain.csproj", "SachkovTech.Domain/"]
COPY ["SachkovTech.Infrastructure/SachkovTech.Infrastructure.csproj", "SachkovTech.Infrastructure/"]
RUN dotnet restore "SachkovTech.API/SachkovTech.API.csproj"
COPY . .
WORKDIR "/src/SachkovTech.API"
RUN dotnet build "./SachkovTech.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SachkovTech.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SachkovTech.API.dll"]
