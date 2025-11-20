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

        public IActionResult Calendario()
        {
            return View();
        }

        public IActionResult GetProyectosAsEvents()
        {
            var proyectos = db.Proyecto.ToList();

            var eventos = proyectos.Select(p => new
            {
                id = p.ProyectoID,
                title = p.Nombre,
                start = p.FechaInicio.ToString("yyyy-MM-dd"),
                end = p.FechaFin.HasValue ? p.FechaFin.Value.AddDays(1).ToString("yyyy-MM-dd") : p.FechaInicio.AddDays(1).ToString("yyyy-MM-dd"),
                allDay = true,
                url = Url.Action("Details", "Proyectos", new { id = p.ProyectoID })
            });

            return Json(eventos);
        }
    }
}
