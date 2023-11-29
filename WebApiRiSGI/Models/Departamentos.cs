using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Departamentos
{
    public int DepartamentoId { get; set; }

    public string Departamento1 { get; set; } = null!;

    public int OrganoId { get; set; }

    public int LocalidadId { get; set; }
}
