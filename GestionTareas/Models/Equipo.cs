using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class Equipo
{
    public int EquipoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<EquipoMiembro> EquipoMiembros { get; set; } = new List<EquipoMiembro>();

    public virtual ICollection<ProyectoEquipo> ProyectoEquipos { get; set; } = new List<ProyectoEquipo>();
}
