# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UaStack/Directory.Build.props", "UaStack/"]
COPY ["src/AAS-Repository.csproj", "src/"]
COPY ["UaStack/Libraries/Opc.Ua.Client.ComplexTypes/Opc.Ua.Client.ComplexTypes.csproj", "UaStack/Libraries/Opc.Ua.Client.ComplexTypes/"]
COPY ["UaStack/Stack/Opc.Ua.Core/Opc.Ua.Core.csproj", "UaStack/Stack/Opc.Ua.Core/"]
COPY ["UaStack/Libraries/Opc.Ua.Security.Certificates/Opc.Ua.Security.Certificates.csproj", "UaStack/Libraries/Opc.Ua.Security.Certificates/"]
COPY ["UaStack/Libraries/Opc.Ua.Client/Opc.Ua.Client.csproj", "UaStack/Libraries/Opc.Ua.Client/"]
COPY ["UaStack/Libraries/Opc.Ua.Configuration/Opc.Ua.Configuration.csproj", "UaStack/Libraries/Opc.Ua.Configuration/"]
COPY ["UaStack/Libraries/Opc.Ua.Server/Opc.Ua.Server.csproj", "UaStack/Libraries/Opc.Ua.Server/"]
RUN dotnet restore "./src/AAS-Repository.csproj"
COPY . .
WORKDIR "/src/src"
RUN dotnet build "./AAS-Repository.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AAS-Repository.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AAS-Repository.dll"]