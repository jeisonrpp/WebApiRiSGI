using System;
using System.Collections.Generic;

namespace WebApiRiSGI.Models;

public partial class Asignaciones
{
    public int AsigId { get; set; }

    public int ActivosId { get; set; }

    public string DomainUser { get; set; }
    public string DisplayName { get; set; }

    public int LocalidadId { get; set; }

    public int OrganoID { get; set; }

    public int AreaId { get; set; }


    public DateTime FechaAsignacion { get; set; }
}
