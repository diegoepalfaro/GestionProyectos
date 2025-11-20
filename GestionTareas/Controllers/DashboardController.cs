using GestionTareas.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.Controllers
{
    public class DashboardController : Controller
    {
        // 💡 CORRECCIÓN IMPORTANTE:
        // Tu DbContext real se llama GESTIONPROYECTOSDBCONTEXT, NO GestionProyectosContext.
        private readonly GestionProyectosDbContext _db;

        // ✔ Inyección correcta del contexto
        public DashboardController(GestionProyectosDbContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            var model = new DashboardViewModel
            {
                ProyectosPendientes = 12,
                ProyectosEnProceso = 7,
                ProyectosCompletados = 18,

                ProximosEventos = new List<EventoDashboardViewModel>
                {
                    new EventoDashboardViewModel { Descripcion = "Reunión de equipo", Fecha = "19/09/2025" },
                    new EventoDashboardViewModel { Descripcion = "Entrega de prototipo", Fecha = "23/09/2025" },
                    new EventoDashboardViewModel { Descripcion = "Presentación al cliente", Fecha = "09/10/2025" }
                }
            };

            return View(model);
        }


        // ================================
        //        VISTA DEL CALENDARIO
        // ================================
        public ActionResult Calendario()
        {
            return View();
        }



        // ========================================
        //   PROYECTOS + TAREAS EN UN SOLO JSON
        // ========================================
        public IActionResult GetProyectosAsEvents()
        {
            // ⭐ PROYECTOS
            var proyectos = _db.Proyecto.ToList();

            var eventosProyectos = proyectos.Select(p => new EventoCalendario
            {
                id = p.ProyectoId,
                title = "[Proyecto] " + p.Nombre,
                start = p.FechaInicio.ToString("yyyy-MM-dd"),
                end = p.FechaFin.HasValue
                        ? p.FechaFin.Value.AddDays(1).ToString("yyyy-MM-dd")
                        : p.FechaInicio.AddDays(1).ToString("yyyy-MM-dd"),
                allDay = true,
                color = GetColorByEstado(p.Estado),
                url = Url.Action("Details", "Proyectos", new { id = p.ProyectoId })
            }).ToList();


            // ⭐ TAREAS
            var tareas = _db.Set<Tarea>().ToList();

            var eventosTareas = tareas.Select(t => new EventoCalendario
            {
                id = t.TareaId + 50000, // evitar choque de IDs con proyectos
                title = "[Tarea] " + t.Titulo,
                start = t.FechaInicio?.ToString("yyyy-MM-dd") ?? "2025-01-01",
                end = t.FechaFin.HasValue
                        ? t.FechaFin.Value.AddDays(1).ToString("yyyy-MM-dd")
                        : t.FechaInicio?.AddDays(1).ToString("yyyy-MM-dd") ?? "2025-01-02",
                allDay = true,
                color = "#8E44AD",  // Morado para diferenciar tareas
                url = Url.Action("Details", "Tareas", new { id = t.TareaId })
            }).ToList();


            // ⭐ UNIR PROYECTOS + TAREAS
            var todosEventos = eventosProyectos.Concat(eventosTareas).ToList();

            return Json(todosEventos);
        }




        // Método auxiliar para asignar un color según estado del proyecto
        private string GetColorByEstado(string estado)
        {
            if (estado == "Completado") return "#4CAF50";
            if (estado == "En Progreso") return "#FFC107";
            if (estado == "Atrasado") return "#F44336";
            return "#3a87ad";
        }
    }
}
