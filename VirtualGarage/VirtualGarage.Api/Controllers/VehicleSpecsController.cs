using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualGarage.Domain.Services.Interfaces;

namespace VirtualGarage.VehicleSpecs.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/vehiclespecs")]
    public class VehicleSpecsController : ControllerBase
    {
        private readonly IVehicleReportService _reportService;

        public VehicleSpecsController(IVehicleReportService reportService)
        {
            _reportService = reportService;
        }

        // GET api/vehiclespecs?brand=Volvo&model=V60&year=2019
        [HttpGet]
        public async Task<IActionResult> GetSpecsJson([FromQuery] string brand, [FromQuery] string model, [FromQuery] int year)
        {
            try
            {
                var specs = await _reportService.GetRawSpecsAsync(brand, model, year);

                if (specs == null)
                    return NotFound();

                return Ok(specs);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Specifications not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }

        // GET api/vehiclespecs/pdf?brand=Volvo&model=V60&year=2019
        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf([FromQuery] string brand, [FromQuery] string model, [FromQuery] int year)
        {
            try
            {
                var pdfBytes = await _reportService.GetOrCreatePdfAsync(brand, model, year);

                if (pdfBytes.Length == 0)
                    return NotFound();

                return File(pdfBytes, "application/pdf", $"{brand}-{model}-{year}.pdf");
            }
            catch (ApplicationException ex)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Specifications not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid request",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
        }
    }
}