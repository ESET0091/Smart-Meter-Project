using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace SmartMeter.Models;

public partial class User
{
    public long Userid { get; set; }

    public string Username { get; set; } = null!;

    public byte[] Passwordhash { get; set; } = null!;


    public string Displayname { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? Lastloginutc { get; set; } = DateTime.UtcNow;

    public bool Isactive { get; set; }
    public string Roles { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiry { get; set; } = DateTime.UtcNow;



}
