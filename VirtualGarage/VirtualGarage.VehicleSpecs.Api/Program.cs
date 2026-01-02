using Microsoft.Extensions.Options;
using VirtualGarage.VehicleSpecs.Domain.Services;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();