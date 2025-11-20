using GestionTareas.Models;
using GestionTareas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GestionTareas.Controllers
{
    public class TareasController : Controller
    {
        private readonly GestionProyectosDbContext _context;
        private readonly EmailService _email;

        // ÚNICO constructor que usará el contenedor de DI
        public TareasController(GestionProyectosDbContext context, EmailService email)
        {
            _context = context;
            _email = email;
        }

        // ===================== TABLERO POR PROYECTO =====================
        public IActionResult Proyecto(int id)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null) return RedirectToAction("Login", "Usuarios");

            var proyecto = _context.Proyecto.FirstOrDefault(p => p.ProyectoID == id);
            if (proyecto == null) return NotFound();

            var miembro = _context.EquipoMiembro
                .FirstOrDefault(em => em.UsuarioID == usuarioId && em.EquipoID == proyecto.EquipoID);

            if (miembro == null) return Forbid();

            bool esLider = miembro.RolEnEquipo != null &&
                           miembro.RolEnEquipo.ToLower() == "líder";

            ViewBag.EsLider = esLider;
            ViewBag.ProyectoID = id;
            ViewBag.ProyectoNombre = proyecto.Nombre;

            var tareas = _context.Tarea
                .Include(t => t.UsuarioAsignado)   // 👈 cargar usuario asignado
                .Where(t => t.ProyectoID == id)
                .OrderBy(t => t.FechaInicio)
                .ToList();


            ViewBag.UsuariosEquipo = _context.EquipoMiembro
                .Where(em => em.EquipoID == proyecto.EquipoID)
                .Select(em => em.Usuario)
                .ToList();


            return View(tareas);
        }

        // ===================== CREAR TAREA =====================
        [HttpPost]
        public async Task<IActionResult> Crear(int ProyectoID, string Titulo, string Descripcion, string Prioridad, DateTime? FechaFin, int? AsignadoA)
        {
            var tarea = new Tarea
            {
                ProyectoID = ProyectoID,
                Titulo = Titulo,
                Descripcion = Descripcion,
                Prioridad = Prioridad,
                Estado = "Pendiente",
                FechaInicio = DateTime.Now,
                FechaFin = FechaFin,
                AsignadoA = AsignadoA
            };

            _context.Tarea.Add(tarea);
            await _context.SaveChangesAsync();

            // 🔥 Notificar al usuario asignado
            if (AsignadoA.HasValue)
            {
                var usuario = _context.Usuario.FirstOrDefault(u => u.UsuarioID == AsignadoA);
                var proyecto = _context.Proyecto.First(p => p.ProyectoID == ProyectoID);

                await _email.SendEmail(
                    usuario.Email,
                    $"Se te ha asignado una tarea",
                    $"Se te asignó la tarea <b>{tarea.Titulo}</b> del proyecto <b>{proyecto.Nombre}</b>. "
                );
            }

            return RedirectToAction("Proyecto", new { id = ProyectoID });
        }


        // ===================== CAMBIAR ESTADO (botones) =====================
        public IActionResult CambiarEstado(int tareaId, string nuevoEstado)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null) return RedirectToAction("Login", "Usuarios");

            var tarea = _context.Tarea
                .Include(t => t.Proyecto)
                .FirstOrDefault(t => t.TareaID == tareaId);
            if (tarea == null) return NotFound();

            var miembro = _context.EquipoMiembro
                .FirstOrDefault(em => em.UsuarioID == usuarioId && em.EquipoID == tarea.Proyecto.EquipoID);

            if (miembro == null || miembro.RolEnEquipo == null || miembro.RolEnEquipo.ToLower() != "líder")
                return Forbid();

            tarea.Estado = nuevoEstado;
            _context.SaveChanges();

            return RedirectToAction("Proyecto", new { id = tarea.ProyectoID });
        }

        // ===================== CAMBIAR ESTADO (drag & drop / AJAX) =====================
        [HttpPost]
        public async Task<IActionResult> CambiarEstadoAjax(int tareaId, string nuevoEstado)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null) return Unauthorized();

            var tarea = await _context.Tarea
                .Include(t => t.Proyecto)
                .FirstOrDefaultAsync(t => t.TareaID == tareaId);

            if (tarea == null) return NotFound();

            var miembro = await _context.EquipoMiembro
                .FirstOrDefaultAsync(em => em.UsuarioID == usuarioId && em.EquipoID == tarea.Proyecto.EquipoID);

            if (miembro == null || miembro.RolEnEquipo == null || miembro.RolEnEquipo.ToLower() != "líder")
                return Forbid();

            tarea.Estado = nuevoEstado;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // ===================== COMENTARIOS =====================
        public IActionResult Comentarios(int tareaId)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioId == null) return RedirectToAction("Login", "Usuarios");

            var tarea = _context.Tarea
                .Include(t => t.Proyecto)
                .Include(t => t.Comentarios)
                    .ThenInclude(c => c.Usuario)
                .FirstOrDefault(t => t.TareaID == tareaId);

            if (tarea == null) return NotFound();

            bool pertenece = _context.EquipoMiembro
                .Any(em => em.UsuarioID == usuarioId && em.EquipoID == tarea.Proyecto.EquipoID);

            if (!pertenece) return Forbid();

            return View(tarea);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarComentario(int TareaID, string Texto)
        {
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioID == null) return RedirectToAction("Login", "Usuarios");

            if (string.IsNullOrWhiteSpace(Texto))
                return RedirectToAction("Comentarios", new { tareaId = TareaID });

            var comentario = new Comentario
            {
                TareaID = TareaID,
                UsuarioID = usuarioID.Value,
                TextoComentario = Texto,
                FechaComentario = DateTime.Now
            };

            _context.Comentario.Add(comentario);
            await _context.SaveChangesAsync();

            await NotificarComentario(TareaID, usuarioID.Value);

            return RedirectToAction("Comentarios", new { tareaId = TareaID });
        }

        private async Task NotificarComentario(int tareaId, int usuarioAutor)
        {
            var tarea = _context.Tarea
                .Include(t => t.Proyecto)
                .First(t => t.TareaID == tareaId);

            var correos = _context.EquipoMiembro
                .Where(em => em.EquipoID == tarea.Proyecto.EquipoID && em.UsuarioID != usuarioAutor)
                .Select(em => em.Usuario.Email)
                .ToList();

            foreach (var correo in correos)
            {
                await _email.SendEmail(
                    correo,
                    $"Nuevo comentario en la tarea {tarea.Titulo}",
                    $"Se agregó un nuevo comentario en la tarea <b>{tarea.Titulo}</b> del proyecto <b>{tarea.Proyecto.Nombre}</b>."
                );
            }

        }


        [HttpPost]
        public IActionResult ActualizarFechaFin(int TareaID, DateTime FechaFin)
        {
            var tarea = _context.Tarea.FirstOrDefault(t => t.TareaID == TareaID);
            if (tarea == null) return NotFound();

            tarea.FechaFin = FechaFin;
            _context.SaveChanges();

            return RedirectToAction("Proyecto", new { id = tarea.ProyectoID });
        }
    }

}
