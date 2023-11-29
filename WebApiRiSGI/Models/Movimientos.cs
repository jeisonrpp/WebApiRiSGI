using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Movimientos
{
    public int MovimientoId { get; set; }

    public int ActivoId { get; set; }

    public int LocalidadId { get; set; }

    public int AreaId { get; set; }

    public string UsuarioDestino { get; set; } = null!;

    public string? Observacion { get; set; }

    public string UsuarioRemitente { get; set; } = null!;

    public DateTime Fecha { get; set; }
}
