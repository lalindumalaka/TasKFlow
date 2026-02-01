# TaskFlow Analytics - Setup Commands

This document contains the terminal commands used to scaffold the TaskFlow Analytics solution.

## Solution and Project Creation

```bash
# Create the solution
dotnet new sln -n TaskFlow

# Create the four projects
dotnet new classlib -n TaskFlow.Shared -o TaskFlow.Shared
dotnet new classlib -n TaskFlow.Data -o TaskFlow.Data
dotnet new webapi -n TaskFlow.API -o TaskFlow.API
dotnet new blazorwasm -n TaskFlow.UI -o TaskFlow.UI

# Add all projects to the solution
dotnet sln add TaskFlow.Shared/TaskFlow.Shared.csproj TaskFlow.Data/TaskFlow.Data.csproj TaskFlow.API/TaskFlow.API.csproj TaskFlow.UI/TaskFlow.UI.csproj

# Set up project references
dotnet add TaskFlow.Data/TaskFlow.Data.csproj reference TaskFlow.Shared/TaskFlow.Shared.csproj
dotnet add TaskFlow.API/TaskFlow.API.csproj reference TaskFlow.Data/TaskFlow.Data.csproj TaskFlow.Shared/TaskFlow.Shared.csproj
dotnet add TaskFlow.UI/TaskFlow.UI.csproj reference TaskFlow.Shared/TaskFlow.Shared.csproj
```

## NuGet Package Installation

```bash
# Add EF Core packages to TaskFlow.Data
dotnet add TaskFlow.Data/TaskFlow.Data.csproj package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add TaskFlow.Data/TaskFlow.Data.csproj package Microsoft.EntityFrameworkCore.Design --version 9.0.0
dotnet add TaskFlow.Data/TaskFlow.Data.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.0
dotnet add TaskFlow.Data/TaskFlow.Data.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.0

# Add EF Core tools to TaskFlow.API (for migrations)
dotnet add TaskFlow.API/TaskFlow.API.csproj package Microsoft.EntityFrameworkCore.Design --version 9.0.0
dotnet add TaskFlow.API/TaskFlow.API.csproj package Microsoft.EntityFrameworkCore.Tools --version 9.0.0

# Add Swagger for API documentation
dotnet add TaskFlow.API/TaskFlow.API.csproj package Swashbuckle.AspNetCore
```

## Next Steps

After running these commands, the following code files were created:

1. **Entities** in `TaskFlow.Shared/Entities/`:
   - `Status.cs`
   - `TaskItem.cs`
   - `TimeEntry.cs`

2. **DbContext** in `TaskFlow.Data/`:
   - `TaskFlowDbContext.cs` (with Fluent API configuration and data seeding)

3. **API** in `TaskFlow.API/`:
   - `Controllers/TasksController.cs`
   - Updated `Program.cs` (with DbContext configuration and health endpoint)

4. **CI/CD Files**:
   - `Dockerfile` (multi-stage build)
   - `.dockerignore`
   - `.github/workflows/main.yml` (GitHub Actions workflow)

## Creating the Initial Migration

To create the initial EF Core migration, run:

```bash
dotnet ef migrations add InitialCreate --project TaskFlow.Data --startup-project TaskFlow.API --context TaskFlowDbContext
```

## Building and Running

```bash
# Build the solution
dotnet build TaskFlow.sln

# Run the API
cd TaskFlow.API
dotnet run

# Run the Blazor UI (in a separate terminal)
cd TaskFlow.UI
dotnet run
```

