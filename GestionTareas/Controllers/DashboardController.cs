// Archivo: Controllers/DashboardController.cs
using GestionTareas.Models;
//using GestionTareas.ViewModels; // Necesitas esta referencia
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
                return RedirectToAction("Login", "Usuarios");

            var usuario = db.Usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);
            ViewBag.NombreUsuario = usuario?.Nombre;

            // Obtener todos los equipos del usuario
            var equiposUsuario = db.EquipoMiembros
                .Where(x => x.UsuarioId == usuarioId)
                .Select(x => x.EquipoId)
                .ToList();

            // Obtener todos los proyectos en los que participa el usuario
            var proyectosUsuario = db.ProyectoEquipos
                .Where(x => equiposUsuario.Contains(x.EquipoId))
                .Select(x => x.Proyecto)
                .ToList();

            var model = new DashboardViewModel
            {
                ProyectosPendientes = proyectosUsuario.Count(p => p.Estado == "Pendiente"),
                ProyectosEnProceso = proyectosUsuario.Count(p => p.Estado == "En Progreso"),
                ProyectosCompletados = proyectosUsuario.Count(p => p.Estado == "Completado")
            };

            return View(model);
        }

        private GestionProyectosContext db = new GestionProyectosContext(); // Reemplaza 'YourDbContext' con tu contexto de base de datos real

        // Acción principal para la vista de calendario
        public ActionResult Calendario()
        {
            return View();
        }

        // Acción que devuelve los datos de los proyectos en formato JSON para FullCalendar
        public IActionResult GetProyectosAsEvents()
        {
            // 1. Obtener los proyectos de tu base de datos
            var proyectos = db.Proyectos.ToList();

            // 2. Mapear los Proyectos a la estructura de CalendarEvent
            var eventos = proyectos.Select(p => new EventoCalendario
            {
                id = p.ProyectoId,
                title = p.Nombre,
                // Convertir DateTime a formato ISO 8601 (necesario para FullCalendar)
                start = p.FechaInicio.ToString("yyyy-MM-dd"),
                // Para eventos de día completo, FullCalendar espera que la fecha final sea **el día después** del final real.
                // Si FechaFin es nula, usamos FechaInicio. Si no, le sumamos un día.
                end = p.FechaFin.HasValue ? p.FechaFin.Value.AddDays(1).ToString("yyyy-MM-dd") : p.FechaInicio.AddDays(1).ToString("yyyy-MM-dd"),
                allDay = true,
                // Establecer color basado en el estado (opcional)
                color = GetColorByEstado(p.Estado),
                // Crear una URL para que el evento sea clickeable (ej. redirigir a la vista de detalles del proyecto)
                url = Url.Action("Details", "Proyectos", new { id = p.ProyectoId })
            }).ToList();

            // 3. Devolver los eventos como JSON
            return Json(eventos);
        }

        // Método auxiliar para asignar un color
        private string GetColorByEstado(string estado)
        {
            if (estado == "Completado") return "#4CAF50"; // Verde
            if (estado == "En Progreso") return "#FFC107"; // Amarillo
            if (estado == "Atrasado") return "#F44336"; // Rojo
            return "#3a87ad"; // Azul por defecto
        }
    }
}