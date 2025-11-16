using GestionTareas.Models; // Tu modelo de datos
using Microsoft.AspNetCore.Mvc; // Para el controlador
using Microsoft.EntityFrameworkCore; // Para los métodos asíncronos de EF Core
using System.Linq;
using System.Threading.Tasks;

namespace GestionTareas.Controllers
{
    public class ProyectosController : Controller // Hereda de Controller de Core
    {
        // 1. Campo privado para el DbContext
        private readonly GestionProyectosDbContext _context;

        // 2. Inyección de Dependencias (Constructor)
        // El framework crea e inyecta la instancia de GestionProyectosContext
        public ProyectosController(GestionProyectosDbContext context)
        {
            _context = context;
        }

        // GET: Proyectos
        // IActionResult es el tipo de retorno estándar en Core
        public async Task<IActionResult> Index(string search)
        {
            // Usa _context en lugar de db
            var proyectos = _context.Proyecto.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                // Usamos ToLower() para búsquedas case-insensitive
                string searchLower = search.ToLower();
                proyectos = proyectos.Where(p =>
                    (p.Nombre != null && p.Nombre.ToLower().Contains(searchLower)) ||
                    (p.Descripcion != null && p.Descripcion.ToLower().Contains(searchLower))
                );
            }

            // Usamos ToListAsync() para operaciones asíncronas
            var listaProyectos = await proyectos.OrderBy(p => p.Nombre).ToListAsync();

            return View(listaProyectos);
        }

        // GET: Proyectos/Create
        public IActionResult Create()
        {
            return View("Crear");
        }

        // POST: Proyectos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Se recomienda usar Task<IActionResult> para operaciones de escritura
        public async Task<IActionResult> Create(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proyecto); // Método de Add en Core
                await _context.SaveChangesAsync(); // Guardado asíncrono
                return RedirectToAction(nameof(Index)); // Uso de nameof para seguridad de tipo
            }
            return View("Crear",proyecto);
        }

        // GET: Proyectos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Reemplaza HttpStatusCodeResult por NotFound() o BadRequest()
            if (id == null) return NotFound();

            // Reemplaza db.Proyectos.Find() por SingleOrDefaultAsync() o FindAsync()
            var proyecto = await _context.Proyecto.FirstOrDefaultAsync(m => m.ProyectoId== id);

            if (proyecto == null) return NotFound(); // Reemplaza HttpNotFound()

            return View(proyecto);
        }

        // Nota: La implementación de Dispose no es necesaria en el controlador de Core,
        // ya que la Inyección de Dependencias gestiona el ciclo de vida del DbContext.
    }
}