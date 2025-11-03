using System;
using System.Collections.Generic;

namespace SmartMeter.Models;

public partial class Address
{
    public long Aid { get; set; }

    public string Houseno { get; set; } = null!;

    public string Lanelocality { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Pincode { get; set; } = null!;

    public virtual ICollection<Consumer> Consumers { get; set; } = new List<Consumer>();
}
