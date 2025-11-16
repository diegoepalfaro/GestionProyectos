using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class Reunion
{
    public int ReunionId { get; set; }

    public int ProyectoId { get; set; }

    public string Titulo { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public string? Notas { get; set; }

    public virtual Proyecto Proyecto { get; set; } = null!;

    public virtual ICollection<ReunionParticipante> ReunionParticipantes { get; set; } = new List<ReunionParticipante>();
}
