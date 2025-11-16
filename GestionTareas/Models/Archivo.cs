using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class Archivo
{
    public int ArchivoId { get; set; }

    public int? TareaId { get; set; }

    public int? ProyectoId { get; set; }

    public int UsuarioId { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string Ruta { get; set; } = null!;

    public DateTime FechaSubida { get; set; }

    public virtual Proyecto? Proyecto { get; set; }

    public virtual Tarea? Tarea { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
