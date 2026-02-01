# Stage 1: Build the API
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS api-build
WORKDIR /src

# Copy solution and project files
COPY TaskFlow.sln ./
COPY TaskFlow.API/TaskFlow.API.csproj ./TaskFlow.API/
COPY TaskFlow.Data/TaskFlow.Data.csproj ./TaskFlow.Data/
COPY TaskFlow.Shared/TaskFlow.Shared.csproj ./TaskFlow.Shared/

# Restore dependencies
RUN dotnet restore TaskFlow.sln

# Copy all source files
COPY TaskFlow.API/ ./TaskFlow.API/
COPY TaskFlow.Data/ ./TaskFlow.Data/
COPY TaskFlow.Shared/ ./TaskFlow.Shared/

# Build the API
WORKDIR /src/TaskFlow.API
RUN dotnet build -c Release -o /app/build

# Publish the API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Build the Blazor WASM
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS wasm-build
WORKDIR /src

# Copy solution and project files
COPY TaskFlow.sln ./
COPY TaskFlow.UI/TaskFlow.UI.csproj ./TaskFlow.UI/
COPY TaskFlow.Shared/TaskFlow.Shared.csproj ./TaskFlow.Shared/

# Restore dependencies
RUN dotnet restore TaskFlow.sln

# Copy all source files
COPY TaskFlow.UI/ ./TaskFlow.UI/
COPY TaskFlow.Shared/ ./TaskFlow.Shared/

# Build and publish the Blazor WASM
WORKDIR /src/TaskFlow.UI
RUN dotnet publish -c Release -o /app/publish

# Stage 3: Runtime - Serve both API and Blazor WASM
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy API from build stage
COPY --from=api-build /app/publish ./

# Copy Blazor WASM static files to API's wwwroot directory
COPY --from=wasm-build /app/publish/wwwroot ./wwwroot

# The API will serve static files from wwwroot and handle API routes
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the API (which will also serve static files if configured)
ENTRYPOINT ["dotnet", "TaskFlow.API.dll"]

