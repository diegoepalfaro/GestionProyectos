using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class Proyecto
{
    public int ProyectoID { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string? Estado { get; set; }

    public int EquipoID { get; set; }


    public virtual ICollection<Archivo> Archivos { get; set; } = new List<Archivo>();

    public virtual ICollection<ProyectoEquipo> ProyectoEquipos { get; set; } = new List<ProyectoEquipo>();

    public virtual ICollection<Reporte> Reportes { get; set; } = new List<Reporte>();

    public virtual ICollection<Reunion> Reunions { get; set; } = new List<Reunion>();

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
