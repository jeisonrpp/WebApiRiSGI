using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Descargos
{
    public int DescargoId { get; set; }

    public int? ActivoId { get; set; }

    public int? LocalidadId { get; set; }

    public int? AreaId { get; set; }

    public string? Observacion { get; set; }

    public string? UsuarioRemitente { get; set; }

    public DateTime? Fecha { get; set; }
}
