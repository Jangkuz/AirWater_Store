FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApps/AirWaterStore.Web/AirWaterStore.Web.csproj", "WebApps/AirWaterStore.Web/"]
RUN dotnet restore "WebApps/AirWaterStore.Web/AirWaterStore.Web.csproj"
COPY . .
WORKDIR "/src/WebApps/AirWaterStore.Web"
RUN dotnet build "./AirWaterStore.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AirWaterStore.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AirWaterStore.Web.dll"]
