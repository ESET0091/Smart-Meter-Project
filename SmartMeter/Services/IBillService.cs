using SmartMeter.Models.DTOs;

namespace SmartMeter.Services
{
    public interface IBillService
    {
        Task<BillResponseDto> GenerateBillAsync(GenerateBillDto request);
        Task<List<BillResponseDto>> GetConsumerBillsAsync(long consumerId);
        Task<BillResponseDto?> GetBillByIdAsync(int billId);

        // ADD THESE TWO METHODS:
        //Task<bool> PayBillAsync(PayBillDto request);
        Task<PaymentResponseDto?> PayBillAsync(PayBillDto request);
        Task<List<BillResponseDto>> GetPendingBillsAsync();
    }

    // ADD THIS DTO CLASS (add it in the same file):
    public class PayBillDto
    {
        public int BillId { get; set; }
        public string PaymentMethod { get; set; } = "Cash";
    }
}