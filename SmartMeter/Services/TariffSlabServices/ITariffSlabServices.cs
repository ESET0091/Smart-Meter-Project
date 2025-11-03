using SmartMeter.Models.DTOs;
using SmartMeter.Models;

namespace SmartMeter.Services.TariffSlabServices
{
    public interface ITariffSlabServices
    {
        Task<Tariffslab> UpdateTariffSlabAsync(TariffSlabDto request);
    }
}
