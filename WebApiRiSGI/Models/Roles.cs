using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Roles
{
    public int RoliD { get; set; }

    public string RolName { get; set; } = null!;

    public bool RolActivo { get; set; }
}
