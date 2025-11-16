using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionTareas.Models;

namespace GestionTareas.Controllers
{
    public class ArchivoesController : Controller
    {
        private readonly GestionProyectosDbContext _context;

        public ArchivoesController(GestionProyectosDbContext context)
        {
            _context = context;
        }

        // GET: Archivoes
        public async Task<IActionResult> Index()
        {
            var gestionProyectosDbContext = _context.Archivo.Include(a => a.Proyecto).Include(a => a.Tarea).Include(a => a.Usuario);
            return View(await gestionProyectosDbContext.ToListAsync());
        }

        // GET: Archivoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archivo = await _context.Archivo
                .Include(a => a.Proyecto)
                .Include(a => a.Tarea)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.ArchivoId == id);
            if (archivo == null)
            {
                return NotFound();
            }

            return View(archivo);
        }

        // GET: Archivoes/Create
        public IActionResult Create()
        {
            ViewData["ProyectoId"] = new SelectList(_context.Proyecto, "ProyectoId", "ProyectoId");
            ViewData["TareaId"] = new SelectList(_context.Set<Tarea>(), "TareaId", "TareaId");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "UsuarioId");
            return View();
        }

        // POST: Archivoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArchivoId,TareaId,ProyectoId,UsuarioId,NombreArchivo,Ruta,FechaSubida")] Archivo archivo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(archivo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProyectoId"] = new SelectList(_context.Proyecto, "ProyectoId", "ProyectoId", archivo.ProyectoId);
            ViewData["TareaId"] = new SelectList(_context.Set<Tarea>(), "TareaId", "TareaId", archivo.TareaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "UsuarioId", archivo.UsuarioId);
            return View(archivo);
        }

        // GET: Archivoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archivo = await _context.Archivo.FindAsync(id);
            if (archivo == null)
            {
                return NotFound();
            }
            ViewData["ProyectoId"] = new SelectList(_context.Proyecto, "ProyectoId", "ProyectoId", archivo.ProyectoId);
            ViewData["TareaId"] = new SelectList(_context.Set<Tarea>(), "TareaId", "TareaId", archivo.TareaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "UsuarioId", archivo.UsuarioId);
            return View(archivo);
        }

        // POST: Archivoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArchivoId,TareaId,ProyectoId,UsuarioId,NombreArchivo,Ruta,FechaSubida")] Archivo archivo)
        {
            if (id != archivo.ArchivoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(archivo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArchivoExists(archivo.ArchivoId))
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
            ViewData["ProyectoId"] = new SelectList(_context.Proyecto, "ProyectoId", "ProyectoId", archivo.ProyectoId);
            ViewData["TareaId"] = new SelectList(_context.Set<Tarea>(), "TareaId", "TareaId", archivo.TareaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioId", "UsuarioId", archivo.UsuarioId);
            return View(archivo);
        }

        // GET: Archivoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var archivo = await _context.Archivo
                .Include(a => a.Proyecto)
                .Include(a => a.Tarea)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.ArchivoId == id);
            if (archivo == null)
            {
                return NotFound();
            }

            return View(archivo);
        }

        // POST: Archivoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var archivo = await _context.Archivo.FindAsync(id);
            if (archivo != null)
            {
                _context.Archivo.Remove(archivo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArchivoExists(int id)
        {
            return _context.Archivo.Any(e => e.ArchivoId == id);
        }
    }
}
