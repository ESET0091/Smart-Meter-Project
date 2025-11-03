using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Models.DTOs;
namespace SmartMeter.Services.TariffServices
{
    public class TariffServices : ITariffServices
    {
        private readonly SmartMeterDbContext _context;
      

        public TariffServices (SmartMeterDbContext context)
        {
            _context = context;
         
        }

        public async Task<IEnumerable<Tariff>> GetAllTariffsAsync()
        {
            Console.WriteLine("Enters the function to get the list...");

            var listofTariff = await _context.Tariffs
                .Include(t => t.Todrules.Where(r => !r.Deleted))
                .ToListAsync();

            Console.WriteLine(listofTariff);
            return listofTariff;
        }

        public async Task<Tariff> GetTariffByIdAsync(int tariffId)
        {
            return await _context.Tariffs.Include(t => t.Todrules.Where(r => !r.Deleted))
                .FirstOrDefaultAsync(t => t.Tariffid == tariffId);
        }

        public async Task<IEnumerable<Todrule>> GetTodRulesByTariffAsync(int tariffId)
        {
            return await _context.Todrules
                .Where(r => r.Tariffid == tariffId && !r.Deleted)
                .ToListAsync();
        }

        public async Task<Tariff> UpdateTariffAsync(TariffDto request)
        {
            var tariff = await _context.Tariffs.FirstOrDefaultAsync(t => t.Tariffid == request.TariffId);
            if (tariff == null)
                return null; // Let controller handle not-found case

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.Effectivefrom))
            {
                DateOnly dateOnly = DateOnly.Parse(request.Effectivefrom);
                tariff.Effectivefrom = dateOnly;

            }

            if (!string.IsNullOrEmpty(request.Effectiveto))
            {
                DateOnly dateOnly = DateOnly.Parse(request.Effectiveto);
                tariff.Effectiveto = dateOnly;
            }

            if (request.Baserate.HasValue)
                tariff.Baserate = request.Baserate.Value;

            if (request.Taxrate.HasValue)
                tariff.Taxrate = request.Taxrate.Value;

            await _context.SaveChangesAsync();
            return tariff;
        }


    }
}
