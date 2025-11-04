using System.Formats.Tar;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMeter.Models.DTOs;
using SmartMeter.Models.DTOs.BillingParameteresDto;

using SmartMeter.Models.DTOs.BillingParameteresDto;
using SmartMeter.Services.Interface;

namespace SmartMeter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillingParameterController : Controller
    {
        private readonly IBillingParametersService _billingParametersService;
        private readonly ILogger<BillingParameterController> _logger;

        public BillingParameterController(IBillingParametersService billingParametersService, ILogger<BillingParameterController> logger)
        {
            _billingParametersService = billingParametersService;
            _logger = logger;
        }

        [HttpGet("Parameters")]
        public async Task<ActionResult<ApiResponse<List<BilingParametersResponseDto>>>> GetBillingParameters(

        [FromQuery] string meterSerialNo,

        [FromQuery] DateTime fromDate,

        [FromQuery] DateTime toDate)


        {
            try
            {
                var userId = GetUserId();
                var query = new BillingParametersQueryDto
                {
                    Meterserialno = meterSerialNo,
                    FromDate = fromDate.ToString("o"),
                    ToDate = toDate.ToString("o")
                };
                var parameters = await _billingParametersService.GetBillingParametersAsync(query, userId);
                return Ok(ApiResponse<List<BilingParametersResponseDto>>.SuccessResponse(parameters, "Billing parameters retrieved successfully"));
            }

            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving billing parameters for meter {MeterSerialNo}", meterSerialNo);
                return StatusCode(500, ApiResponse<List<BilingParametersResponseDto>>.ErrorResponse("An error occurred while retrieving billing parameters"));
            }
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim);
        }


    }
}
