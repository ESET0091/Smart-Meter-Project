//namespace SmartMeter.Controllers
//{
//    public class AuthController
//    {
//    }
//}


using SmartMeter.Models;
using SmartMeter.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using SmartMeter.Services;

namespace SmartMeter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService service;
        public AuthController(IAuthService service)
        {
            this.service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User?>> Register(UserDto request)
        {
            var user = await service.RegisterAsync(request);
            if (user is null)
                return BadRequest("Username already exist");
            return Ok(user);

        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var token = await service.LoginAsync(request);
            if (token is null)
            {
                return BadRequest("Username/password is wrong");
            }
            return Ok(token);

        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var token = await service.RefreshTokenAsync(request);
            if (token is null)
            {
                return BadRequest("Invalid/expired Token");
            }
            return Ok(token);

        }

        [HttpGet("Auth-end points")]
        [Authorize]
        public ActionResult AuthCheck()
        {
            return Ok();
        }
        [HttpGet("Admin-end points")]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminCheck()
        {
            return Ok();
        }

    }
}