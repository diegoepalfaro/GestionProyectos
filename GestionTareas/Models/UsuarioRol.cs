using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class UsuarioRol
{
    public int UsuarioRolID { get; set; }

    public int UsuarioID { get; set; }

    public int RolID { get; set; }

    public virtual Rol Rol { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
