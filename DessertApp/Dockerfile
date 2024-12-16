FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DessertApp/DessertApp.csproj", "DessertApp/"]
COPY ["DessertApp.Infraestructure/DessertApp.Infraestructure.csproj", "DessertApp.Infraestructure/"]
COPY ["DessertApp.Models/DessertApp.Models.csproj", "DessertApp.Models/"]
COPY ["DessertApp.Services/DessertApp.Services.csproj", "DessertApp.Services/"]
RUN dotnet restore "./DessertApp/DessertApp.csproj"
COPY . .
WORKDIR "/src/DessertApp"
RUN dotnet build "./DessertApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DessertApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DessertApp.dll"]
