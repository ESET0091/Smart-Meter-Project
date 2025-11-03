using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMeter.Models.DTOs;
using SmartMeter.Services.TariffSlabServices;
//using SmartMeter.Services.TodRuleServices;

namespace SmartMeter.Controllers
{
    public class TariffSlabController : ControllerBase
    {
        private readonly ITariffSlabServices _tariffSlabService;
        public TariffSlabController(ITariffSlabServices tariffSlabService)
        {
            _tariffSlabService = tariffSlabService;
        }


        [HttpPut("update-tariffslab")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpadateTariffSlab([FromBody] TariffSlabDto request)
        {

            if (request == null)
                return BadRequest("Invalid request body.");

            var result = await _tariffSlabService.UpdateTariffSlabAsync(request);

            if (result == null)
                return NotFound($"TodRule with ID {request.Tariffslabid} not found.");

            return Ok(new
            {
                message = "Tariff updated successfully.",
                updatedTodRule = result
            });
        }
    }
}
