using SmartMeter.Data;
using SmartMeter.Models.DTOs;
using SmartMeter.Models;
using SmartMeter.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

 namespace SmartMeter.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly SmartMeterDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IConfiguration configuration, SmartMeterDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return null;

            var user = new User
            {
                Username = request.Username,
                Displayname = request.Username, // Set display name
                Isactive = true
            };

            // Hash password and convert to byte[]
            var hashedPassword = _passwordHasher.HashPassword(user, request.Password);
            user.Passwordhash = Encoding.UTF8.GetBytes(hashedPassword);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<TokenResponseDto?> LoginAsync(UserDto request)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null)
                return null;

            // Convert byte[] back to string for verification
            var storedHashedPassword = Encoding.UTF8.GetString(user.Passwordhash);
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, storedHashedPassword, request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var token = new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };

            return token;
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            // Make sure your User entity has these properties
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);

            await _context.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Userid.ToString()),
                // Remove Roles if your User entity doesn't have it, or add it if needed
                 new Claim(ClaimTypes.Role, user.Roles ?? "User"),
            };

            //var key = new SymmetricSecurityKey(
            //    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ??
            //        _configuration.GetValue<string>("AppSettings:Token") ??
            //        "fallback-secret-key-32-chars-long!"));
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ??
                       _configuration.GetValue<string>("AppSettings:Issuer") ??
                       "SmartMeter",
                audience: _configuration["Jwt:Audience"] ??
                         _configuration.GetValue<string>("AppSettings:Audience") ??
                         "SmartMeterUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
                return null;

            var token = new TokenResponseDto
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user)
            };

            return token;
        }


        public async Task<bool> ChangePasswordAsync(long userId, ChangePasswordDto request)
        {
            // Validate new password confirmation
            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return false;
            }

            // Find user
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return false;
            }

            // Verify current password
            var storedHashedPassword = Encoding.UTF8.GetString(user.Passwordhash);
            var currentPasswordVerification = _passwordHasher.VerifyHashedPassword(user, storedHashedPassword, request.CurrentPassword);

            if (currentPasswordVerification == PasswordVerificationResult.Failed)
            {
                return false;
            }

            // Hash new password
            var newHashedPassword = _passwordHasher.HashPassword(user, request.NewPassword);
            user.Passwordhash = Encoding.UTF8.GetBytes(newHashedPassword);

            // Invalidate all refresh tokens (optional security measure)
            //user.RefreshToken = null;
            //user.RefreshTokenExpiry = null;

            // Save changes
            await _context.SaveChangesAsync();
            return true;
        }
    }
}