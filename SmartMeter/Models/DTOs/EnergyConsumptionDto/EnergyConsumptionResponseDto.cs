namespace SmartMeter.Models.DTOs.EnergyConsumptionDto
{
    public class EnergyConsumptionResponseDto
    {
        public string? ReadingDate { get; set; }
        public decimal EnergyConsumed { get; set; } // kWh
        //public decimal CumulativeConsumption { get; set; } // kWh

     

    }
}
