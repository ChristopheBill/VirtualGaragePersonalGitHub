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
        private readonly ISpecsService _specsService;

        public SpecificationController(ISpecsService specsService)
        {
            _specsService = specsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecs(
            [FromQuery] string brand,
            [FromQuery] string model,
            [FromQuery] int year)
        {
            var specs = await _specsService.GetSpecsAsync(brand, model, year);
        return Ok(specs);
    }
    }
}
