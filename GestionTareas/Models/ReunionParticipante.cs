using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class ReunionParticipante
{
    public int ReunionParticipanteId { get; set; }

    public int ReunionId { get; set; }

    public int UsuarioId { get; set; }

    public virtual Reunion Reunion { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
