using Microsoft.Extensions.Options;
using Microsoft.Azure.Cosmos;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;
using VirtualGarage.VehicleSpecs.Persistence.Interfaces;
using VirtualGarage.VehicleSpecs.Persistence.Repositories;
using VirtualGarage.VehicleSpecs.Domain.Services;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Key Vault
try
{
    var keyVaultUrl = new Uri("https://virtualgarage-keyvault.vault.azure.net/");
    builder.Configuration.AddAzureKeyVault(
        keyVaultUrl,
        new DefaultAzureCredential(),
        new AzureKeyVaultConfigurationOptions
        {
            ReloadInterval = TimeSpan.FromHours(1)
        });
}
catch (Exception ex)
{
    throw new InvalidOperationException("Failed to load Key Vault configuration", ex);
}

// Bind CarApi settings from Key Vault (hyphenated secrets)
var carApiBaseUrl = builder.Configuration["CarApi-BaseUrl"];
var carApiJwtToken = builder.Configuration["CarApi-JwtToken"];

if (string.IsNullOrWhiteSpace(carApiBaseUrl))
{
    throw new InvalidOperationException("CarApi-BaseUrl not found in configuration (Key Vault).");
}

if (string.IsNullOrWhiteSpace(carApiJwtToken))
{
    throw new InvalidOperationException("CarApi-JwtToken not found in configuration (Key Vault).");
}

builder.Services.Configure<CarApiSettings>(opts =>
{
    opts.BaseUrl = carApiBaseUrl;
    opts.JwtToken = carApiJwtToken;
});

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

// Cosmos DB client registration (prefer single connection string)
var cosmosConn = builder.Configuration["CosmosDb-ConnectionString"] ?? builder.Configuration["Cosmos:ConnectionString"];
if (!string.IsNullOrWhiteSpace(cosmosConn))
{
    builder.Services.AddSingleton(_ => new CosmosClient(cosmosConn));
}
else
{
    var endpoint = builder.Configuration["CosmosDb-AccountEndpoint"] ?? builder.Configuration["Cosmos:AccountEndpoint"];
    var key = builder.Configuration["CosmosDb-AccountKey"] ?? builder.Configuration["Cosmos:AccountKey"];

    if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(key))
    {
        throw new InvalidOperationException("Cosmos configuration missing. Set 'CosmosDb-ConnectionString' (preferred) or both 'CosmosDb-AccountEndpoint' and 'CosmosDb-AccountKey' in Key Vault.");
    }

    builder.Services.AddSingleton(_ => new CosmosClient(endpoint, key));
}

// Register repository (creates DB/container if needed)
builder.Services.AddSingleton<IVehicleSpecsRepository>(sp =>
{
    var client = sp.GetRequiredService<CosmosClient>();
    var cfg = sp.GetRequiredService<IConfiguration>();

    // Prefer Key Vault hyphenated secrets; fall back to defaults
    var dbId = cfg["CosmosDb-DatabaseName"] ?? "VirtualGarage";
    var containerId = cfg["CosmosDb-ContainerName"] ?? "VehicleSpecs";

    return new VehicleSpecsRepository(client, dbId, containerId);
});

// CORS for frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://christophebilliet.be")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Setup authentication/authorization
var identityAuthority = builder.Configuration["IdentityServer:Authority"]
                       ?? "https://virtualgarage-identityserver.azurewebsites.net";

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = identityAuthority;
        options.TokenValidationParameters.ValidateAudience = false;
    });
                
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("VehicleSpecsApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "vehiclespecs.api");
    });
});


var app = builder.Build();

    app.MapOpenApi();
    app.MapScalarApiReference(); 

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseCors("DefaultCors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();