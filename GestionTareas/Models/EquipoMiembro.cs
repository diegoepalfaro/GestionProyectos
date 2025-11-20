using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class EquipoMiembro
{
    public int EquipoMiembroID { get; set; }

    public int EquipoID { get; set; }

    public int UsuarioID { get; set; }

    public string? RolEnEquipo { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
