using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Usuarios
{
    public int UserId { get; set; }

    public string UserLogin { get; set; } = null!;

    public string? UserPass { get; set; }

    public int AreaId { get; set; }

    public string UserName { get; set; } = null!;
}
