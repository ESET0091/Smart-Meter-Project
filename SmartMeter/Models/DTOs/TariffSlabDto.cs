namespace SmartMeter.Models.DTOs
{
    public class TariffSlabDto
    {
        public int? Tariffslabid { get; set; }

        public int? Tariffid { get; set; }

        public decimal? Fromkwh { get; set; }

        public decimal? Tokwh { get; set; }

        public decimal? Rateperkwh { get; set; }
    }
}
