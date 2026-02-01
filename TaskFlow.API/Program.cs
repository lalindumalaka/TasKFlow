using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext - supports both PostgreSQL and SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Use PostgreSQL by default, but can be switched to SQL Server via configuration
var usePostgreSQL = builder.Configuration.GetValue<bool>("Database:UsePostgreSQL", true);

if (usePostgreSQL)
{
    builder.Services.AddDbContext<TaskFlowDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    builder.Services.AddDbContext<TaskFlowDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Add CORS for Blazor WASM
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:5001", "http://localhost:5000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

// Serve static files for Blazor WASM (when deployed together)
var wwwrootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
if (Directory.Exists(wwwrootPath))
{
    app.UseStaticFiles();
    
    // Fallback to index.html for client-side routing
    app.MapFallbackToFile("index.html");
}

app.UseAuthorization();
app.MapControllers();

// Health endpoint
app.MapGet("/health", async (TaskFlowDbContext dbContext) =>
{
    try
    {
        // Check if database connection is active
        var canConnect = await dbContext.Database.CanConnectAsync();
        return Results.Ok(new { status = "healthy", database = canConnect ? "connected" : "disconnected" });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            detail: ex.Message,
            statusCode: 503,
            title: "Service Unavailable"
        );
    }
})
.WithName("HealthCheck")
.WithTags("Health");

app.Run();
