using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class Archivo
{
    public int ArchivoID { get; set; }

    public int? TareaID { get; set; }

    public int? ProyectoID { get; set; }

    public int UsuarioID { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string Ruta { get; set; } = null!;

    public DateTime FechaSubida { get; set; }

    public virtual Proyecto? Proyecto { get; set; }

    public virtual Tarea? Tarea { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
