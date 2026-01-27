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
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Scalar.AspNetCore;
using System.Security.Claims;


namespace VirtualGarage.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Key Vault
            var keyVaultUrl = new Uri("https://virtualgarage-keyvault.vault.azure.net/");
            builder.Configuration.AddAzureKeyVault(
                keyVaultUrl,
                new DefaultAzureCredential(),
                new AzureKeyVaultConfigurationOptions
                {
                    ReloadInterval = TimeSpan.FromHours(1)
                });

            global::QuestPDF.Settings.License = global::QuestPDF.Infrastructure.LicenseType.Community;

            // EF Core - SQL
            // Connection string loaded from Key Vault secret 'virtualgarage-db-connection-string'
            string? connectionString = builder.Configuration["virtualgarage-db-connection-string"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'virtualgarage-db-connection-string' not found in Key Vault configuration.");
            }

            // Register DbContext with DI container

            builder.Services.AddDbContext<VirtualGarageDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configure options from appsettings.json

            builder.Services.Configure<VehicleSpecsApiOptions>(
                builder.Configuration.GetSection("VehicleSpecsApi"));

            // Configure BlobStorage options via Key Vault (hyphenated secrets)
            var blobConnectionString = builder.Configuration["BlobStorage-ConnectionString"];
            var blobContainerName = builder.Configuration["BlobStorage-ContainerName"];

            if (string.IsNullOrWhiteSpace(blobConnectionString))
            {
                throw new InvalidOperationException("BlobStorage-ConnectionString not found in configuration (Key Vault).");
            }

            if (string.IsNullOrWhiteSpace(blobContainerName))
            {
                throw new InvalidOperationException("BlobStorage-ContainerName not found in configuration (Key Vault).");
            }

            // Bind options
            builder.Services.Configure<BlobStorageOptions>(opts =>
            {
                opts.ConnectionString = blobConnectionString;
                opts.ContainerName = blobContainerName;
            });

            // Register BlobServiceClient with DI container
            
            builder.Services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BlobStorageOptions>>().Value;

                return new BlobServiceClient(options.ConnectionString);
            });

            // Add SeriLog logging

            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddOpenApi();

            // Setup authentication/authorization
            var identityAuthority = builder.Configuration["IdentityServer:Authority"]
                                   ?? "https://virtualgarage-identityserver.azurewebsites.net";

            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = identityAuthority;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                });
            
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            

            // Add services to the container.

            builder.Services.AddControllers();

            // Register domain services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IDonationService, DonationService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<IDonationRepository, DonationRepository>();
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
                var baseUrl = config["VehicleSpecsApi-BaseUrl"]
                             ?? "https://vehiclespecs2-api.azurewebsites.net";

                client.BaseAddress = new Uri(baseUrl);
            });
            builder.Services.AddProblemDetails();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DevCors", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:5173",
                            "https://christophebilliet.be")
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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            app.MapControllers();
            System.Console.WriteLine("Starting VirtualGarage.Api...");
            app.Run();
        }
    }
}
