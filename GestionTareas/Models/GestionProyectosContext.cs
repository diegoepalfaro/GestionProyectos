using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas.Models;

public partial class GestionProyectosContext : DbContext
{
    public GestionProyectosContext()
    {
    }

    public GestionProyectosContext(DbContextOptions<GestionProyectosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Archivo> Archivos { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Equipo> Equipos { get; set; }

    public virtual DbSet<EquipoMiembro> EquipoMiembros { get; set; }

    public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    public virtual DbSet<ProyectoEquipo> ProyectoEquipos { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<Reunion> Reunions { get; set; }

    public virtual DbSet<ReunionParticipante> ReunionParticipantes { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioRol> UsuarioRols { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source = DIEGO; Initial Catalog = GestionProyectos; User Id = sa; Password = Catolica10; Encrypt = False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Archivo>(entity =>
        {
            entity.HasKey(e => e.ArchivoId).HasName("PK_dbo.Archivo");

            entity.ToTable("Archivo");

            entity.HasIndex(e => e.ProyectoId, "IX_ProyectoID");

            entity.HasIndex(e => e.TareaId, "IX_TareaID");

            entity.HasIndex(e => e.UsuarioId, "IX_UsuarioID");

            entity.Property(e => e.ArchivoId).HasColumnName("ArchivoID");
            entity.Property(e => e.FechaSubida).HasColumnType("datetime");
            entity.Property(e => e.NombreArchivo).HasMaxLength(200);
            entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");
            entity.Property(e => e.TareaId).HasColumnName("TareaID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Proyecto).WithMany(p => p.Archivos)
                .HasForeignKey(d => d.ProyectoId)
                .HasConstraintName("FK_dbo.Archivo_dbo.Proyecto_ProyectoID");

            entity.HasOne(d => d.Tarea).WithMany(p => p.Archivos)
                .HasForeignKey(d => d.TareaId)
                .HasConstraintName("FK_dbo.Archivo_dbo.Tarea_TareaID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Archivos)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_dbo.Archivo_dbo.Usuario_UsuarioID");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.ComentarioId).HasName("PK_dbo.Comentario");

            entity.ToTable("Comentario");

            entity.HasIndex(e => e.TareaId, "IX_TareaID");

            entity.HasIndex(e => e.UsuarioId, "IX_UsuarioID");

            entity.Property(e => e.ComentarioId).HasColumnName("ComentarioID");
            entity.Property(e => e.FechaComentario).HasColumnType("datetime");
            entity.Property(e => e.TareaId).HasColumnName("TareaID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Tarea).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.TareaId)
                .HasConstraintName("FK_dbo.Comentario_dbo.Tarea_TareaID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_dbo.Comentario_dbo.Usuario_UsuarioID");
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.EquipoId).HasName("PK_dbo.Equipo");

            entity.ToTable("Equipo");

            entity.Property(e => e.EquipoId).HasColumnName("EquipoID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<EquipoMiembro>(entity =>
        {
            entity.HasKey(e => e.EquipoMiembroId).HasName("PK_dbo.EquipoMiembro");

            entity.ToTable("EquipoMiembro");

            entity.HasIndex(e => e.EquipoId, "IX_EquipoID");

            entity.HasIndex(e => e.UsuarioId, "IX_UsuarioID");

            entity.Property(e => e.EquipoMiembroId).HasColumnName("EquipoMiembroID");
            entity.Property(e => e.EquipoId).HasColumnName("EquipoID");
            entity.Property(e => e.RolEnEquipo).HasMaxLength(100);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Equipo).WithMany(p => p.EquipoMiembros)
                .HasForeignKey(d => d.EquipoId)
                .HasConstraintName("FK_dbo.EquipoMiembro_dbo.Equipo_EquipoID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.EquipoMiembros)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_dbo.EquipoMiembro_dbo.Usuario_UsuarioID");
        });

        modelBuilder.Entity<MigrationHistory>(entity =>
        {
            entity.HasKey(e => new { e.MigrationId, e.ContextKey }).HasName("PK_dbo.__MigrationHistory");

            entity.ToTable("__MigrationHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ContextKey).HasMaxLength(300);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.ProyectoId).HasName("PK_dbo.Proyecto");

            entity.ToTable("Proyecto");

            entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(200);
        });

        modelBuilder.Entity<ProyectoEquipo>(entity =>
        {
            entity.HasKey(e => e.ProyectoEquipoId).HasName("PK_dbo.ProyectoEquipo");

            entity.ToTable("ProyectoEquipo");

            entity.HasIndex(e => e.EquipoId, "IX_EquipoID");

            entity.HasIndex(e => e.ProyectoId, "IX_ProyectoID");

            entity.Property(e => e.ProyectoEquipoId).HasColumnName("ProyectoEquipoID");
            entity.Property(e => e.EquipoId).HasColumnName("EquipoID");
            entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");

            entity.HasOne(d => d.Equipo).WithMany(p => p.ProyectoEquipos)
                .HasForeignKey(d => d.EquipoId)
                .HasConstraintName("FK_dbo.ProyectoEquipo_dbo.Equipo_EquipoID");

            entity.HasOne(d => d.Proyecto).WithMany(p => p.ProyectoEquipos)
                .HasForeignKey(d => d.ProyectoId)
                .HasConstraintName("FK_dbo.ProyectoEquipo_dbo.Proyecto_ProyectoID");
        });

        modelBuilder.Entity<Reporte>(entity =>
        {
            entity.HasKey(e => e.ReporteId).HasName("PK_dbo.Reporte");

            entity.ToTable("Reporte");

            entity.HasIndex(e => e.ProyectoId, "IX_ProyectoID");

            entity.HasIndex(e => e.UsuarioId, "IX_UsuarioID");

            entity.Property(e => e.ReporteId).HasColumnName("ReporteID");
            entity.Property(e => e.FechaGeneracion).HasColumnType("datetime");
            entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");
            entity.Property(e => e.TipoReporte).HasMaxLength(100);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Proyecto).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.ProyectoId)
                .HasConstraintName("FK_dbo.Reporte_dbo.Proyecto_ProyectoID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_dbo.Reporte_dbo.Usuario_UsuarioID");
        });

        modelBuilder.Entity<Reunion>(entity =>
        {
            entity.HasKey(e => e.ReunionId).HasName("PK_dbo.Reunion");

            entity.ToTable("Reunion");

            entity.HasIndex(e => e.ProyectoId, "IX_ProyectoID");

            entity.Property(e => e.ReunionId).HasColumnName("ReunionID");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.Proyecto).WithMany(p => p.Reunions)
                .HasForeignKey(d => d.ProyectoId)
                .HasConstraintName("FK_dbo.Reunion_dbo.Proyecto_ProyectoID");
        });

        modelBuilder.Entity<ReunionParticipante>(entity =>
        {
            entity.HasKey(e => e.ReunionParticipanteId).HasName("PK_dbo.ReunionParticipante");

            entity.ToTable("ReunionParticipante");

            entity.HasIndex(e => e.ReunionId, "IX_ReunionID");

            entity.HasIndex(e => e.UsuarioId, "IX_UsuarioID");

            entity.Property(e => e.ReunionParticipanteId).HasColumnName("ReunionParticipanteID");
            entity.Property(e => e.ReunionId).HasColumnName("ReunionID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Reunion).WithMany(p => p.ReunionParticipantes)
                .HasForeignKey(d => d.ReunionId)
                .HasConstraintName("FK_dbo.ReunionParticipante_dbo.Reunion_ReunionID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ReunionParticipantes)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_dbo.ReunionParticipante_dbo.Usuario_UsuarioID");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK_dbo.Rol");

            entity.ToTable("Rol");

            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.TareaId).HasName("PK_dbo.Tarea");

            entity.ToTable("Tarea");

            entity.HasIndex(e => e.AsignadoA, "IX_AsignadoA");

            entity.HasIndex(e => e.ProyectoId, "IX_ProyectoID");

            entity.Property(e => e.TareaId).HasColumnName("TareaID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.Prioridad).HasMaxLength(20);
            entity.Property(e => e.ProyectoId).HasColumnName("ProyectoID");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.AsignadoANavigation).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.AsignadoA)
                .HasConstraintName("FK_dbo.Tarea_dbo.Usuario_AsignadoA");

            entity.HasOne(d => d.Proyecto).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.ProyectoId)
                .HasConstraintName("FK_dbo.Tarea_dbo.Proyecto_ProyectoID");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK_dbo.Usuario");

            entity.ToTable("Usuario");

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Contraseña).HasMaxLength(255);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<UsuarioRol>(entity =>
        {
            entity.HasKey(e => e.UsuarioRolId).HasName("PK_dbo.UsuarioRol");

            entity.ToTable("UsuarioRol");

            entity.HasIndex(e => e.RolId, "IX_RolID");

            entity.HasIndex(e => e.UsuarioId, "IX_UsuarioID");

            entity.Property(e => e.UsuarioRolId).HasColumnName("UsuarioRolID");
            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK_dbo.UsuarioRol_dbo.Rol_RolID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK_dbo.UsuarioRol_dbo.Usuario_UsuarioID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
