namespace SmartMeter.Models.DTOs
{
    public class TodRuleDto
    {
        public int Todruleid { get; set; }

        public int? Tariffid { get; set; }

        //public string Name { get; set; } = null!;

        public string Starttime { get; set; }

        public string Endtime { get; set; }

        public decimal? Rateperkwh { get; set; }
    }
}
