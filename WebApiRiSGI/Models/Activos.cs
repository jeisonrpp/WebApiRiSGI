using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Activos
{
    public int ActivosId { get; set; }

    public string? ActivoPrincipal { get; set; }

    public string? ActivoSecundario { get; set; }

    public string? Serial { get; set; }

    public int TipoActivo { get; set; }

    public string? Descripcion { get; set; }

    public int MarcaActivo { get; set; }

    public int ModeloActivo { get; set; }
    public DateTime FechaCompra { get; set; }
}
