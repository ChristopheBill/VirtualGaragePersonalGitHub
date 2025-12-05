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
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                )
            );

            // Add services to the container.

            builder.Services.AddControllers();

            // Add SeriLog logging

            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSerilogRequestLogging();

            app.MapControllers();

            app.Run();
        }
    }
}
