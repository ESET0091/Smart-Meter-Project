namespace SmartMeter.Models.DTOs
{
    public class ConsumerBillsByDateRangeDto
    {
        public long ConsumerId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
