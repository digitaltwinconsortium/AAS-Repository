#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AAS-Repository.csproj", "."]
RUN dotnet restore "./AAS-Repository.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AAS-Repository.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AAS-Repository.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AAS-Repository.dll"]