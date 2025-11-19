using GestionTareas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace GestionTareas.Controllers
{
    public class ProyectosController : Controller
    {
        private readonly GestionProyectosDbContext _context;

        public ProyectosController(GestionProyectosDbContext context)
        {
            _context = context;
        }

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
                select pr;

            if (!string.IsNullOrEmpty(search))
            {
                string s = search.ToLower();
                proyectosUsuario = proyectosUsuario.Where(p =>
                    p.Nombre.ToLower().Contains(s) ||
                    (p.Descripcion != null && p.Descripcion.ToLower().Contains(s)));
            }

            return View(await proyectosUsuario.OrderBy(p => p.Nombre).ToListAsync());
        }

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

        public async Task<IActionResult> Details(int id)
        {
            var proyecto = await _context.Proyecto.FirstOrDefaultAsync(p => p.ProyectoID == id);
            if (proyecto == null)
                return NotFound();

            return View(proyecto);
        }
    }
}
