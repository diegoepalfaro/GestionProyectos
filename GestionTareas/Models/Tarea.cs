using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class Tarea
{
    public int TareaId { get; set; }

    public int ProyectoId { get; set; }

    public int? AsignadoA { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Estado { get; set; }

    public string? Prioridad { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public virtual ICollection<Archivo> Archivos { get; set; } = new List<Archivo>();

    public virtual Usuario? AsignadoANavigation { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();

    public virtual Proyecto Proyecto { get; set; } = null!;
}
