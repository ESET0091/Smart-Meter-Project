using System.ComponentModel.DataAnnotations;

namespace SmartMeter.Models.DTOs
{
    public class ConsumerDto
    {
        //public long Userid { get; set; }
        //public long Consumerid { get; set; }

        [Required]
        public string? Username { get; set; }

        public string? Name { get; set; }

        //public long? Addressid { get; set; }
        [Required]
        public string? Phone { get; set; }

        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public int Orgunitid { get; set; }
        [Required]
        public int Tariffid { get; set; }

        //public string Status { get; set; } = null!;

        [Required]
        public string Password { get; set; } = string.Empty;
        public DateTime Createdat { get; set; }

        //public string Createdby { get; set; } = null!;

        //public DateTime? Updatedat { get; set; }

        //public string? Updatedby { get; set; }

        //public bool Deleted { get; set; }

    }
}
