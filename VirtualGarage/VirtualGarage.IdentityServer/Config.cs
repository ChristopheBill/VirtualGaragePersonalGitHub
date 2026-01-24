using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace VirtualGarage.IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("virtualgarage.api.read"),
            new ApiScope("virtualgarage.api.write")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m (postman => virtualgarage.api)
            new Client
            {
                ClientId = "m2m.postman",
                ClientName = "Postman Client for Virtual Garage API",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("postmangeheim".Sha256()) },

                AllowedScopes = { "virtualgarage.api.read", "virtualgarage.api.write" }
            },
            // m2m (postman => vehiclespecs.api)
            new Client
            {
                ClientId = "m2m.postman",
                ClientName = "Postman Client for Vehicles Specs API",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("postmangeheim".Sha256()) },

                AllowedScopes = { "virtualgarage.api.read", "virtualgarage.api.write" }
            },
            //m2m (virtualgarage.api => vehiclespecs.api)
            new Client
            {
                ClientId = "m2m.virtualgarage.api",
                ClientName = "Client for Virtual Garage API to access Vehicle Specs API",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("virtualgaragesecret".Sha256()) },

                AllowedScopes = { "virtualgarage.api.read", "virtualgarage.api.write" }
            },
            // interactive ASP.NET Core MVC web app
            new Client {
                ClientId = "react-app-client",
                ClientSecrets = {new Secret("reactapp-secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "virtualgarage.api.read",
                    "virtualgarage.api.write"
                },
                RedirectUris = { "http://localhost:5215/" }, 
                PostLogoutRedirectUris = { "http://localhost:5215/" }, 
                AllowedCorsOrigins = { "http://localhost:5215" }, 
            }
        };
}
