using System.Text.Json.Serialization;

namespace SmartMeter.Models.DTOs.EnergyConsumptionDto
{
    public class EnergyConsumptionRecordDto
    {
        public string MeterSerialNo { get; set; }
        public string? ReadingDate { get; set; }
        public decimal EnergyConsumed { get; set; } // kWh

        // New property exposed in JSON that includes the unit. Keeps numeric EnergyConsumed intact.
        [JsonPropertyName("energyConsumedWithUnit")]
        public string EnergyConsumedWithUnit => $"{EnergyConsumed} kWh";

    }
}
