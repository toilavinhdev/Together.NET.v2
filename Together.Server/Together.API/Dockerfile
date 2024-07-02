FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Together.Server/Together.API/Together.API.csproj", "Together.Server/Together.API/"]
COPY ["Together.Server/Together.Domain/Together.Domain.csproj", "Together.Server/Together.Domain/"]
COPY ["Together.Server/Together.Application/Together.Application.csproj", "Together.Server/Together.Application/"]
COPY ["Together.Server/Together.Persistence/Together.Persistence.csproj", "Together.Server/Together.Persistence/"]
COPY ["Together.Server/Together.Shared/Together.Shared.csproj", "Together.Server/Together.Shared/"]
RUN dotnet restore "Together.Server/Together.API/Together.API.csproj"
COPY . .
WORKDIR "/src/Together.Server/Together.API"
RUN dotnet build "Together.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Together.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Together.API.dll"]
