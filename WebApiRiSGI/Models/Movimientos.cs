using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Movimientos
{
    public int MovimientoId { get; set; }

    public string Movimiento { get; set; }

    public int ActivoId { get; set; }

    public int LocalidadId_Destino { get; set; }

    public int AreaId_Destino { get; set; }

    public string UsuarioDestino { get; set; } = null!;

    public int LocalidadId_Remitente { get; set; }

    public int AreaId_Remitente { get; set; }

    public string? Observacion { get; set; }

    public string UsuarioRemitente { get; set; } = null!;
    public string MovimientoTipo { get; set; }

    public DateTime Fecha { get; set; }
}
