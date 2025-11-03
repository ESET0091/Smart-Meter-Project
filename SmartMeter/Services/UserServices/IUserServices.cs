using SmartMeter.Models;
using SmartMeter.Models.DTOs;

namespace SmartMeter.Services.UserServices
{
    public interface IUserServices
    {
        Task<HistoricalConsumptionDto> GetHistoricalConsumptionAsync(int orgUnitId, DateTime startDate, DateTime endDate);
        Task<Consumer?> RegisterConsumerAsync(ConsumerDto request);
    }
}
