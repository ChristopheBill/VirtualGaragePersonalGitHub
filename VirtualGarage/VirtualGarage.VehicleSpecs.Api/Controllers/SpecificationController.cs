using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualGarage.VehicleSpecs.Domain.Services;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;

namespace VirtualGarage.VehicleSpecs.Api.Controllers
{
    [Route("api/specifications")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        private readonly ISpecService _specService;

        public SpecificationController(ISpecService specService)
        {
            _specService = specService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecs(
            [FromQuery] string brand,
            [FromQuery] string model,
            [FromQuery] int year)
        {
            var specs = await _specService.GetSpecsAsync(brand, model, year);
        return Ok(specs);
    }
    }
}
