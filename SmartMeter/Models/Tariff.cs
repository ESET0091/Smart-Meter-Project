using System;
using System.Collections.Generic;

namespace SmartMeter.Models;

public partial class Tariff
{
    public int Tariffid { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Effectivefrom { get; set; }

    public DateOnly? Effectiveto { get; set; }

    public decimal Baserate { get; set; }

    public decimal Taxrate { get; set; }

    public virtual ICollection<Consumer> Consumers { get; set; } = new List<Consumer>();

    public virtual ICollection<Tariffdetail> Tariffdetails { get; set; } = new List<Tariffdetail>();

    public virtual ICollection<Tariffslab> Tariffslabs { get; set; } = new List<Tariffslab>();

    public virtual ICollection<Todrule> Todrules { get; set; } = new List<Todrule>();
}
