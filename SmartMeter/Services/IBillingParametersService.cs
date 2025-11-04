using SmartMeter.Models.DTOs;
using SmartMeter.Models.DTOs.BillingParameteresDto;
namespace SmartMeter.Services.Interface
{
    public interface IBillingParametersService
    {
        Task<List<BilingParametersResponseDto>> GetBillingParametersAsync(BillingParametersQueryDto query, int ConsumerId);
        Task<bool> ValidateBillingParametersAsync(decimal Voltage, decimal Current, decimal PowerFactor);
    }
}
