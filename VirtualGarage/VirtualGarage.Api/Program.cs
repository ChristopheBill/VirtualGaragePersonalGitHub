using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using Serilog;
using VirtualGarage.Domain.Services;
using VirtualGarage.Domain.Services.Interfaces;
using VirtualGarage.Persistence;
using VirtualGarage.Persistence.DbContexts;
using VirtualGarage.Persistence.Interfaces;
using VirtualGarage.QuestPDF.Infrastructure;
using VirtualGarage.QuestPDF.Infrastructure.Clients;
using VirtualGarage.QuestPDF.Infrastructure.Interfaces;
using QuestPDF.Infrastructure;
using VirtualGarage.Infrastructure.Storage;
using Azure.Storage.Blobs;
using Scalar.AspNetCore;


namespace VirtualGarage.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            global::QuestPDF.Settings.License = global::QuestPDF.Infrastructure.LicenseType.Community;

            // EF Core - SQL

            string? connectionString = builder.Configuration.GetConnectionString("VirtualGarage");

            // Register DbContext with DI container

            builder.Services.AddDbContext<VirtualGarageDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configure options from appsettings.json

            builder.Services.Configure<VehicleSpecsApiOptions>(
                builder.Configuration.GetSection("VehicleSpecsApi"));

            Console.WriteLine($"Connection String: {connectionString}");

            // Configure BlobStorage options

            builder.Services.Configure<BlobStorageOptions>(
                builder.Configuration.GetSection("BlobStorage"));

            System.Console.WriteLine("BlobStorage options configured, ContainerName: " + 
                builder.Configuration.GetSection("BlobStorage:ContainerName").Value);
            
            // Register BlobServiceClient with DI container
            
            builder.Services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BlobStorageOptions>>().Value;

                return new BlobServiceClient(options.ConnectionString);
            });

            // Add SeriLog logging

            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));
            System.Console.WriteLine("SeriLog configured");

            builder.Services.AddOpenApi();

            // Setup authentication/authorization
            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.TokenValidationParameters.ValidateAudience = false;
                });
                
            builder.Services.AddAuthorization();

            

            // Add services to the container.

            builder.Services.AddControllers();

            // Register domain services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IVehicleReportService, VehicleReportService>();

            // Register infrastructure providers
            builder.Services.AddScoped<IVehicleSpecsProvider, VehicleSpecsClient>();
            builder.Services.AddScoped<IVehicleSpecsPdfGenerator, VehicleSpecsPdfGenerator>();
            builder.Services.AddScoped<IBlobStorage, AzureBlobStorage>();

            // Using HttpClient for VehicleSpecsClient
            builder.Services.AddHttpClient<IVehicleSpecsProvider, VehicleSpecsClient>(
            (sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var baseUrl = config["VehicleSpecsApi:BaseUrl"];

                client.BaseAddress = new Uri(baseUrl!);
            });
            builder.Services.AddProblemDetails();

            System.Console.WriteLine("HttpClient for VehicleSpecsClient configured, BaseUrl: " + builder.Configuration["VehicleSpecsApi:BaseUrl"]);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DevCors", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });         
            });

            //PascalCase JSON Serialization
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                  options.JsonSerializerOptions.PropertyNamingPolicy =
                  JsonNamingPolicy.CamelCase;
              });

            var app = builder.Build();

            app.UseCors("DevCors");

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            System.Console.WriteLine("App running at http://localhost:5215/scalar");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            app.MapControllers();

            System.Console.WriteLine("Application started");

            app.Run();

            System.Console.WriteLine("Application shutdown");
        }
    }
}
