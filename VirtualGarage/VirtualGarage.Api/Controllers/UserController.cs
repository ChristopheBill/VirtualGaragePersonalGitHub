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


    }
}