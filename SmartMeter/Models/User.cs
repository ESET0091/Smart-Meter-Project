using System;
using System.Collections.Generic;

namespace SmartMeter.Models;

public partial class User
{
    public long Userid { get; set; }

    public string Username { get; set; } = null!;

    public byte[] Passwordhash { get; set; } = null!;

    public string Displayname { get; set; } = null!;

    public string? Profilepic { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Roles { get; set; }

    public DateTime? Lastloginutc { get; set; } = DateTime.UtcNow;

    public bool Isactive { get; set; }
}
