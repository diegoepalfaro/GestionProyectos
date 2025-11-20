using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionTareas.Models;
using Microsoft.AspNetCore.Http;
using GestionTareas.Services;


namespace GestionTareas.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly GestionProyectosDbContext _context;

        public UsuariosController(GestionProyectosDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UsuarioID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Nombre, string Email, string Telefono, string Direccion, string Contraseña, string ConfirmPassword,
                                        [FromServices] EmailService emailService)
        {
            if (Contraseña != ConfirmPassword)
            {
                ModelState.AddModelError("", "Las contraseñas no coinciden.");
                return View();
            }

            if (_context.Usuario.Any(u => u.Email == Email))
            {
                ModelState.AddModelError("", "El correo ya está registrado.");
                return View();
            }

            Usuario usuario = new Usuario
            {
                Nombre = Nombre,
                Email = Email,
                Telefono = Telefono,
                Direccion = Direccion,
                Contraseña = Contraseña,
                Estado = true
            };

            _context.Add(usuario);
            await _context.SaveChangesAsync();

            // Enviar correo de bienvenida
            string asunto = "¡Bienvenido a la Plataforma de Gestión de Proyectos!";
            string cuerpo = $@"
            <h2>Hola {Nombre}, ¡Bienvenido!</h2>
            <p>Tu registro se ha completado exitosamente.</p>
            <p>Ahora puedes gestionar tus proyectos, tareas y equipos dentro de nuestra plataforma.</p>
            <br/>
            <p>Nos alegra tenerte con nosotros 👋</p>
        ";

            await emailService.SendEmail(Email, asunto, cuerpo);

            // Login automático
            HttpContext.Session.SetInt32("UsuarioID", usuario.UsuarioID);

            return RedirectToAction("Index", "Dashboard");
        }


        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkID=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioID,Nombre,Email,Telefono,Direccion,Contraseña,Estado")] Usuario usuario)
        {
            if (id != usuario.UsuarioID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.UsuarioID == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet] // Opcional, pero buena práctica para las vistas que solo muestran la página
        public IActionResult Login()
        {
            // Devuelve la vista que está en /Views/Usuarios/Login.cshtml
            return View();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.UsuarioID == id);
        }

        [HttpPost]
        public IActionResult Login(string Email, string Contraseña)
        {
            var usuario = _context.Usuario
                .FirstOrDefault(u => u.Email == Email && u.Contraseña == Contraseña);

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos";
                return View("Login");
            }

            HttpContext.Session.SetInt32("UsuarioID", usuario.UsuarioID);
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuarios");
        }



    }
}
