using System;
using System.Collections.Generic;

namespace SmartMeter.Models;

public partial class Todrule
{
    public int Todruleid { get; set; }

    public int Tariffid { get; set; }

    public string Name { get; set; } = null!;

    public TimeOnly Starttime { get; set; }

    public TimeOnly Endtime { get; set; }

    public decimal Rateperkwh { get; set; }

    public bool Deleted { get; set; }

    public virtual Tariff Tariff { get; set; } = null!;

    public virtual ICollection<Tariffdetail> Tariffdetails { get; set; } = new List<Tariffdetail>();
}
