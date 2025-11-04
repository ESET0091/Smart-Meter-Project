
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
        public async Task<ActionResult<BillResponseDto>> GenerateBill(GenerateBillDto request)
        {
            try
            {
                

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

        //
        [HttpGet("consumer/bills-by-date")]
        public async Task<ActionResult<List<BillResponseDto>>> GetConsumerBillsByDateRange([FromBody] ConsumerBillsByDateRangeDto request)
        {
            try
            {
                if (request.ConsumerId <= 0)
                    return BadRequest("Invalid consumer ID");

                if (request.FromDate >= request.ToDate)
                    return BadRequest("From date must be before To date");

                // Ensure dates are in UTC
                var fromDate = request.FromDate.ToUniversalTime();
                var toDate = request.ToDate.ToUniversalTime();

                var bills = await _billService.GetConsumerBillsByDateRangeAsync(request.ConsumerId, fromDate, toDate);
                return Ok(bills);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bills for consumer {ConsumerId} from {FromDate} to {ToDate}",
                    request.ConsumerId, request.FromDate, request.ToDate);
                return StatusCode(500, "Error retrieving bills");
            }
        }
        //

        [HttpGet("consumer/{consumerId}")]
        public async Task<ActionResult<List<BillResponseDto>>> GetConsumerBills(long consumerId)
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
        public async Task<ActionResult<BillResponseDto>> GetBill(int billId)
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

        [HttpPost("pay")]
        public async Task<ActionResult<PaymentResponseDto>> PayBill(PayBillDto request)  // Changed return type
        {
            try
            {
                if (request.BillId <= 0)
                    return BadRequest("Invalid bill ID");

                if (string.IsNullOrEmpty(request.PaymentMethod))
                    return BadRequest("Payment method is required");

                var result = await _billService.PayBillAsync(request);
                if (result == null)
                    return NotFound("Bill not found");

                return Ok(result);  // Now returns detailed payment info
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error paying bill {BillId}", request.BillId);
                return StatusCode(500, "Error processing payment");
            }
        }




        [HttpGet("pending")]
        public async Task<ActionResult<List<BillResponseDto>>> GetPendingBills()
        {
            try
            {
                var bills = await _billService.GetPendingBillsAsync();
                return Ok(bills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending bills");
                return StatusCode(500, "Error retrieving pending bills");
            }
        }
    }
}