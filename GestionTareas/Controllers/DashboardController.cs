using GestionTareas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GestionTareas.Controllers
{
    public class DashboardController : Controller
    {
        private readonly GestionProyectosDbContext db;

        public DashboardController(GestionProyectosDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");

            if (usuarioID == null)
                return RedirectToAction("Login", "Usuarios");

            var usuario = db.Usuario.FirstOrDefault(u => u.UsuarioID == usuarioID);
            ViewBag.NombreUsuario = usuario?.Nombre;

            var equiposUsuario = db.EquipoMiembro
                .Where(x => x.UsuarioID == usuarioID)
                .Select(x => x.EquipoID)
                .ToList();

            var proyectosUsuario = db.Proyecto
                .Where(p => equiposUsuario.Contains(p.EquipoID))
                .ToList();

            // TAREAS PRÓXIMAS POR VENCER
            var proximasTareas = db.Tarea
                .Include(t => t.Proyecto)
                .Where(t => equiposUsuario.Contains(t.Proyecto.EquipoID)
                        && t.Estado != "Completado"
                        && t.FechaFin.HasValue
                        && t.FechaFin > DateTime.Now)
                .OrderBy(t => t.FechaFin)
                .Select(t => new EventosDashboard
                {
                    Descripcion = $"{t.Titulo} — {t.Proyecto.Nombre}",
                    Fecha = t.FechaFin.Value.ToString("yyyy-MM-dd"),
                    Prioridad = t.Prioridad   // <-- Añadido
                })
                .Take(5)
                .ToList();

            var model = new DashboardViewModel
            {
                ProyectosPendientes = proyectosUsuario.Count(p => p.Estado == "Pendiente"),
                ProyectosEnProceso = proyectosUsuario.Count(p => p.Estado == "En Progreso"),
                ProyectosCompletados = proyectosUsuario.Count(p => p.Estado == "Completado"),
                ProximosEventos = proximasTareas     // ahora aparecen las tareas futuras
            };

            return View(model);
        }


        // NUEVA ACCIÓN: listado de proyectos del usuario para elegir de cuál ver tareas
        public IActionResult SeleccionarProyecto()
        {
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioID == null)
                return RedirectToAction("Login", "Usuarios");

            // Equipos a los que pertenece el usuario
            var equiposUsuario = db.EquipoMiembro
                .Where(x => x.UsuarioID == usuarioID)
                .Select(x => x.EquipoID)
                .ToList();

            // Proyectos de esos equipos
            var proyectosUsuario = db.Proyecto
                .Where(p => equiposUsuario.Contains(p.EquipoID))
                .OrderBy(p => p.Nombre)
                .ToList();

            return View(proyectosUsuario);
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
