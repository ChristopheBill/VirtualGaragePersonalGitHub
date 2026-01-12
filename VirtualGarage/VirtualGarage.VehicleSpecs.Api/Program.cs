using Microsoft.Extensions.Options;
using Microsoft.Azure.Cosmos;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;
using VirtualGarage.VehicleSpecs.Persistence.Interfaces;
using VirtualGarage.VehicleSpecs.Persistence.Repositories;
using VirtualGarage.VehicleSpecs.Domain.Services;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Bind CarApi settings (User Secrets / appsettings / KeyVault)
builder.Services.Configure<CarApiSettings>(
    builder.Configuration.GetSection("CarApi"));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Typed HttpClient for CarApi
builder.Services.AddHttpClient<ICarApiClient, CarApiClient>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<CarApiSettings>>().Value;
    client.BaseAddress = new Uri(settings.BaseUrl);
    // CarAPI accepts either X-Api-Key OR Bearer
    client.DefaultRequestHeaders.Add("X-Api-Key", settings.JwtToken);
});

// Domain service
builder.Services.AddScoped<ISpecsService, SpecsService>();

// Cosmos DB client registration (supports both "Cosmos" and "CosmosDb" user-secrets)
var cfgRoot = builder.Configuration;
var cosmosConn = cfgRoot["Cosmos:ConnectionString"] ?? cfgRoot["CosmosDb:ConnectionString"];
var endpoint = cfgRoot["Cosmos:AccountEndpoint"] ?? cfgRoot["CosmosDb:AccountEndpoint"];
var key = cfgRoot["Cosmos:AccountKey"] ?? cfgRoot["CosmosDb:AccountKey"];

if (!string.IsNullOrEmpty(cosmosConn))
{
    builder.Services.AddSingleton(sp => new CosmosClient(cosmosConn));
}
else
{
    if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(key))
    {
        throw new InvalidOperationException("Cosmos configuration is missing. Set 'Cosmos:ConnectionString' or both 'Cosmos:AccountEndpoint' and 'Cosmos:AccountKey' (use dotnet user-secrets for local development). Or provide the same values under the 'CosmosDb' section in user-secrets.");
    }
    builder.Services.AddSingleton(sp => new CosmosClient(endpoint, key));
}

// Register repository (creates DB/container if needed)
builder.Services.AddSingleton<IVehicleSpecsRepository>(sp =>
{
    var client = sp.GetRequiredService<CosmosClient>();
    var cfg = sp.GetRequiredService<IConfiguration>();
    var dbId = cfg["Cosmos:DatabaseId"] ?? cfg["CosmosDb:DatabaseName"] ?? cfg["Cosmos:DatabaseName"] ?? "VirtualGarage";
    var containerId = cfg["Cosmos:ContainerId"] ?? cfg["CosmosDb:ContainerName"] ?? cfg["Cosmos:ContainerName"] ?? "VehicleSpecs";
    return new VehicleSpecsRepository(client, dbId, containerId);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); 
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();