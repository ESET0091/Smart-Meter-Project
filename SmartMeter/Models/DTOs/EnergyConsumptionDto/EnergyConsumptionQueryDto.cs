namespace SmartMeter.Models.DTOs.EnergyConsumptionDto
{
    public class EnergyConsumptionQueryDto
    {
        public string MeterSerialNo { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
