#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AasxServerBlazor/AAS-Repository.csproj", "AasxServerBlazor/"]
COPY ["AasxServerStandardBib/AasxServerStandardBib.csproj", "AasxServerStandardBib/"]
COPY ["jsoncanonicalizer/jsoncanonicalizer.csproj", "jsoncanonicalizer/"]
COPY ["es6numberserializer/es6numberserializer.csproj", "es6numberserializer/"]
COPY ["AasxServer.DomainModelV3_0_RC02/AasCore.Aas3_0_RC02.csproj", "AasxServer.DomainModelV3_0_RC02/"]
COPY ["IO.Swagger.V1RC03/IO.Swagger.V1RC03.csproj", "IO.Swagger.V1RC03/"]
COPY ["IO.Swagger.Registry.Lib/IO.Swagger.Registry.csproj", "IO.Swagger.Registry.Lib/"]
RUN dotnet restore "AasxServerBlazor/AAS-Repository.csproj"
COPY . .
WORKDIR "/src/AasxServerBlazor"
RUN dotnet build "AAS-Repository.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AAS-Repository.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AAS-Repository.dll"]