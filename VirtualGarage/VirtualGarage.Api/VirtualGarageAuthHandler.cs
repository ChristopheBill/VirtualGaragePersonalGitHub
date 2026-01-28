using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace VirtualGarage.Api;

public class VirtualGarageAuthHandler : AuthorizationHandler<ClaimOrRoleRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public VirtualGarageAuthHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ClaimOrRoleRequirement requirement)
    {
        var claims = context.User.Claims.ToList();

        // Als de app client de juiste _apiscope_ niet heeft...
        // Check in both "scope" claim (for space-separated values) and individual claim values
        var hasScope = claims.Exists(c => c.Value == requirement.Claim) ||
                       claims.Exists(c => c.Type == "scope" && c.Value.Split(' ').Contains(requirement.Claim));
        
        if (!hasScope)
        {
            // ...is het NOT OK.
            context.Fail();
        }
        // Als de app client de juiste scope wel heeft...
        else
        {
            var userId = claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            // ...en de call gebeurt door een _persoon_...
            if (userId is not null)
            {
                // ... dan vragen we meer info aan identity server...
                var client = new HttpClient();
                var identityAuthority = _configuration["IdentityServer:Authority"] ?? "https://localhost:5001";
                var disco = await client.GetDiscoveryDocumentAsync(identityAuthority);

                if (disco.IsError)
                {
                    context.Fail();
                    return;
                }

                var token = await _httpContextAccessor.HttpContext!.GetTokenAsync("access_token");

                var request = new UserInfoRequest
                {
                    Address = disco.UserInfoEndpoint,
                    Token = token,
                };

                var userInfo = await client.GetUserInfoAsync(request);

                if (userInfo.IsError)
                {
                    context.Fail();
                    return;
                }

                // ... en de persoon heeft de juiste _rol_...
                if (userInfo.Claims.ToList()
                    .Exists(c => c.Type == "role" && c.Value == requirement.Role))
                {
                    // ...dan is het OK.
                    context.Succeed(requirement);
                }
                // ... en de persoon heeft de juiste _rol_ niet
                else
                {
                    // ...dan is het NOT OK.
                    context.Fail();
                }
            }
            // ...en de call gebeurt door een _systeem_...
            else
            {
                // ...dan is het OK.
                context.Succeed(requirement);
            }
        }
    }
}
