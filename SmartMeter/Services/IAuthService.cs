using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartMeter.Models;
using SmartMeter.Models.DTOs;
using SmartMeter.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SmartMeter.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);

        Task<User?> RegisterAsync(UserDto request);
        Task<bool> ChangePasswordAsync(long userId, ChangePasswordDto request);
    }
}