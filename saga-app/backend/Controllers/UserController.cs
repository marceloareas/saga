using saga.Services;
using saga.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using saga.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace saga.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var token = await _userService.AuthenticateAsync(loginDto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("resetPasswordRequet")]
        public async Task<IActionResult> ResetPasswordRequest(RequestResetPasswordDto loginDto)
        {
            try
            {
                await _userService.ResetPasswordRequestAsync(loginDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("resetPassword")]
        [Authorize(Roles = "ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto loginDto)
        {
            try
            {
                var token = await _userService.ResetPasswordAsync(loginDto);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
