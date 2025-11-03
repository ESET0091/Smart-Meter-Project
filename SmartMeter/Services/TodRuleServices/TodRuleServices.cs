//using Microsoft.EntityFrameworkCore;
//using SmartMeter.Models.DTOs;
//using SmartMeter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeter.Data;
using SmartMeter.Models;
using SmartMeter.Models.DTOs;
using SmartMeter.Services.TodRuleServices;
namespace SmartMeter.Services.TodRuleServices
{
    public class TodRuleServices : ITodRuleServices
    {
        private readonly SmartMeterDbContext _context;


        public TodRuleServices(SmartMeterDbContext context)
        {
            _context = context;

        }
        public async Task<Todrule> UpdateTodRuleAsync(TodRuleDto request)
        {
        var todrule = await _context.Todrules.FirstOrDefaultAsync(t => t.Todruleid == request.Todruleid);
                    if (todrule == null)
            return null; // Let controller handle not-found case

        // Update only provided fields
        if (request.Tariffid.HasValue)
            todrule.Tariffid = request.Tariffid.Value;

        if (!string.IsNullOrEmpty(request.Starttime))
        {
            TimeOnly timeOnly = TimeOnly.Parse(request.Starttime);
            todrule.Starttime = timeOnly;

        }

        if (!string.IsNullOrEmpty(request.Endtime))
        {
            TimeOnly timeOnly = TimeOnly.Parse(request.Endtime);
            todrule.Endtime = timeOnly;
        }

        if (request.Rateperkwh.HasValue)
            todrule.Rateperkwh = request.Rateperkwh.Value;

        await _context.SaveChangesAsync();
        return todrule;
    }


}
}
