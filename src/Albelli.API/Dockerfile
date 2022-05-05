#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Albelli.API/Albelli.API.csproj", "src/Albelli.API/"]
COPY ["src/Albelli.Core/Albelli.Core.csproj", "src/Albelli.Core/"]
RUN dotnet restore "src/Albelli.API/Albelli.API.csproj"
COPY . .
WORKDIR "/src/src/Albelli.API"
RUN dotnet build "Albelli.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Albelli.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Albelli.API.dll"]