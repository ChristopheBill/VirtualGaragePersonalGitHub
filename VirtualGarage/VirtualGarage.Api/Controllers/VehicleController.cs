using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Interfaces;
using VirtualGarage.Persistence.Interfaces;
using VirtualGarage.Persistence.Entities;
using VirtualGarage.Shared;

namespace VirtualGarage.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/vehicles")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IUserRepository _userRepository;

        public VehicleController(IVehicleService vehicleService, IUserRepository userRepository)
        {
            _vehicleService = vehicleService;
            _userRepository = userRepository;
        }

        private Guid? GetUserIdFromClaims()
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(sub, out var userId))
            {
                return userId;
            }
            return null;
        }

        private async Task EnsureUserExists(Guid userId)
        {
            var existing = await _userRepository.GetUserByIdAsync(userId);
            if (existing != null)
            {
                return;
            }

            var placeholder = new User
            {
                Id = userId,
                FirstName = User.FindFirstValue(ClaimTypes.GivenName) ?? "Unknown",
                LastName = User.FindFirstValue(ClaimTypes.Surname) ?? User.FindFirstValue(ClaimTypes.Name) ?? "User",
                Email = User.FindFirstValue(ClaimTypes.Email) ?? $"{userId}@placeholder.local",
                BirthDay = DateTime.UtcNow,
                UserRole = RoleEnum.User
            };

            await _userRepository.CreateUserAsync(placeholder);
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
            var userId = GetUserIdFromClaims();
            if (userId is null)
            {
                return Unauthorized();
            }
            await EnsureUserExists(userId.Value);
            var created = await _vehicleService.CreateVehicleAsync(vehicle, userId.Value);
            return Ok(created);
        }

        // [Authorize]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyVehicles()
        {
            var userId = GetUserIdFromClaims();
            if (userId is null)
            {
                return Unauthorized();
            }
            await EnsureUserExists(userId.Value);
            var vehicles = await _vehicleService.GetVehiclesForUserAsync(userId.Value);
            return Ok(vehicles);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] VehicleRequestContract contract)
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