using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskFlow.UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient to point to the API
// In development, use the API's localhost URL
// In production (when served from API), use relative URLs
var apiBaseAddress = builder.Configuration["ApiBaseAddress"] 
    ?? (builder.HostEnvironment.IsDevelopment() 
        ? "https://localhost:7000"  // Default API port in development
        : builder.HostEnvironment.BaseAddress);

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(apiBaseAddress) 
});

// Register services
builder.Services.AddScoped<TaskFlow.UI.Services.TaskService>();
builder.Services.AddScoped<TaskFlow.UI.Services.TimeEntryService>();

await builder.Build().RunAsync();
