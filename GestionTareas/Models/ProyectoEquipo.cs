using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class ProyectoEquipo
{
    public int ProyectoEquipoID { get; set; }

    public int ProyectoID { get; set; }

    public int EquipoID { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;

    public virtual Proyecto Proyecto { get; set; } = null!;
}
