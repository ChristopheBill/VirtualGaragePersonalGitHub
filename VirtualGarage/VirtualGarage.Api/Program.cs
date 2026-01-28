using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace VirtualGarage.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Key Vault (only in Production or when UseKeyVault is true)
            var useKeyVault = builder.Configuration.GetValue<bool>("UseKeyVault");
            if (builder.Environment.IsProduction() || useKeyVault)
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

            global::QuestPDF.Settings.License = global::QuestPDF.Infrastructure.LicenseType.Community;

            // EF Core - SQL
            // Connection string loaded from Key Vault or appsettings
            string? connectionString = builder.Configuration["virtualgarage-db-connection-string"] 
                ?? builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string not found. Add 'ConnectionStrings:DefaultConnection' to appsettings or 'virtualgarage-db-connection-string' to Key Vault.");
            }

            // Register DbContext with DI container

            builder.Services.AddDbContext<VirtualGarageDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configure options from appsettings.json

            builder.Services.Configure<VehicleSpecsApiOptions>(
                builder.Configuration.GetSection("VehicleSpecsApi"));

            // Configure BlobStorage options via Key Vault or appsettings
            var blobConnectionString = builder.Configuration["BlobStorage-ConnectionString"]
                ?? builder.Configuration["BlobStorage:ConnectionString"];
            var blobContainerName = builder.Configuration["BlobStorage-ContainerName"]
                ?? builder.Configuration["BlobStorage:ContainerName"];

            if (string.IsNullOrWhiteSpace(blobConnectionString))
            {
                throw new InvalidOperationException("BlobStorage ConnectionString not found. Add to appsettings or Key Vault.");
            }

            if (string.IsNullOrWhiteSpace(blobContainerName))
            {
                throw new InvalidOperationException("BlobStorage ContainerName not found. Add to appsettings or Key Vault.");
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
            var identityAuthority = builder.Configuration["IdentityServer:Authority"];

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = identityAuthority;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.RoleClaimType = "role";
                    // Save the token so we can access it in the auth handler
                    options.SaveToken = true;
                });
            
            // Register HttpContextAccessor and custom auth handler
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IAuthorizationHandler, VirtualGarageAuthHandler>();
            
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                
                options.AddPolicy("VehicleReadPolicy", policy =>
                    policy.Requirements.Add(
                        new ClaimOrRoleRequirement(
                            "virtualgarage.api.read",
                            "User")));
                
                options.AddPolicy("VehicleWritePolicy", policy =>
                    policy.Requirements.Add(
                        new ClaimOrRoleRequirement(
                            "virtualgarage.api.write",
                            "User")));
                
                options.AddPolicy("AdminReadPolicy", policy =>
                    policy.Requirements.Add(
                        new ClaimOrRoleRequirement(
                            "virtualgarage.api.admin",
                            "Admin")));
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
            // builder.Services.AddScoped<IVehicleSpecsProvider, VehicleSpecsClient>();
            builder.Services.AddScoped<IVehicleSpecsPdfGenerator, VehicleSpecsPdfGenerator>();
            builder.Services.AddScoped<IBlobStorage, AzureBlobStorage>();

            // Using HttpClient for VehicleSpecsClient
            builder.Services.AddHttpClient<IVehicleSpecsProvider, VehicleSpecsClient>(
            (sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var baseUrl = config["VehicleSpecsApi-BaseUrl"];
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    client.BaseAddress = new Uri(baseUrl);
                }
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
                        .AllowAnyMethod()
                        .AllowCredentials();
                });         
            });

            //PascalCase JSON Serialization - Configure the already-added controllers from earlier
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
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
