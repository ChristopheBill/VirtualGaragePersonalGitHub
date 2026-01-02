using Microsoft.Extensions.Options;
using Microsoft.Azure.Cosmos;
using VirtualGarage.VehicleSpecs.Domain.Services;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;
using VirtualGarage.VehicleSpecs.Persistence.Interfaces;
using VirtualGarage.VehicleSpecs.Persistence.Repositories;

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

// Cosmos DB client registration
var cosmosConn = builder.Configuration["Cosmos:ConnectionString"];
if (!string.IsNullOrEmpty(cosmosConn))
{
    builder.Services.AddSingleton(sp => new CosmosClient(cosmosConn));
}
else
{
    var endpoint = builder.Configuration["Cosmos:AccountEndpoint"];
    var key = builder.Configuration["Cosmos:AccountKey"];

    if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(key))
    {
        throw new InvalidOperationException("Cosmos configuration is missing. Set 'Cosmos:ConnectionString' or both 'Cosmos:AccountEndpoint' and 'Cosmos:AccountKey' (use dotnet user-secrets for local development).");
    }

    builder.Services.AddSingleton(sp => new CosmosClient(endpoint, key));
}

// Register repository (creates DB/container if needed)
builder.Services.AddSingleton<IVehicleSpecsRepository>(sp =>
{
    var client = sp.GetRequiredService<CosmosClient>();
    var cfg = sp.GetRequiredService<IConfiguration>();
    var dbId = cfg["Cosmos:DatabaseId"] ?? "VirtualGarage";
    var containerId = cfg["Cosmos:ContainerId"] ?? "VehicleSpecs";
    return new VehicleSpecsRepository(client, dbId, containerId);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();