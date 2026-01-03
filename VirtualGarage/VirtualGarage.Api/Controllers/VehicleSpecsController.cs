using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VirtualGarage.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleSpecsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetVehicleSpecs(
            [FromQuery] string brand,
            [FromQuery] string model,
            [FromQuery] int year)
        {
            // Implementation goes here
            return Ok();
        }
    }
}
