using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Activos
{
    public int ActivosId { get; set; }

    public string? ActivoPrincipal { get; set; }

    public string? ActivoSecundario { get; set; }

    public string? Serial { get; set; }

    public string? TipoActivo { get; set; }

    public string? Descripcion { get; set; }

    public string? MarcaActivo { get; set; }

    public string? ModeloActivo { get; set; }
}
