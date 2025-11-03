using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Models.DTOs;
using SmartMeter.Services.BillServices;

namespace SmartMeter.Services
{
    public class BillService  : IBillService
    {
        private readonly SmartMeterDbContext _context;
        private readonly ILogger<BillService> _logger;

        public BillService (SmartMeterDbContext context, ILogger<BillService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BillResponseDto> GenerateBillAsync(GenerateBillDto request)
        {
            _logger.LogInformation("Generating bill for consumer {ConsumerId} and meter {MeterSerialNo}",
                request.ConsumerId, request.MeterSerialNo);

            // 1. Validate consumer exists WITH TARIFF
            var consumer = await _context.Consumers
                .Include(c => c.Address)
                .Include(c => c.Tariff)  // ADD THIS LINE
                    .ThenInclude(t => t.Tariffslabs)  // ADD THIS LINE
                .FirstOrDefaultAsync(c => c.Consumerid == request.ConsumerId);

            if (consumer == null)
                throw new ArgumentException($"Consumer with ID {request.ConsumerId} not found");

            // 2. Validate meter exists and belongs to consumer
            var meter = await _context.Meters
                .FirstOrDefaultAsync(m => m.Meterserialno == request.MeterSerialNo && m.Consumerid == request.ConsumerId);

            if (meter == null)
                throw new ArgumentException($"Meter with serial number {request.MeterSerialNo} not found for consumer {request.ConsumerId}");

            // 3. Get previous reading
            DateTime BillingPeriodSt = DateTime.SpecifyKind(
                DateOnly.Parse(request.BillingPeriodStart).ToDateTime(TimeOnly.MinValue),
                DateTimeKind.Utc);
            var previousReading = await GetPreviousReading(request.MeterSerialNo, BillingPeriodSt);

            // 4. Calculate units consumed
            var unitsConsumed = request.CurrentReading - previousReading;
            if (unitsConsumed < 0)
                throw new ArgumentException("Current reading cannot be less than previous reading");

            // 5. CALCULATE CHARGES USING TARIFF (NEW CODE)
            var (baseAmount, taxAmount, totalAmount) = await CalculateCharges(unitsConsumed, consumer.Tariffid);

            // 6. Create billing record WITH CALCULATED VALUES
            var billing = new Billing
            {
                Consumerid = request.ConsumerId,
                Meterid = request.MeterSerialNo,
                Billingperiodstart = DateTime.SpecifyKind(
                    DateOnly.Parse(request.BillingPeriodStart).ToDateTime(TimeOnly.MinValue),
                    DateTimeKind.Utc),
                Billingperiodend = DateTime.SpecifyKind(
                    DateOnly.Parse(request.BillingPeriodEnd).ToDateTime(TimeOnly.MinValue),
                    DateTimeKind.Utc),
                Totalunitsconsumed = unitsConsumed,
                Baseamount = baseAmount,
                Taxamount = taxAmount,
                Totalamount = totalAmount,
                Generatedat = DateTime.UtcNow,
                Paiddate = null,
                Duedate = DateTime.SpecifyKind(
                    DateOnly.Parse(request.BillingPeriodEnd).ToDateTime(TimeOnly.MinValue),
                    DateTimeKind.Utc).AddDays(15),
                Paymentstatus = "Unpaid"
            };

            try
            {
                _context.Billings.Add(billing);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw; // Re-throw the exception after logging
            }

            _logger.LogInformation("Bill generated successfully: {Units} units, ₹{Amount}",
                unitsConsumed, totalAmount);

            // 7. Return response
            return await MapToBillResponseDto(billing);
        }




        public async Task<List<BillResponseDto>> GetConsumerBillsAsync(long consumerId)
        {
            var bills = await _context.Billings
                .Include(b => b.Consumer)
                    .ThenInclude(c => c.Address) // Include Address
                .Include(b => b.Meter)
                .Where(b => b.Consumerid == consumerId)
                .OrderByDescending(b => b.Generatedat)
                .ToListAsync(); // First get the list as entities

            // Then convert each entity to DTO
            var billDtos = new List<BillResponseDto>();
            foreach (var bill in bills)
            {
                billDtos.Add(await MapToBillResponseDto(bill));
            }

            return billDtos;
        }

        public async Task<BillResponseDto?> GetBillByIdAsync(int billId)
        {
            var billing = await _context.Billings
                .Include(b => b.Consumer)
                    .ThenInclude(c => c.Address) // Include Address
                .Include(b => b.Meter)
                .FirstOrDefaultAsync(b => b.Billid == billId);

            if (billing == null) return null;

            return await MapToBillResponseDto(billing);
        }

        private async Task<decimal> GetPreviousReading(string meterSerialNo, DateTime billingPeriodStart)
        {
            // Get the last reading before the billing period start
            //var previousReadingRecord = await _context.Meterreadings
            //    .Where(mr => mr.Meterid == meterSerialNo &&
            //               mr.Meterreadingdate < billingPeriodStart.ToDateTime(TimeOnly.MinValue))
            //    .OrderByDescending(mr => mr.Meterreadingdate)
            //    .FirstOrDefaultAsync();

            var previousReadingRecord = await _context.Meterreadings
                .Where(mr => mr.Meterid == meterSerialNo &&
                           mr.Meterreadingdate < billingPeriodStart)
                .OrderByDescending(mr => mr.Meterreadingdate)
                .FirstOrDefaultAsync();


            return previousReadingRecord?.Energyconsumed ?? 0;
        }

        private async Task<BillResponseDto> MapToBillResponseDto(Billing billing)
        {
            // Build address from Address table
            string address = "Address not available";
            if (billing.Consumer.Address != null)
            {
                address = $"{billing.Consumer.Address.Houseno}, {billing.Consumer.Address.Lanelocality}, {billing.Consumer.AidNavigation.City}, {billing.Consumer.AidNavigation.State} - {billing.Consumer.AidNavigation.Pincode}";
            }

            // Fix for DateOnly to DateTime conversion
            DateTime? paidDateTime = null;
            if (billing.Paiddate.HasValue)
            {
                paidDateTime = billing.Paiddate;
            }

            return new BillResponseDto
            {
                BillId = billing.Billid,
                ConsumerId = billing.Consumerid,
                MeterSerialNo = billing.Meterid,
                ConsumerName = billing.Consumer.Name, // Using Name property instead of FirstName+LastName
                Address = address,
                BillingPeriodStart = billing.Billingperiodstart,
                BillingPeriodEnd = billing.Billingperiodend,
                TotalUnitsConsumed = billing.Totalunitsconsumed,
                BaseAmount = billing.Baseamount,
                TaxAmount = billing.Taxamount,
                TotalAmount = billing.Totalamount ?? 0,
                GeneratedAt = billing.Generatedat,
                DueDate = billing.Duedate,
                PaymentStatus = billing.Paymentstatus,
                PaidDate = paidDateTime
            };
        }




        // Method 1: Pay Bill
        public async Task<bool> PayBillAsync(PayBillDto request)
        {
            var billing = await _context.Billings.FindAsync(request.BillId);
            if (billing == null) return false;

            if (billing.Paymentstatus == "Paid")
                throw new InvalidOperationException("Bill is already paid");

            // Update billing status
            billing.Paymentstatus = "Paid";
            billing.Paiddate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Bill {BillId} paid successfully", request.BillId);
            return true;
        }

        // Method 2: Get Pending Bills
        public async Task<List<BillResponseDto>> GetPendingBillsAsync()
        {
            var bills = await _context.Billings
                .Include(b => b.Consumer)
                    .ThenInclude(c => c.Address)
                .Include(b => b.Meter)
                .Where(b => b.Paymentstatus == "Unpaid" && b.Duedate >= DateOnly.FromDateTime(DateTime.UtcNow))
                .OrderBy(b => b.Duedate)
                .ToListAsync();

            var billDtos = new List<BillResponseDto>();
            foreach (var bill in bills)
            {
                billDtos.Add(await MapToBillResponseDto(bill));
            }

            return billDtos;
        }

        // Method 3: Calculate Charges (NEW - Add this)
        private async Task<(decimal baseAmount, decimal taxAmount, decimal totalAmount)> CalculateCharges(decimal unitsConsumed, int tariffId)
        {
            // Get tariff with slabs from database
            var tariff = await _context.Tariffs
                .Include(t => t.Tariffslabs)
                .FirstOrDefaultAsync(t => t.Tariffid == tariffId);

            if (tariff == null)
                throw new ArgumentException($"Tariff with ID {tariffId} not found");

            _logger.LogInformation("Calculating charges for {Units} units using tariff {TariffName}",
                unitsConsumed, tariff.Name);

            decimal baseAmount = 0;

            // METHOD 1: Calculate based on tariff slabs (if available)
            if (tariff.Tariffslabs.Any(s => !s.Deleted))
            {
                baseAmount = CalculateSlabBasedAmount(unitsConsumed, tariff.Tariffslabs.Where(s => !s.Deleted).OrderBy(s => s.Fromkwh).ToList());
                _logger.LogInformation("Slab-based calculation: ₹{Amount}", baseAmount);
            }
            // METHOD 2: Use base rate if no slabs
            else
            {
                baseAmount = unitsConsumed * tariff.Baserate;
                _logger.LogInformation("Base rate calculation: {Units} × {Rate} = ₹{Amount}",
                    unitsConsumed, tariff.Baserate, baseAmount);
            }

            // Calculate tax and total
            var taxAmount = baseAmount * (tariff.Taxrate / 100);
            var totalAmount = baseAmount + taxAmount;

            _logger.LogInformation("Final calculation: Base=₹{Base}, Tax=₹{Tax}, Total=₹{Total}",
                baseAmount, taxAmount, totalAmount);

            return (baseAmount, taxAmount, totalAmount);
        }

        // Method 4: Slab Calculation (NEW - Add this)
        private decimal CalculateSlabBasedAmount(decimal unitsConsumed, List<Tariffslab> slabs)
        {
            decimal totalAmount = 0;
            decimal remainingUnits = unitsConsumed;

            _logger.LogInformation("Starting slab calculation for {Units} units", unitsConsumed);

            foreach (var slab in slabs)
            {
                if (remainingUnits <= 0) break;

                // Calculate units in this slab
                decimal slabUnits;
                decimal slabRange = slab.Tokwh - slab.Fromkwh;

                if (remainingUnits > slabRange)
                {
                    // Use entire slab range
                    slabUnits = slabRange;
                }
                else
                {
                    // Use remaining units
                    slabUnits = remainingUnits;
                }

                decimal slabAmount = slabUnits * slab.Rateperkwh;
                totalAmount += slabAmount;

                _logger.LogInformation("Slab {From}-{To} kWh: {Units} units × ₹{Rate} = ₹{Amount}",
                    slab.Fromkwh, slab.Tokwh, slabUnits, slab.Rateperkwh, slabAmount);

                remainingUnits -= slabUnits;
            }

            // If there are remaining units beyond the highest slab, use the last slab's rate
            if (remainingUnits > 0)
            {
                var lastSlab = slabs.Last();
                decimal extraAmount = remainingUnits * lastSlab.Rateperkwh;
                totalAmount += extraAmount;

                _logger.LogInformation("Extra units beyond slabs: {Units} × ₹{Rate} = ₹{Amount}",
                    remainingUnits, lastSlab.Rateperkwh, extraAmount);
            }

            _logger.LogInformation("Total slab amount: ₹{Amount}", totalAmount);
            return totalAmount;
        }
    }
}

    