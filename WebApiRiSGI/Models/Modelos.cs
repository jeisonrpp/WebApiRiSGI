using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Modelos
{
    public int ModeloId { get; set; }

    public int MarcaId { get; set; }

    public string Modelo1 { get; set; } = null!;

    public int Tipoid { get; set; }
}
