using Duende.IdentityServer;
using VirtualGarage.IdentityServer.Data;
using VirtualGarage.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

namespace VirtualGarage.IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Configure Key Vault
        var keyVaultUrl = new Uri("https://virtualgarage-keyvault.vault.azure.net/");
        builder.Configuration.AddAzureKeyVault(
            keyVaultUrl,
            new DefaultAzureCredential(),
            new AzureKeyVaultConfigurationOptions
            {
                ReloadInterval = TimeSpan.FromHours(1)
            });

        builder.Services.AddRazorPages();
        builder.Services.AddControllers();
        
        // Add CORS support for React frontend
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });


        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration["identityserver-db-connection-string"]));
        
        builder.Services.AddDbContext<ConfigurationDbContext>(options =>
            options.UseSqlServer(builder.Configuration["identityserver-db-connection-string"],
            options => options.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            // .AddInMemoryIdentityResources(Config.IdentityResources)
            // .AddInMemoryApiScopes(Config.ApiScopes)
            // .AddInMemoryClients(Config.Clients)
            .AddConfigurationStore()
            .AddAspNetIdentity<ApplicationUser>();
        
        builder.Services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            });

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("AllowReactApp");
        app.UseIdentityServer();
        app.UseAuthorization();
        
        app.MapRazorPages()
            .RequireAuthorization();
        
        app.MapControllers();

        return app;
    }
}