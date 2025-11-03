namespace SmartMeter.Models.DTOs
{
    public class GenerateBillDto
    {
        public long ConsumerId { get; set; }
        public string MeterSerialNo { get; set; } = null!;  // Changed to match your Meter model
        public string BillingPeriodStart { get; set; }
        public string BillingPeriodEnd { get; set; }
        public decimal CurrentReading { get; set; }
    }

    public class BillResponseDto
    {
        public int BillId { get; set; }
        public long ConsumerId { get; set; }
        public string MeterSerialNo { get; set; } = null!;  // Changed to match your Meter model
        public string ConsumerName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime BillingPeriodStart { get; set; } = DateTime.UtcNow; // Initiallly it was DayOnly
        public DateTime BillingPeriodEnd { get; set; } = DateTime.UtcNow;    // Initiallly it was DayOnly
        public decimal TotalUnitsConsumed { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; } = DateTime.UtcNow;    // Initiallly it was DayOnly
        public string PaymentStatus { get; set; } = null!;
        public DateTime? PaidDate { get; set; } = DateTime.UtcNow;
    }
}
