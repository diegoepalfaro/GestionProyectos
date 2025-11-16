using System;
using System.Collections.Generic;

namespace GestionTareas.Models;

public partial class Reporte
{
    public int ReporteId { get; set; }

    public int ProyectoId { get; set; }

    public int UsuarioId { get; set; }

    public string? TipoReporte { get; set; }

    public DateTime FechaGeneracion { get; set; }

    public string? Detalles { get; set; }

    public virtual Proyecto Proyecto { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
