using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualGarage.VehicleSpecs.Api.Contracts.RequestContracts;
using VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;
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
    public async Task<IActionResult> GetSpecsAsync(
            [FromQuery] string brand,
            [FromQuery] string model,
            [FromQuery] int year)
        {
            var specs = await _specsService.GetOrFetchAsync(brand, model, year);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(specs);
        }   
    // [HttpPost("lookup")]
    // public async Task<ActionResult<CarSpecsResponse>> LookupAsync(
    //     CarSpecsLookupRequest request)
    // {
    //     var specs = await _specsService.LookupAsync(request);
    //     return Ok(specs);
    // }
}
}