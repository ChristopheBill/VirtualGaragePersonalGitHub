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
            new ApiScope("virtualgarage.vehiclespecs.api"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m (postman => virtualgarage.vehiclespecs.api)
            new Client
            {
                ClientId = "m2m.postman",
                ClientName = "Postman Client for Virtual Garage Vehicle Specs API",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("postmangeheim".Sha256()) },

                AllowedScopes = { "virtualgarage.vehiclespecs.api" }
            },
        };
}
