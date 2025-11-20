using Microsoft.EntityFrameworkCore;

namespace GestionTareas.Models
{
    public class GestionProyectosDbContext : DbContext
    {
        public GestionProyectosDbContext(DbContextOptions<GestionProyectosDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Usuario> Usuario { get; set; } = default!;
        public DbSet<Proyecto> Proyecto { get; set; } = default!;
        public DbSet<Archivo> Archivo { get; set; } = default!;
        public DbSet<Equipo> Equipo { get; set; } = default!;
        public DbSet<EquipoMiembro> EquipoMiembro { get; set; } = default!;
        public DbSet<ProyectoEquipo> ProyectoEquipo { get; set; } = default!;
        public DbSet<Tarea> Tarea { get; set; } = default!;
        public DbSet<Comentario> Comentario { get; set; } = default!;
        public DbSet<Reporte> Reporte { get; set; } = default!;
        public DbSet<Reunion> Reunion { get; set; } = default!;
        public DbSet<ReunionParticipante> ReunionParticipante { get; set; } = default!;
        public DbSet<UsuarioRol> UsuarioRol { get; set; } = default!;
        public DbSet<Rol> Rol { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔥 Mapeos exactos a las tablas de la BD
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Proyecto>().ToTable("Proyectos");
            modelBuilder.Entity<Archivo>().ToTable("Archivos");
            modelBuilder.Entity<Equipo>().ToTable("Equipos");
            modelBuilder.Entity<EquipoMiembro>().ToTable("EquipoMiembros");
            modelBuilder.Entity<ProyectoEquipo>().ToTable("ProyectoEquipos");
            modelBuilder.Entity<Tarea>().ToTable("Tareas");
            modelBuilder.Entity<Comentario>().ToTable("Comentarios");
            modelBuilder.Entity<Reporte>().ToTable("Reportes");
            modelBuilder.Entity<Reunion>().ToTable("Reuniones");
            modelBuilder.Entity<ReunionParticipante>().ToTable("ReunionesParticipantes");
            modelBuilder.Entity<UsuarioRol>().ToTable("UsuarioRoles");
            modelBuilder.Entity<Rol>().ToTable("Roles");

            base.OnModelCreating(modelBuilder);
        }
    }
}
