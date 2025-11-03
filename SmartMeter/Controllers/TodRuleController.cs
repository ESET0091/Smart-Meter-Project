using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMeter.Models.DTOs;
using SmartMeter.Services.TodRuleServices;

namespace SmartMeter.Controllers
{
    public class TodRuleController : ControllerBase
    {
        private readonly ITodRuleServices _todRuleService;
        public TodRuleController(ITodRuleServices todruleService)
        {
            _todRuleService = todruleService;
        }


        [HttpPut("update-todrule")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpadateTodRule([FromBody] TodRuleDto request)
        {

            if (request == null)
                return BadRequest("Invalid request body.");

            var result = await _todRuleService.UpdateTodRuleAsync(request);

            if (result == null)
                return NotFound($"TodRule with ID {request.Todruleid} not found.");

            return Ok(new
            {
                message = "Tariff updated successfully.",
                updatedTodRule = result
            });
        }
    }
}
