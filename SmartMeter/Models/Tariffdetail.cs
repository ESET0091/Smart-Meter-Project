using System;
using System.Collections.Generic;

namespace SmartMeter.Models;

public partial class Tariffdetail
{
    public long Tariffdetailsid { get; set; }

    public int Tariffid { get; set; }

    public int Tariffslabid { get; set; }

    public int Tarifftodid { get; set; }

    public virtual Tariff Tariff { get; set; } = null!;

    public virtual Tariffslab Tariffslab { get; set; } = null!;

    public virtual Todrule Tarifftod { get; set; } = null!;
}
