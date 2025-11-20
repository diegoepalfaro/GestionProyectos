using Microsoft.EntityFrameworkCore;
using GestionTareas.Models;
namespace GestionTareas.Models
{
    public class GestionProyectosDbContext: DbContext
    {
        public GestionProyectosDbContext(DbContextOptions options) : base(options) { 
        }
        public DbSet<GestionTareas.Models.Usuario> Usuario { get; set; } = default!;
        public DbSet<GestionTareas.Models.Proyecto> Proyecto { get; set; } = default!;
        public DbSet<GestionTareas.Models.Archivo> Archivo { get; set; } = default!;


        public DbSet<Tarea> Tarea { get; set; } = default!;  // <-- NECESARIO PARA EL CALENDARIO


    }
}
