using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Areas
{
    public int AreaId { get; set; }
    public string AreaNombre { get; set; }

    public int DepartamentoId { get; set; }

    public string? AreaEncargado { get; set; }
}
