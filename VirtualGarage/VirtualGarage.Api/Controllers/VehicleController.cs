using Microsoft.AspNetCore.Mvc;
using VirtualGarage.Api.Contracts;
using VirtualGarage.Domain.Services.Interfaces;

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
}
}