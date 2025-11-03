using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMeter.Data;
using SmartMeter.Models.DTOs;
using SmartMeter.Services.TariffServices;
using System.Security.Claims;

namespace SmartMeter.Controllers
{

    [ApiController]
    //[Route()]
    public class TariffController : ControllerBase
    {
        private readonly SmartMeterDbContext _context;
        private readonly ITariffServices _tariffService;

        public TariffController(SmartMeterDbContext context, ITariffServices tariffService)
        {
            _context = context;
            _tariffService = tariffService;
        }

        [HttpGet("tariffs")]
        //[Authorize(Roles = "Admin,User,Consumer")]
        public async Task<IActionResult> GetAllTariffs()
        {
            var tariffs = await _tariffService.GetAllTariffsAsync();
            return Ok(tariffs);
        }

        [HttpGet("tariff/{id}")]
        public async Task<IActionResult> GetTariffById(int id)
        {
            var tariff = await _tariffService.GetTariffByIdAsync(id);
            if (tariff == null)
                return NotFound(new { message = "Tariff not found" });

            return Ok(tariff);
        }

        [HttpGet("tariff/{id}/todrules")]
        public async Task<IActionResult> GetTodRulesByTariff(int id)
        {
            var rules = await _tariffService.GetTodRulesByTariffAsync(id);
            return Ok(rules);
        }


        [HttpPut("update-tariff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTariff([FromBody] TariffDto request)
        {

            if (request == null)
                return BadRequest("Invalid request body.");

            var result = await _tariffService.UpdateTariffAsync(request);

            if (result == null)
                return NotFound($"Tariff with ID {request.TariffId} not found.");

            return Ok(new
            {
                message = "Tariff updated successfully.",
                updatedTariff = result
            });


        }
    }
}
