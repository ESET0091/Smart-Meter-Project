namespace SmartMeter.Models.DTOs
{
    public class HistoricalConsumptionDto
    {
        public string OrgUnitName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? TotalEnergyConsumed { get; set; }
    }
}
