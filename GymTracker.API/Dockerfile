
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GymTracker.API/GymTracker.API.csproj", "GymTracker.API/"]
COPY ["GymTracker.Application/GymTracker.Application.csproj", "GymTracker.Application/"]
COPY ["GymTracker.Domain/GymTracker.Domain.csproj", "GymTracker.Domain/"]
COPY ["GymTracker.Infrastructure/GymTracker.Infrastructure.csproj", "GymTracker.Infrastructure/"]
RUN dotnet restore "GymTracker.API/GymTracker.API.csproj"
COPY . .
WORKDIR "/src/GymTracker.API"
RUN dotnet build "./GymTracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GymTracker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GymTracker.API.dll"]
