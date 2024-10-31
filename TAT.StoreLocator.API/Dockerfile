#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Use the official .NET image as a base for runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["TAT.StoreLocator.API/TAT.StoreLocator.API.csproj", "TAT.StoreLocator.API/"]
COPY ["TAT.StoreLocator.Core/TAT.StoreLocator.Core.csproj", "TAT.StoreLocator.Core/"]
COPY ["TAT.StoreLocator.Infrastructure/TAT.StoreLocator.Infrastructure.csproj", "TAT.StoreLocator.Infrastructure/"]
RUN dotnet restore "./TAT.StoreLocator.API/./TAT.StoreLocator.API.csproj"

# Copy everything else and build the app
COPY . .
WORKDIR "/src/TAT.StoreLocator.API"
RUN dotnet build "./TAT.StoreLocator.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TAT.StoreLocator.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage / runtime image
FROM base AS final
WORKDIR /app

# Copy the app and config files
COPY --from=publish /app/publish .
COPY ["TAT.StoreLocator.API/appsettings.json", "."]


# Set environment variable for logging
ENV ASPNETCORE_ENVIRONMENT=Production


# Ensure log directory exists
RUN mkdir -p /app/logs

# Start the application
ENTRYPOINT ["dotnet", "TAT.StoreLocator.API.dll"]


