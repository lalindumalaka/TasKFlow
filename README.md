# TaskFlow Analytics

A Blazor WebAssembly + EF Core application for tracking tasks and time spent, built specifically for practicing CI/CD pipelines with GitHub Actions and Azure DevOps.

## Project Structure

The solution follows a "Clean Architecture Lite" pattern with four projects:

- **TaskFlow.UI** - Blazor WebAssembly frontend
- **TaskFlow.API** - ASP.NET Core Web API backend
- **TaskFlow.Data** - EF Core data access layer with PostgreSQL/SQL Server support
- **TaskFlow.Shared** - Shared DTOs and entities

## Data Schema

### Entities

- **TaskItem**: `Id`, `Title`, `Description`, `StatusId`, `CreatedAt`
- **TimeEntry**: `Id`, `TaskItemId`, `StartTime`, `EndTime`, `Duration`
- **Status**: `Id`, `Name` (seeded with: "To Do", "In Progress", "Review", "Completed")

## API Endpoints

- `GET /health` - Health check endpoint that verifies database connectivity
- `GET /api/tasks` - Get all tasks
- `GET /api/tasks/{id}` - Get a specific task
- `POST /api/tasks` - Create a new task
- `PUT /api/tasks/{id}` - Update a task
- `DELETE /api/tasks/{id}` - Delete a task

## Database Configuration

The application supports both PostgreSQL and SQL Server. Configure the database provider in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=TaskFlowDb;Username=postgres;Password=postgres"
  },
  "Database": {
    "UsePostgreSQL": true
  }
}
```

Set `UsePostgreSQL` to `false` to use SQL Server instead.

## Running the Application

### Development

1. Ensure you have a database running (PostgreSQL or SQL Server)
2. Update the connection string in `TaskFlow.API/appsettings.json`
3. Create the initial migration:
   ```bash
   dotnet ef migrations add InitialCreate --project TaskFlow.Data --startup-project TaskFlow.API --context TaskFlowDbContext
   ```
4. Apply the migration:
   ```bash
   dotnet ef database update --project TaskFlow.Data --startup-project TaskFlow.API --context TaskFlowDbContext
   ```
5. Run the API:
   ```bash
   cd TaskFlow.API
   dotnet run
   ```
6. Run the Blazor UI (in a separate terminal):
   ```bash
   cd TaskFlow.UI
   dotnet run
   ```

### Docker

Build and run with Docker:

```bash
docker build -t taskflow-analytics .
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="your-connection-string" taskflow-analytics
```

## CI/CD

The project includes a GitHub Actions workflow (`.github/workflows/main.yml`) that:

1. Restores and builds the solution
2. Runs tests (if any exist)
3. Generates an idempotent EF Core migration script

The migration script is uploaded as an artifact for deployment.

## Technologies

- .NET 9.0
- Blazor WebAssembly
- ASP.NET Core Web API
- Entity Framework Core 9.0
- PostgreSQL / SQL Server
- Docker (multi-stage build)

