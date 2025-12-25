using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Interfaces;

using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    // Extension method to get UserId from ClaimsPrincipal - before using IdentityServer
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.Parse(id!);
    }
}

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

        [Authorize]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyVehicles()
        {
            var userId = User.GetUserId();
            var vehicles = await _vehicleService.GetVehiclesForUserAsync(userId);
            return Ok(vehicles);
        }
    }
}