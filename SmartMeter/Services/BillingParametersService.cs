
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Models.DTOs;
using SmartMeter.Models.DTOs.BillingParameteresDto;
using SmartMeter.Services.Interface;
//using SmartMeter.Models.DTOs.BillingParameteresDto;
namespace SmartMeter.Services.Implementation
{
    public class BillingParametersService : IBillingParametersService
    {
        private readonly SmartMeterDbContext _context;
        private readonly ILogger<BillingParametersService> _logger;

        public BillingParametersService(SmartMeterDbContext context, ILogger<BillingParametersService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ValidateBillingParametersAsync(decimal Voltage, decimal Current, decimal PowerFactor)
        {
            if (Voltage < 200 || Voltage > 250)
            {
                _logger.LogWarning("Invalid voltage value: {Voltage}V", Voltage);
            }

            if (Current < 0)
            {
                _logger.LogWarning("Invalid current value: {Current}A", Current);
            }

            if (PowerFactor < 0 || PowerFactor > 1)
            {
                _logger.LogWarning("Invalid power factor value: {PowerFactor}", PowerFactor);
            }

            return true;

        }

        public async Task<List<BilingParametersResponseDto>> GetBillingParametersAsync(BillingParametersQueryDto query, int ConsumerId)
        {

            // Verify user has access to this meter
            var canAccess = await _context.Meters
                .Include(m => m.Consumer)
                .AnyAsync(m => m.Meterserialno == query.Meterserialno && m.Consumerid == ConsumerId
                           && m.Status == "Active"
                           && m.Consumer.Status == "Active");
            if (!canAccess)
            {
                throw new UnauthorizedAccessException("You do not have access to this meter's billing parameters.");
            }

           


            var readings = await _context.Meterreadings
                .Where(mr => mr.Meterid == query.Meterserialno
                    && mr.Meterreadingdate >= DateTime.SpecifyKind(DateOnly.Parse(query.FromDate).ToDateTime(TimeOnly.MinValue),DateTimeKind.Utc)
                    && mr.Meterreadingdate <= DateTime.SpecifyKind(DateOnly.Parse(query.ToDate).ToDateTime(TimeOnly.MinValue),DateTimeKind.Utc)
                    && mr.Voltage > 0) // Only readings with billing parameters
                .OrderBy(mr => mr.Meterreadingdate)
                .Select(mr => new BilingParametersResponseDto
                {
                    ReadingDate = mr.Meterreadingdate,
                    Voltage = mr.Voltage,
                    Current = mr.Current,
                    PowerFactor = mr.Powerfactor,
                    RealPower = Math.Round(mr.Voltage * mr.Current * mr.Powerfactor / 1000, 3), // kW
                    ApparentPower = Math.Round(mr.Voltage * mr.Current / 1000, 3) // kVA
                })
                .ToListAsync();


            return readings;

        }
    }
}
