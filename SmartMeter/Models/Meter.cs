using System;
using System.Collections.Generic;

namespace SmartMeter.Models;

public partial class Meter
{
    public string Meterserialno { get; set; } = null!;

    public string Ipaddress { get; set; } = null!;

    public string Iccid { get; set; } = null!;

    public string Imsi { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public string? Firmware { get; set; }

    public string Category { get; set; } = null!;

    public DateTime Installtsutc { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = null!;

    public long? Consumerid { get; set; }

    public virtual ICollection<Billing> Billings { get; set; } = new List<Billing>();

    public virtual Consumer? Consumer { get; set; }

    public virtual ICollection<Meterreading> Meterreadings { get; set; } = new List<Meterreading>();
}
