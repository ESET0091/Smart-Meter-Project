using SmartMeter.Models.DTOs;
using SmartMeter.Models;

namespace SmartMeter.Services.TodRuleServices
{
    public interface ITodRuleServices
    {
        Task<Todrule> UpdateTodRuleAsync(TodRuleDto request);
    }
}
