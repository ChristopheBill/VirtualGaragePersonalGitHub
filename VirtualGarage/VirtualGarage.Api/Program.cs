using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Serilog;
using VirtualGarage.Persistence.DbContexts;

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

            Console.WriteLine($"Connection String: {connectionString}");
            // Add services to the container.

            builder.Services.AddControllers();

            // Add SeriLog logging

            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

            System.Console.WriteLine("SeriLog configured");

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            System.Console.WriteLine("HTTPS Redirection configured");

            app.UseAuthorization();

            System.Console.WriteLine("Authorization configured");

            app.UseSerilogRequestLogging();

            app.MapControllers();
            System.Console.WriteLine("Controllers mapped");
            app.Run();
        }
    }
}
