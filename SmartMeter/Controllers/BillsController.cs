//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SmartMeter.Models.DTOs;
//using SmartMeter.Services;

//namespace SmartMeter.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class BillsController : ControllerBase
//    {
//        private readonly IBillService _billService;
//        private readonly ILogger<BillsController> _logger;

//        public BillsController(IBillService billService, ILogger<BillsController> logger)
//        {
//            _billService = billService;
//            _logger = logger;
//        }

//        [HttpPost("generate")]
//        public async Task<ActionResult<BillResponseDto>> GenerateBill(GenerateBillDto request)
//        {
//            try
//            {
//                if (request.CurrentReading <= 0)
//                    return BadRequest("Current reading must be greater than 0");

//                if (request.BillingPeriodStart >= request.BillingPeriodEnd)
//                    return BadRequest("Billing period start must be before end");

//                var bill = await _billService.GenerateBillAsync(request);
//                return Ok(bill);
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error generating bill for consumer {ConsumerId}", request.ConsumerId);
//                return StatusCode(500, "Error generating bill");
//            }
//        }

//        [HttpGet("consumer/{consumerId}")]
//        public async Task<ActionResult<List<BillResponseDto>>> GetConsumerBills(long consumerId)
//        {
//            try
//            {
//                var bills = await _billService.GetConsumerBillsAsync(consumerId);
//                return Ok(bills);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving bills for consumer {ConsumerId}", consumerId);
//                return StatusCode(500, "Error retrieving bills");
//            }
//        }

//        [HttpGet("{billId}")]
//        public async Task<ActionResult<BillResponseDto>> GetBill(int billId)
//        {
//            try
//            {
//                var bill = await _billService.GetBillByIdAsync(billId);
//                if (bill == null)
//                    return NotFound("Bill not found");

//                return Ok(bill);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving bill {BillId}", billId);
//                return StatusCode(500, "Error retrieving bill");
//            }
//        }

//        [HttpPost("pay")]
//        public async Task<ActionResult> PayBill(PayBillDto request)
//        {
//            try
//            {
//                var result = await _billService.PayBillAsync(request);
//                if (!result)
//                    return NotFound("Bill not found");

//                return Ok("Bill paid successfully");
//            }
//            catch (InvalidOperationException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error paying bill {BillId}", request.BillId);
//                return StatusCode(500, "Error processing payment");
//            }
//        }

//        [HttpGet("pending")]
//        [Authorize(Roles = "Admin")]
//        public async Task<ActionResult<List<BillResponseDto>>> GetPendingBills()
//        {
//            try
//            {
//                var bills = await _billService.GetPendingBillsAsync();
//                return Ok(bills);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving pending bills");
//                return StatusCode(500, "Error retrieving pending bills");
//            }
//        }

//        [HttpGet("{billId}/download")]
//        public async Task<IActionResult> DownloadBill(int billId)
//        {
//            try
//            {
//                var bill = await _billService.GetBillByIdAsync(billId);
//                if (bill == null)
//                    return NotFound("Bill not found");

//                var pdfBytes = await _billService.GeneratePdfBillAsync(bill);
//                return File(pdfBytes, "application/pdf", $"Bill_{bill.BillId}.pdf");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error generating PDF for bill {BillId}", billId);
//                return StatusCode(500, "Error generating bill PDF");
//            }
//        }
//    }
//}




//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using SmartMeter.Models.DTOs;
//using SmartMeter.Services;

//namespace SmartMeter.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class BillsController : ControllerBase
//    {
//        private readonly IBillService _billService;
//        private readonly ILogger<BillsController> _logger;

//        public BillsController(IBillService billService, ILogger<BillsController> logger)
//        {
//            _billService = billService;
//            _logger = logger;
//        }

//        [HttpPost("generate")]
//        public async Task<ActionResult<BillResponseDto>> GenerateBill(GenerateBillDto request)
//        {
//            try
//            {
//                if (request.CurrentReading <= 0)
//                    return BadRequest("Current reading must be greater than 0");

//                if (DateOnly.Parse(request.BillingPeriodStart) >= DateOnly.Parse(request.BillingPeriodEnd))
//                    return BadRequest("Billing period start must be before end");

//                var bill = await _billService.GenerateBillAsync(request);
//                return Ok(bill);
//            }
//            catch (ArgumentException ex)
//            {
//                return BadRequest(ex.Message);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error generating bill for consumer {ConsumerId}", request.ConsumerId);
//                return StatusCode(500, "Error generating bill");
//            }
//        }

//        [HttpGet("consumer/{consumerId}")]
//        public async Task<ActionResult<List<BillResponseDto>>> GetConsumerBills(long consumerId)
//        {
//            try
//            {
//                var bills = await _billService.GetConsumerBillsAsync(consumerId);
//                return Ok(bills);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving bills for consumer {ConsumerId}", consumerId);
//                return StatusCode(500, "Error retrieving bills");
//            }
//        }

//        [HttpGet("{billId}")]
//        public async Task<ActionResult<BillResponseDto>> GetBill(int billId)
//        {
//            try
//            {
//                var bill = await _billService.GetBillByIdAsync(billId);
//                if (bill == null)
//                    return NotFound("Bill not found");

//                return Ok(bill);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving bill {BillId}", billId);
//                return StatusCode(500, "Error retrieving bill");
//            }
//        }
//    }
//}





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
                //if (request.CurrentReading <= 0)
                //    return BadRequest("Current reading must be greater than 0");

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

        // ADD THESE TWO NEW ENDPOINTS:

        //[HttpPost("pay")]
        //public async Task<ActionResult> PayBill(PayBillDto request)
        //{
        //    try
        //    {
        //        if (request.BillId <= 0)
        //            return BadRequest("Invalid bill ID");

        //        var result = await _billService.PayBillAsync(request);
        //        if (!result)
        //            return NotFound("Bill not found");

        //        return Ok(new { message = "Bill paid successfully" });
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error paying bill {BillId}", request.BillId);
        //        return StatusCode(500, "Error processing payment");
        //    }
        //}




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