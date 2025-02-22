
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
COPY . .
RUN dotnet restore "./src/AAS-Repository.csproj"
RUN dotnet build "./src/AAS-Repository.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./src/AAS-Repository.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AAS-Repository.dll"]