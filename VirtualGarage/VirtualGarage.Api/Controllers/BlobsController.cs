using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualGarage.Domain.Services.Interfaces;

namespace VirtualGarage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BlobsController(IBlobStorage blobStorage) : ControllerBase
    {
        /// <summary>
        /// List all blobs in storage
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ListBlobs([FromQuery] string? search = null)
        {
            try
            {
                var blobs = await blobStorage.ListAllAsync();

                // Filter by search term if provided
                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchLower = search.ToLower();
                    blobs = blobs.Where(b => b.Name.ToLower().Contains(searchLower)).ToList();
                }

                return Ok(blobs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to list blobs", details = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific blob by name
        /// </summary>
        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetBlob(string fileName)
        {
            try
            {
                if (!await blobStorage.ExistsAsync(fileName))
                    return NotFound(new { error = "Blob not found" });

                var bytes = await blobStorage.DownloadAsync(fileName);
                return File(bytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to download blob", details = ex.Message });
            }
        }
    }
}
