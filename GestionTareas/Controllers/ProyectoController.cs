using GestionTareas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using GestionTareas.Services;

namespace GestionTareas.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly GestionProyectosDbContext _context;
        private readonly EmailService _email;

        public ProyectosController(GestionProyectosDbContext context, EmailService email)
        {
            _context = context;
            _email = email;
        }

        // ------------------------ LISTA DE PROYECTOS ------------------------
        public async Task<IActionResult> Index(string search)
        {
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioID == null)
                return RedirectToAction("Login", "Usuarios");

            var proyectosUsuario =
                from em in _context.EquipoMiembro
                join eq in _context.Equipo on em.EquipoID equals eq.EquipoID
                join pr in _context.Proyecto on eq.EquipoID equals pr.EquipoID
                where em.UsuarioID == usuarioID
                select new
                {
                    Proyecto = pr,
                    NombreEquipo = eq.Nombre
                };

            if (!string.IsNullOrEmpty(search))
            {
                string s = search.ToLower();
                proyectosUsuario = proyectosUsuario.Where(p =>
                    p.Proyecto.Nombre.ToLower().Contains(s) ||
                    (p.Proyecto.Descripcion != null && p.Proyecto.Descripcion.ToLower().Contains(s)));
            }

            var lista = await proyectosUsuario
                .OrderBy(p => p.Proyecto.Nombre)
                .ToListAsync();

            return View(lista);
        }

        // ------------------------ CREAR PROYECTO ------------------------
        public IActionResult Create()
        {
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioID == null)
                return RedirectToAction("Login", "Usuarios");

            var equiposUsuario =
                from em in _context.EquipoMiembro
                join eq in _context.Equipo on em.EquipoID equals eq.EquipoID
                where em.UsuarioID == usuarioID
                select eq;

            ViewBag.Equipos = new SelectList(equiposUsuario.ToList(), "EquipoID", "Nombre");
            return View("Crear");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proyecto proyecto)
        {
            // 🔥 Forzamos que solo existan estos estados válidos
            if (proyecto.Estado != "Pendiente" &&
                proyecto.Estado != "En Progreso" &&
                proyecto.Estado != "Completado")
            {
                proyecto.Estado = "Pendiente";
            }

            if (ModelState.IsValid)
            {
                _context.Add(proyecto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            var equiposUsuario =
                from em in _context.EquipoMiembro
                join eq in _context.Equipo on em.EquipoID equals eq.EquipoID
                where em.UsuarioID == usuarioID
                select eq;

            ViewBag.Equipos = new SelectList(equiposUsuario.ToList(), "EquipoID", "Nombre");
            return View("Crear", proyecto);
        }

        // ------------------------ DETALLES DE PROYECTO ------------------------
        public async Task<IActionResult> Details(int id)
        {
            var proyecto = await _context.Proyecto.FirstOrDefaultAsync(p => p.ProyectoID == id);
            if (proyecto == null)
                return NotFound();

            return View(proyecto);
        }

        // ------------------------ COMPLETAR PROYECTO ------------------------
        public async Task<IActionResult> Completar(int id)
        {
            var proyecto = _context.Proyecto.Include(p => p.Tareas)
                                            .FirstOrDefault(p => p.ProyectoID == id);

            if (proyecto == null) return NotFound();

            // 1) Cambiar estado del proyecto
            proyecto.Estado = "Completado";
            proyecto.FechaFin = DateTime.Now;

            // 2) Completar todas las tareas
            var tareas = _context.Tarea.Where(t => t.ProyectoID == id).ToList();
            foreach (var t in tareas)
            {
                t.Estado = "Completado";
                t.FechaFin = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            // 3) Notificar por correo a todos los miembros
            await NotificarProyectoCompletado(id);

            return RedirectToAction("Index");
        }

        private async Task NotificarProyectoCompletado(int proyectoId)
        {
            var proyecto = _context.Proyecto.First(p => p.ProyectoID == proyectoId);

            var correos = _context.EquipoMiembro
                .Where(em => em.EquipoID == proyecto.EquipoID)
                .Select(em => em.Usuario.Email)
                .ToList();

            foreach (var correo in correos)
            {
                await _email.SendEmail(
                    correo,
                    $"Proyecto completado: {proyecto.Nombre}",
                    $"El proyecto <b>{proyecto.Nombre}</b> ha sido completado exitosamente.<br/>" +
                    "¡Gracias por tu colaboración!"
                );
            }
        }
    }
}
