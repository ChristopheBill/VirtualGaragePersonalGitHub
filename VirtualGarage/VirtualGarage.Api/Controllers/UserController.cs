using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VirtualGarage.Domain.Services.Interfaces;
using VirtualGarage.Contracts;
using VirtualGarage.Api.Contracts;

namespace VirtualGarage.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Policy = "AdminOnly")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:guid}")]
        // [Authorize]
        public async Task<ActionResult<UserResponseContract>> GetUserByIdAsync([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseContract>> CreateUserAsync([FromBody] UserRequestContract userRequestContract)
        {
            var createdUser = await _userService.CreateUserAsync(userRequestContract);
            // return CreatedAtAction(nameof(GetUserByIdAsync),  // action name
            //                         new { id = createdUser.Id }, // route values
            //                         createdUser                 // body
            //                         );
            return new OkObjectResult(createdUser);
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponseContract?>>> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return new OkObjectResult(users);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}