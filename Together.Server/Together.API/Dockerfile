FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5015

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Together.Server/Together.API/Together.API.csproj", "Together.Server/Together.API/"]
COPY ["Together.Server/Together.Application/Together.Application.csproj", "Together.Server/Together.Application/"]
COPY ["Together.Server/Together.Persistence/Together.Persistence.csproj", "Together.Server/Together.Persistence/"]
COPY ["Together.Server/Together.Shared/Together.Shared.csproj", "Together.Server/Together.Shared/"]
COPY ["Together.Server/Together.Domain/Together.Domain.csproj", "Together.Server/Together.Domain/"]
RUN dotnet restore "Together.Server/Together.API/Together.API.csproj"

COPY . .

WORKDIR "/src/Together.Server/Together.API"
RUN dotnet build "Together.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Together.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://*:5015
ENTRYPOINT ["dotnet", "Together.API.dll", "--urls", "http://*:5015"]

# /
# |-- /app
# |   |-- /build
# |   |-- /publish
# |-- /src
# |   |-- /Together.Server
# |       |-- /Together.API
# |       |-- /Together.Application
# |       |-- /Together.Persistence
# |       |-- /Together.Shared
# |       |-- /Together.Domain