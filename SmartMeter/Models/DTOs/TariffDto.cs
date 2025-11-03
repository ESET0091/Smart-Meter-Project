using System.ComponentModel.DataAnnotations;

namespace SmartMeter.Models.DTOs
{
    public class TariffDto
    {
        public int TariffId { get; set; }

        //[Required]
        public  string Effectivefrom { get; set; }

        //[Required]
        public string Effectiveto { get; set; }

        public decimal? Baserate { get; set; }

        public decimal? Taxrate { get; set; }
    }
}
