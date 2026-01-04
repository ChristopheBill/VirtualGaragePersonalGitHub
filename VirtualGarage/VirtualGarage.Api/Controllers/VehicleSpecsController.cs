using Microsoft.AspNetCore.Mvc;
using VirtualGarage.Domain.Services.Interfaces;

namespace VirtualGarage.VehicleSpecs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            var specs = await _reportService.GetRawSpecsAsync(brand, model, year);

            if (specs == null)
                return NotFound();

            return Ok(specs);
        }

        // GET api/vehiclespecs/pdf?brand=Volvo&model=V60&year=2019
        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf([FromQuery] string brand, [FromQuery] string model, [FromQuery] int year)
        {
            var pdfBytes = await _reportService.GeneratePdfAsync(brand, model, year);

            if (pdfBytes.Length == 0)
                return NotFound();

            return File(pdfBytes, "application/pdf", $"{brand}-{model}-{year}.pdf");
        }
    }
}