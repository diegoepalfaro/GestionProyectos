using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class EquipoMiembro
{
    public int EquipoMiembroId { get; set; }

    public int EquipoId { get; set; }

    public int UsuarioId { get; set; }

    public string? RolEnEquipo { get; set; }

    public virtual Equipo Equipo { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
