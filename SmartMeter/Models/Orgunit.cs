using System;
using System.Collections.Generic;

namespace SmartMeter.Models;

public partial class Orgunit
{
    public int Orgunitid { get; set; }

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? Parentid { get; set; }

    public virtual ICollection<Consumer> Consumers { get; set; } = new List<Consumer>();

    public virtual ICollection<Orgunit> InverseParent { get; set; } = new List<Orgunit>();

    public virtual Orgunit? Parent { get; set; }
}
