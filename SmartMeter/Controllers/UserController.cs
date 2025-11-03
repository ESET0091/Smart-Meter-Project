using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMeter.Models.DTOs;
using SmartMeter.Services;
using System.Security.Claims;

namespace SmartMeter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all endpoints
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto request)
        {
            // Get user ID from JWT token
            Console.WriteLine(request.CurrentPassword);
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
            {
                return Unauthorized("Invalid token");
            }

            // Validate request
            if (string.IsNullOrEmpty(request.CurrentPassword) ||
                string.IsNullOrEmpty(request.NewPassword) ||
                string.IsNullOrEmpty(request.ConfirmNewPassword))
            {
                return BadRequest("All fields are required");
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return BadRequest("New password and confirmation do not match");
            }

            // Attempt to change password
            var result = await _authService.ChangePasswordAsync(userId, request);

            if (!result)
            {
                return BadRequest("Failed to change password. Please check your current password.");
            }

            return Ok("Password changed successfully");
        }
    }
}