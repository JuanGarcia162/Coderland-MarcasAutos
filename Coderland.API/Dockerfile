FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Coderland.API/Coderland.API.csproj", "Coderland.API/"]
RUN dotnet restore "Coderland.API/Coderland.API.csproj"
COPY . .
WORKDIR "/src/Coderland.API"
RUN dotnet build "Coderland.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Coderland.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Coderland.API.dll"]
