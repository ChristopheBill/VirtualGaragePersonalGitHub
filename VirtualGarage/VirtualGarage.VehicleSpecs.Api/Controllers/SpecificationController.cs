using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualGarage.VehicleSpecs.Api.Contracts.RequestContracts;
using VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;
using VirtualGarage.VehicleSpecs.Domain.Services;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using System;

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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSpecsAsync(
            [FromQuery] string brand,
            [FromQuery] string model,
            [FromQuery] int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var specs = await _specsService.GetOrFetchAsync(brand, model, year);
                return Ok(specs);
            }
            catch (InvalidOperationException ex)
            {
                // Translate domain/infrastructure "not found" into HTTP 404 with details
                return NotFound(new ProblemDetails
                {
                    Title = "Specifications not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
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