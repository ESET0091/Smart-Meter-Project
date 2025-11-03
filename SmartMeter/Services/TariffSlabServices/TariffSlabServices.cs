using SmartMeter.Data;
using SmartMeter.Models.DTOs;
using SmartMeter.Models;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Services.TariffSlabServices;

namespace SmartMeter.Services.TariffSlabServices
{
    public class TariffSlabServices : ITariffSlabServices
    {
        private readonly SmartMeterDbContext _context;


        public TariffSlabServices(SmartMeterDbContext context)
        {
            _context = context;

        }

        public async Task<Tariffslab> UpdateTariffSlabAsync(TariffSlabDto request)
        {
            var tariffslab = await _context.Tariffslabs.FirstOrDefaultAsync(t => t.Tariffslabid == request.Tariffslabid);
            if (tariffslab == null)
                return null;

            // Update only provided fields
            if (request.Tariffid.HasValue)
                tariffslab.Tariffid = request.Tariffid.Value;

            if (request.Fromkwh.HasValue)
                tariffslab.Fromkwh = request.Fromkwh.Value;

            if (request.Tokwh.HasValue)
                tariffslab.Tokwh = request.Tokwh.Value;

            if (request.Rateperkwh.HasValue)
                tariffslab.Rateperkwh = request.Rateperkwh.Value;

            await _context.SaveChangesAsync();
            return tariffslab;
        }
    }
}
