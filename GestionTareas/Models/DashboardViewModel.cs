using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;

namespace GestionTareas.Models
{
    // Clase auxiliar para un evento
    public class EventoDashboardViewModel
    {
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
    }

    // Modelo principal que alimenta la vista del Dashboard
    public class DashboardViewModel
    {
        public int ProyectosPendientes { get; set; } // Contador 1
        public int ProyectosEnProceso { get; set; }  // Contador 2
        public int ProyectosCompletados { get; set; } // Contador 3

        public List<EventoDashboardViewModel> ProximosEventos { get; set; }

        public DashboardViewModel()
        {
            ProximosEventos = new List<EventoDashboardViewModel>();
        }
    }
}