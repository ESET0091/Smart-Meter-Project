
using SmartMeter.Models.DTOs;
using SmartMeter.Models.DTOs.EnergyConsumptionDto;

namespace SmartMeter.Services.Interface
{
    public interface IEnergyConsumptionService
    {
        Task<bool> RecordEnergyConsumptionAsync(EnergyConsumptionRecordDto record);
        Task<List<EnergyConsumptionResponseDto>> GetEnergyConsumptionAsync(EnergyConsumptionQueryDto query, int userId);
        Task<decimal> GetTotalConsumptionAsync(string meterSerialNo, DateTime fromDate, DateTime toDate, int userId);
    }
}