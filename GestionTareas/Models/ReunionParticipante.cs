using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class ReunionParticipante
{
    public int ReunionParticipanteID { get; set; }

    public int ReunionID { get; set; }

    public int UsuarioID { get; set; }

    public virtual Reunion Reunion { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
