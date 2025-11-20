using System.Collections.Generic;

namespace GestionTareas.Models
{
    public class DashboardViewModel
    {
        public int ProyectosPendientes { get; set; }
        public int ProyectosEnProceso { get; set; }
        public int ProyectosCompletados { get; set; }

        public List<EventosDashboard> ProximosEventos { get; set; } = new List<EventosDashboard>();
    }
}
