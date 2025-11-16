using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class Comentario
{
    public int ComentarioId { get; set; }

    public int TareaId { get; set; }

    public int UsuarioId { get; set; }

    public string ComentarioTexto { get; set; } = null!;

    public DateTime FechaComentario { get; set; }

    public virtual Tarea Tarea { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
