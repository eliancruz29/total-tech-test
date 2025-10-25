# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY AdquisicionesAPI/AdquisicionesAPI.csproj AdquisicionesAPI/
RUN dotnet restore "AdquisicionesAPI/AdquisicionesAPI.csproj"

# Copy everything else and build
COPY AdquisicionesAPI/. AdquisicionesAPI/
WORKDIR "/src/AdquisicionesAPI"
RUN dotnet build "AdquisicionesAPI.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "AdquisicionesAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 5006

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs

ENTRYPOINT ["dotnet", "AdquisicionesAPI.dll"]
