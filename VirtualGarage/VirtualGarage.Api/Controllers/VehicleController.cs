using Microsoft.AspNetCore.Mvc;

namespace VirtualGarage.Api.Controllers
{
    [ApiController]
    [Route("api/vehicle")]
    public class VehicleController : ControllerBase
    {
       [HttpGet]
       [Route("index")]
       public Task<IActionResult> Index()
       {
           return Task.FromResult<IActionResult>(Ok("Vehicle Index"));
       }
}
}