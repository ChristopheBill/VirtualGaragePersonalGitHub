using System.Net.Http.Headers;
using VirtualGarage.VehicleSpecs.Domain.Services;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi;
using VirtualGarage.VehicleSpecs.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Bind CarApi settings from configuration (will pick up User Secrets automatically)
builder.Services.Configure<CarApiSettings>(
    builder.Configuration.GetSection("CarApi"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<SpecsService>();
builder.Services.AddHttpClient<ICarApiClient, CarApiClient>(client =>
{
    var carApiSettings = builder.Configuration.GetSection("CarApi").Get<CarApiSettings>();
    client.BaseAddress = new Uri(carApiSettings.BaseUrl);
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", carApiSettings.JwtToken);
});
builder.Services.AddScoped<ISpecsService, SpecsService>();
builder.Services.AddScoped<ICarApiClient, CarApiClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
