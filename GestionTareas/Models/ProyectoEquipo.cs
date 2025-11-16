using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class ProyectoEquipo
{
    public int ProyectoEquipoId { get; set; }

    public int ProyectoId { get; set; }

    public int EquipoId { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;

    public virtual Proyecto Proyecto { get; set; } = null!;
}
