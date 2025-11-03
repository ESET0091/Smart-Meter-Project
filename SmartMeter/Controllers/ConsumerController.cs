using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Services.TariffServices;
using System.Security.Claims;

namespace SmartMeter.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[Controller]")]
    public class ConsumerController: ControllerBase
    {


        private readonly SmartMeterDbContext _context;
        private readonly ITariffServices _tariffServices;
        public ConsumerController(SmartMeterDbContext context , ITariffServices tariffServices)
        {
            _context = context;
            _tariffServices = tariffServices;
        }

        [HttpGet("get-consumer-tariff")]

        public async Task<ActionResult<Tariff>> GetConsumerTariff()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return Unauthorized("User not authenticated.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized("Invalid Token: UserId not found in JWT.");

            
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest("Invalid UserId format in token.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Userid == userId);
            if (user == null)
                return NotFound("User not found.");

            Console.WriteLine($"email = {user.Email}");
            var consumer = await _context.Consumers.FirstOrDefaultAsync(c => c.Email == user.Email);
            if (consumer == null)
                return NotFound($"Consumer not found for this user.{user.Email}");

          
            var tariff = await _tariffServices.GetTariffByIdAsync(consumer.Tariffid);
            if (tariff == null)
                return NotFound("Tariff not found for this consumer.");

            return Ok(tariff.Name); 
        }

    }
}
