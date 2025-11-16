using System;
using System.Collections.Generic;
namespace GestionTareas.Models
{
    public class EventoCalendario
    {
            public int id { get; set; }
            public string title { get; set; } // Nombre del proyecto
            public string start { get; set; } // FechaInicio (formato ISO 8601)
            public string end { get; set; }   // FechaFin (formato ISO 8601). **IMPORTANTE: FullCalendar excluye el día de fin en eventos de día completo.**
            public bool allDay { get; set; } = true;
            public string url { get; set; } = ""; // URL para ver los detalles del proyecto (opcional)
            public string color { get; set; } = "#3a87ad"; // Color del evento
        
    }
}
