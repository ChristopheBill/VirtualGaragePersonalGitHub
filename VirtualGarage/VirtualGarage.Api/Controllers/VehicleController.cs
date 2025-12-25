using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Interfaces;

public static class HttpContextExtensions
{
    //temp method to get userId from header for testing before IdentityServer is setup
    public static Guid GetDebugUserId(this HttpContext context)
    {
        var userIdHeader = context.Request.Headers["X-Debug-UserId"].ToString();
        if (string.IsNullOrEmpty(userIdHeader) || !Guid.TryParse(userIdHeader, out var userId))
        {
            throw new InvalidOperationException("X-Debug-UserId header is missing or invalid.");
        }
        return userId;
    }
}


// using System.Security.Claims;

// public static class ClaimsPrincipalExtensions
// {
//     // Extension method to get UserId from ClaimsPrincipal - before using IdentityServer
//     public static Guid GetUserId(this ClaimsPrincipal user)
//     {
//         var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
//         return Guid.Parse(id!);
//     }
// }

namespace VirtualGarage.Api.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

       [HttpGet("{id:guid}")]
    //    [Route("index")]
       public async Task<IActionResult> GetCarByIdAsync([FromRoute] Guid id)
        {
            var vehicle =  await _vehicleService.GetVehicleAsync(id);
            if (vehicle == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(vehicle);
        }
        [HttpPost]
        public async Task<IActionResult> CreateVehicleAsync([FromBody] VehicleRequestContract vehicle)
        {
            var createdVehicle = await _vehicleService.CreateVehicleAsync(vehicle);
            // return new CreatedAtActionResult(nameof(GetCarByIdAsync), "Vehicle", new { id = createdVehicle.Id }, createdVehicle);
            return new OkObjectResult(createdVehicle);
        }

        // [Authorize]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyVehicles()
        {
            var userId = HttpContext.GetDebugUserId();
            var vehicles = await _vehicleService.GetVehiclesForUserAsync(userId);
            return Ok(vehicles);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateVehicle(
         Guid id,
         [FromBody] VehicleRequestContract contract)
        {
           var updated = await _vehicleService.UpdateVehicleAsync(id, contract);
           return Ok(updated);
      }   

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteVehicle(Guid id)
        {
            await _vehicleService.DeleteVehicleAsync(id);
            return NoContent();
        }
    }
}