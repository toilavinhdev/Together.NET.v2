FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy all layers -> respective folders
COPY ["Together.Server/Together.API/Together.API.csproj", "Together.Server/Together.API/"]
COPY ["Together.Server/Together.Shared/Together.Shared.csproj", "Together.Server/Together.Shared/"]
COPY ["Together.Server/Together.Domain/Together.Domain.csproj", "Together.Server/Together.Domain/"]
COPY ["Together.Server/Together.Persistence/Together.Persistence.csproj", "Together.Server/Together.Persistence/"]
COPY ["Together.Server/Together.Application/Together.Application.csproj", "Together.Server/Together.Application/"]

# restore dependencies
RUN dotnet restore "Together.Server/Together.API/Together.API.csproj"

COPY . .

# build
RUN dotnet build "Together.Server/Together.API/Together.API.csproj" -c Release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish "Together.Server/Together.API/Together.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5005
ENV ASPNETCORE_URLS http://*:5005
RUN ls -lha
ENTRYPOINT ["dotnet", "Together.API.dll", "--server.urls", "http://*:5005"]
