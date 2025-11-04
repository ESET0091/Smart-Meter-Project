namespace SmartMeter.Models.DTOs.BillingParameteresDto
{
    public class BilingParametersResponseDto
    {
        public DateTime ReadingDate { get; set; }
        public decimal Voltage { get; set; }
        public decimal Current { get; set; }
        public decimal PowerFactor { get; set; }
        public decimal RealPower { get; set; }
        public decimal ApparentPower { get; set; }
    }
}
