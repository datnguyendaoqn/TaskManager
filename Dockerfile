FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY TaskManager_api/*.csproj ./TaskManager_api/
RUN dotnet restore TaskManager_api/
COPY . .
RUN dotnet publish TaskManager_api/ -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskManager_api.dll"]