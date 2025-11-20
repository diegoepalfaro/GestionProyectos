using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionTareas.Models;

public partial class Comentario
{
    public int ComentarioID { get; set; }

    public int TareaID { get; set; }

    public int UsuarioID { get; set; }

    [Column("Comentario")]   //Mapea la propiedad al nombre real de SQL
    public string TextoComentario { get; set; } = null!;

    public DateTime FechaComentario { get; set; }

    public virtual Tarea Tarea { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
