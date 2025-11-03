using System;
using System.Collections.Generic;

namespace SmartMeter.Models;

public partial class Tariffslab
{
    public int Tariffslabid { get; set; }

    public int Tariffid { get; set; }

    public decimal Fromkwh { get; set; }

    public decimal Tokwh { get; set; }

    public decimal Rateperkwh { get; set; }

    public bool Deleted { get; set; }

    public virtual Tariff Tariff { get; set; } = null!;

    public virtual ICollection<Tariffdetail> Tariffdetails { get; set; } = new List<Tariffdetail>();
}
