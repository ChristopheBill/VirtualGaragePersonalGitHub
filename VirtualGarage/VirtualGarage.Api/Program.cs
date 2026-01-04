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

namespace VirtualGarage.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

               // EF Core - MySQL
            string? connectionString = builder.Configuration.GetConnectionString("VirtualGarage");

            builder.Services.AddDbContext<VirtualGarageDbContext>(options =>
                options.UseSqlServer(connectionString)
            );
            builder.Services.Configure<VehicleSpecsApiOptions>(
                builder.Configuration.GetSection("VehicleSpecsApi"));

            Console.WriteLine($"Connection String: {connectionString}");
            // Add services to the container.

            builder.Services.AddControllers();

            // Add SeriLog logging

            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

            System.Console.WriteLine("SeriLog configured");

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVehicleService, VehicleService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

            // Register VehicleSpecsClient
            builder.Services.AddHttpClient<IVehicleSpecsProvider, VehicleSpecsClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<VehicleSpecsApiOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });
            System.Console.WriteLine("VehicleSpecsClient configured");
            System.Console.WriteLine("Base URL: " + builder.Configuration.GetSection("VehicleSpecsApi")["BaseUrl"]);

            // Register PDF generator
            builder.Services.AddSingleton<IVehicleSpecsPdfGenerator, VehicleSpecsPdfGenerator>();

            // Register the report service
            builder.Services.AddScoped<VehicleReportService>();


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

            app.UseHttpsRedirection();

            System.Console.WriteLine("HTTPS Redirection configured");

            app.UseAuthorization();

            System.Console.WriteLine("Authorization configured");

            app.UseSerilogRequestLogging();

            app.MapControllers();

            System.Console.WriteLine("Controllers mapped");

            app.Run();

            System.Console.WriteLine("Application shutdown");
        }
    }
}
