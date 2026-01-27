using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualGarage.VehicleSpecs.Api.Contracts.RequestContracts;
using VirtualGarage.VehicleSpecs.Api.Contracts.ResponseContracts;
using VirtualGarage.VehicleSpecs.Domain.Services;
using VirtualGarage.VehicleSpecs.Domain.Services.Interfaces;
using System;
using Microsoft.Extensions.Options;
using VirtualGarage.VehicleSpecs.Infrastructure.CarApi;

namespace VirtualGarage.VehicleSpecs.Api.Controllers
{
    [Route("api/specifications")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        private readonly ISpecsService _specsService;
        private readonly IOptions<CarApiSettings> _carApiSettings;

        public SpecificationController(ISpecsService specsService, IOptions<CarApiSettings> carApiSettings)
        {
            _specsService = specsService;
            _carApiSettings = carApiSettings;
        }

        /// <summary>
        /// Health check endpoint to verify CarAPI connectivity and configuration
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            var settings = _carApiSettings.Value;
            var diagnostics = new
            {
                carApiConfigured = !string.IsNullOrEmpty(settings.BaseUrl),
                carApiBaseUrl = settings.BaseUrl ?? "(not set)",
                carApiTokenSet = !string.IsNullOrEmpty(settings.JwtToken),
                timestamp = DateTime.UtcNow
            };
            return Ok(diagnostics);
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
            catch (HttpRequestException ex)
            {
                return StatusCode(503, new ProblemDetails
                {
                    Title = "CarAPI unreachable",
                    Detail = ex.Message,
                    Status = StatusCodes.Status503ServiceUnavailable
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal server error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}