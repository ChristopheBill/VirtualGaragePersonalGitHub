using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VirtualGarage.Api.Contracts.RequestContracts;
using VirtualGarage.Api.Contracts.ResponseContracts;
using VirtualGarage.Domain.Services.Interfaces;

namespace VirtualGarage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DonationsController(IDonationService donationService) : ControllerBase
    {
        /// <summary>
        /// Create a payment intent for a donation
        /// </summary>
        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                    return Unauthorized("User ID not found in token");

                var response = await donationService.CreatePaymentIntentAsync(request, userGuid);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to create payment intent", details = ex.Message });
            }
        }

        /// <summary>
        /// Confirm payment for a donation
        /// </summary>
        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                    return Unauthorized("User ID not found in token");

                var response = await donationService.ConfirmPaymentAsync(request, userGuid);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to confirm payment", details = ex.Message });
            }
        }

        /// <summary>
        /// Get all donations (admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<List<DonationRecordResponse>>> GetAll()
        {
            var donations = await donationService.GetAllDonationsAsync();
            return Ok(donations);
        }

        /// <summary>
        /// Get donations for the current user
        /// </summary>
        [HttpGet("me")]
        public async Task<ActionResult<List<DonationRecordResponse>>> GetMine()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                return Unauthorized("User ID not found in token");

            var donations = await donationService.GetDonationsForUserAsync(userGuid);
            return Ok(donations);
        }
    }
}
