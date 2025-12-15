using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VirtualGarage.Domain.Services.Interfaces;

namespace VirtualGarage.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[HttpGet("me")]
        //[Authorize]
        //public IActionResult GetCurrentUser()
        //{
        //    var user = _userService.GetCurrentUser();
        //    return new OkObjectResult(user);
        //}


    }
}