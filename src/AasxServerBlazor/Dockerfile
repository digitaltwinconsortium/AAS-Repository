#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AasxServerBlazor/AasxServerBlazor.csproj", "AasxServerBlazor/"]
COPY ["AasxServerStandardBib/AasxServerStandardBib.csproj", "AasxServerStandardBib/"]
RUN dotnet restore "AasxServerBlazor/AasxServerBlazor.csproj"
COPY . .
WORKDIR "/src/AasxServerBlazor"
RUN dotnet build "AasxServerBlazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AasxServerBlazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AasxServerBlazor.dll"]