using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VirtualGarage.VehicleSpecs.Api.Controllers
{
    [Route("api/specifications")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        [HttpGet("{vehicleId:guid}")]
        public IActionResult GetSpecificationsByVehicleId([FromRoute] Guid vehicleId)
        {
            // Placeholder implementation
            return Ok(new { VehicleId = vehicleId, Specifications = "Sample Specifications" });
        }
    }
}
