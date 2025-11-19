using GestionTareas.Models;
using GestionTareas.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas.Controllers
{
    public class EquiposController : Controller
    {
        private readonly GestionProyectosDbContext _context;

        public EquiposController(GestionProyectosDbContext context)
        {
            _context = context;
        }

        // LISTAR EQUIPOS
        public IActionResult Index()
        {
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioID == null)
                return RedirectToAction("Login", "Usuarios");

            // Equipos donde el usuario es miembro
            var equipos = (from em in _context.EquipoMiembro
                           join eq in _context.Equipo on em.EquipoID equals eq.EquipoID
                           where em.UsuarioID == usuarioID
                           select eq).ToList();

            return View(equipos);
        }

        // FORM PARA CREAR EQUIPO
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Nombre, string Descripcion)
        {
            // 1. Crear el equipo
            var equipo = new Equipo
            {
                Nombre = Nombre,
                Descripcion = Descripcion
            };

            _context.Add(equipo);
            await _context.SaveChangesAsync();

            // 2. Obtener usuario en sesión (el creador)
            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioID != null)
            {
                _context.EquipoMiembro.Add(new EquipoMiembro
                {
                    EquipoID = equipo.EquipoID,
                    UsuarioID = usuarioID.Value,
                    RolEnEquipo = "Líder"
                });

                await _context.SaveChangesAsync();
            }

            // 3. Redirigir a agregar miembros
            return RedirectToAction("AgregarMiembros", new { id = equipo.EquipoID });
        }



        // VISTA PARA AGREGAR USUARIOS
        public IActionResult AgregarMiembros(int id)
        {
            var equipo = _context.Equipo.FirstOrDefault(e => e.EquipoID == id);
            if (equipo == null) return NotFound();

            int? usuarioID = HttpContext.Session.GetInt32("UsuarioID");
            if (usuarioID == null) return RedirectToAction("Login", "Usuarios");

            // Obtener miembros del equipo
            var miembros = (from em in _context.EquipoMiembro
                            join u in _context.Usuario on em.UsuarioID equals u.UsuarioID
                            where em.EquipoID == id
                            select new
                            {
                                em.EquipoMiembroID,
                                u.UsuarioID,
                                u.Nombre,
                                u.Email,
                                em.RolEnEquipo
                            }).ToList();

            ViewBag.EsLider = miembros.Any(m => m.UsuarioID == usuarioID && m.RolEnEquipo == "Líder");
            ViewBag.EquipoID = id;
            ViewBag.NombreEquipo = equipo.Nombre;
            ViewBag.Miembros = miembros;

            return View();
        }

        [HttpPost]
        public IActionResult CambiarRol(int EquipoMiembroID)
        {
            var miembro = _context.EquipoMiembro.FirstOrDefault(m => m.EquipoMiembroID == EquipoMiembroID);
            if (miembro == null) return NotFound();

            int equipoID = miembro.EquipoID;

            // Obtener todos los líderes del equipo (evitar quedarse sin líder)
            bool esLider = miembro.RolEnEquipo == "Líder";
            int totalLideres = _context.EquipoMiembro.Count(m => m.EquipoID == equipoID && m.RolEnEquipo == "Líder");

            if (esLider && totalLideres == 1)
            {
                TempData["Error"] = "El equipo no puede quedarse sin líder.";
                return RedirectToAction("AgregarMiembros", new { id = equipoID });
            }

            miembro.RolEnEquipo = esLider ? "Miembro" : "Líder";
            _context.SaveChanges();

            TempData["Success"] = "Rol actualizado correctamente.";
            return RedirectToAction("AgregarMiembros", new { id = equipoID });
        }


        // BUSCAR USUARIO POR CORREO Y AGREGARLO AL EQUIPO
        [HttpPost]
        public async Task<IActionResult> AgregarMiembro(int EquipoID, string Email, [FromServices] EmailService emailService)
        {
            var equipo = _context.Equipo.FirstOrDefault(e => e.EquipoID == EquipoID);
            if (equipo == null) return NotFound();

            var usuario = _context.Usuario.FirstOrDefault(u => u.Email == Email);
            if (usuario == null)
            {
                TempData["Error"] = "No existe ningún usuario con ese correo.";
                return RedirectToAction("AgregarMiembros", new { id = EquipoID });
            }

            // Verificar si ya es miembro
            bool existe = _context.EquipoMiembro.Any(x => x.EquipoID == EquipoID && x.UsuarioID == usuario.UsuarioID);
            if (existe)
            {
                TempData["Error"] = "Ese usuario ya pertenece al equipo.";
                return RedirectToAction("AgregarMiembros", new { id = EquipoID });
            }

            // Agregar
            _context.EquipoMiembro.Add(new EquipoMiembro
            {
                EquipoID = EquipoID,
                UsuarioID = usuario.UsuarioID,
                RolEnEquipo = "Miembro"
            });
            await _context.SaveChangesAsync();

            // ENVIAR CORREO NOTIFICANDO LA INVITACIÓN
            string asunto = "Has sido agregado a un equipo de proyecto";
            string mensaje = $@"
                <h2>¡Hola {usuario.Nombre}!</h2>
                <p>Has sido agregado al equipo <strong>{equipo.Nombre}</strong>.</p>
                <p>Ahora puedes colaborar con los demás integrantes dentro de la plataforma.</p>
                <br/><p>Saludos 👋</p>
            ";

            await emailService.SendEmail(usuario.Email, asunto, mensaje);

            TempData["Success"] = "Usuario añadido correctamente y notificado.";
            return RedirectToAction("AgregarMiembros", new { id = EquipoID });
        }
    }
}
