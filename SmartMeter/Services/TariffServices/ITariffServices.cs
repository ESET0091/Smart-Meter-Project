using SmartMeter.Models;
using SmartMeter.Models.DTOs;

namespace SmartMeter.Services.TariffServices
{
    public interface ITariffServices
    {
        Task<IEnumerable<Tariff>> GetAllTariffsAsync();
        //Task GetConsumerTariffAsync(object tariffid);
        Task<Tariff> GetTariffByIdAsync(int tariffId);
        Task<IEnumerable<Todrule>> GetTodRulesByTariffAsync(int tariffId);
        Task<Tariff> UpdateTariffAsync(TariffDto request);


    }
}
