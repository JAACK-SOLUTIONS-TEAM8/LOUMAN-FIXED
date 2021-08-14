using Louman.Models.DTOs.User;
using Louman.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            var existingUser = await _authRepository.LoginAsync(user);
            if (existingUser != null)
                return Ok(new { User = existingUser, StatusCode = StatusCodes.Status200OK });
            return Ok(new { User = existingUser, StatusCode = StatusCodes.Status404NotFound });
        }


        [HttpGet("Logout/{id}")]
        public async Task<IActionResult> Logout([FromRoute] int id)
        {
            var user = await _authRepository.LogoutAsync(id);
            if (user != false)
                return Ok(new { User = user, StatusCode = StatusCodes.Status200OK });
            return Ok(new { User = user, StatusCode = StatusCodes.Status404NotFound });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] UserInfoDto user)
        {
            var response = await _authRepository.ResetPassword(user);
            if (response == true)
                return Ok(new { Response = true, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Response = false, StatusCode = StatusCodes.Status404NotFound });

        }




        [HttpGet("Forget")]
        public async Task<IActionResult> ForgetPassword([FromRoute] string email)
        {
            var response = await _authRepository.isEmailExist(email);
            if (response == true)
                return Ok(new { Response = true, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Response = false, StatusCode = StatusCodes.Status404NotFound });

        }

    }
}
