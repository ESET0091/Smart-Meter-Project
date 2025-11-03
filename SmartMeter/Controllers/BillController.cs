using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMeter.Models.DTOs;
using SmartMeter.Services;

namespace SmartMeter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly ILogger<BillsController> _logger;

        public BillsController(IBillService billService, ILogger<BillsController> logger)
        {
            _billService = billService;
            _logger = logger;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<BillDto>> GenerateBill(GenerateBillDto request)
        {
            try
            {
                if (request.CurrentReading <= 0)
                    return BadRequest("Current reading must be greater than 0");

                if (DateOnly.Parse(request.BillingPeriodStart) >= DateOnly.Parse(request.BillingPeriodEnd))
                    return BadRequest("Billing period start must be before end");

                var bill = await _billService.GenerateBillAsync(request);
                return Ok(bill);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating bill for consumer {ConsumerId}", request.ConsumerId);
                return StatusCode(500, "Error generating bill");
            }
        }

        [HttpGet("consumer/{consumerId}")]
        public async Task<ActionResult<List<BillDto>>> GetConsumerBills(long consumerId)
        {
            try
            {
                var bills = await _billService.GetConsumerBillsAsync(consumerId);
                return Ok(bills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bills for consumer {ConsumerId}", consumerId);
                return StatusCode(500, "Error retrieving bills");
            }
        }

        [HttpGet("{billId}")]
        public async Task<ActionResult<BillDto>> GetBill(int billId)
        {
            try
            {
                var bill = await _billService.GetBillByIdAsync(billId);
                if (bill == null)
                    return NotFound("Bill not found");

                return Ok(bill);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bill {BillId}", billId);
                return StatusCode(500, "Error retrieving bill");
            }
        }
    }
}