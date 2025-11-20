using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionTareas.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    EquipoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.EquipoID);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    ProyectoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.ProyectoID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolID);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "ProyectoEquipos",
                columns: table => new
                {
                    ProyectoEquipoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoID = table.Column<int>(type: "int", nullable: false),
                    EquipoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProyectoEquipos", x => x.ProyectoEquipoID);
                    table.ForeignKey(
                        name: "FK_ProyectoEquipos_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProyectoEquipos_Proyectos_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyectos",
                        principalColumn: "ProyectoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reuniones",
                columns: table => new
                {
                    ReunionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoID = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reuniones", x => x.ReunionID);
                    table.ForeignKey(
                        name: "FK_Reuniones_Proyectos_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyectos",
                        principalColumn: "ProyectoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipoMiembros",
                columns: table => new
                {
                    EquipoMiembroID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    RolEnEquipo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipoMiembros", x => x.EquipoMiembroID);
                    table.ForeignKey(
                        name: "FK_EquipoMiembros_Equipos_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipos",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipoMiembros_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reportes",
                columns: table => new
                {
                    ReporteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    TipoReporte = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Detalles = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportes", x => x.ReporteID);
                    table.ForeignKey(
                        name: "FK_Reportes_Proyectos_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyectos",
                        principalColumn: "ProyectoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reportes_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoID = table.Column<int>(type: "int", nullable: false),
                    AsignadoA = table.Column<int>(type: "int", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prioridad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK_Tareas_Proyectos_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyectos",
                        principalColumn: "ProyectoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tareas_Usuarios_AsignadoA",
                        column: x => x.AsignadoA,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRoles",
                columns: table => new
                {
                    UsuarioRolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    RolID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRoles", x => x.UsuarioRolID);
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Roles_RolID",
                        column: x => x.RolID,
                        principalTable: "Roles",
                        principalColumn: "RolID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRoles_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReunionesParticipantes",
                columns: table => new
                {
                    ReunionParticipanteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReunionID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReunionesParticipantes", x => x.ReunionParticipanteID);
                    table.ForeignKey(
                        name: "FK_ReunionesParticipantes_Reuniones_ReunionID",
                        column: x => x.ReunionID,
                        principalTable: "Reuniones",
                        principalColumn: "ReunionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReunionesParticipantes_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Archivos",
                columns: table => new
                {
                    ArchivoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TareaID = table.Column<int>(type: "int", nullable: true),
                    ProyectoID = table.Column<int>(type: "int", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ruta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archivos", x => x.ArchivoID);
                    table.ForeignKey(
                        name: "FK_Archivos_Proyectos_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyectos",
                        principalColumn: "ProyectoID");
                    table.ForeignKey(
                        name: "FK_Archivos_Tareas_TareaID",
                        column: x => x.TareaID,
                        principalTable: "Tareas",
                        principalColumn: "TareaID");
                    table.ForeignKey(
                        name: "FK_Archivos_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    ComentarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TareaID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaComentario = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.ComentarioID);
                    table.ForeignKey(
                        name: "FK_Comentarios_Tareas_TareaID",
                        column: x => x.TareaID,
                        principalTable: "Tareas",
                        principalColumn: "TareaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Archivos_ProyectoID",
                table: "Archivos",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_Archivos_TareaID",
                table: "Archivos",
                column: "TareaID");

            migrationBuilder.CreateIndex(
                name: "IX_Archivos_UsuarioID",
                table: "Archivos",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_TareaID",
                table: "Comentarios",
                column: "TareaID");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UsuarioID",
                table: "Comentarios",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipoMiembros_EquipoID",
                table: "EquipoMiembros",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipoMiembros_UsuarioID",
                table: "EquipoMiembros",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoEquipos_EquipoID",
                table: "ProyectoEquipos",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoEquipos_ProyectoID",
                table: "ProyectoEquipos",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_ProyectoID",
                table: "Reportes",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_Reportes_UsuarioID",
                table: "Reportes",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Reuniones_ProyectoID",
                table: "Reuniones",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_ReunionesParticipantes_ReunionID",
                table: "ReunionesParticipantes",
                column: "ReunionID");

            migrationBuilder.CreateIndex(
                name: "IX_ReunionesParticipantes_UsuarioID",
                table: "ReunionesParticipantes",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_AsignadoA",
                table: "Tareas",
                column: "AsignadoA");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ProyectoID",
                table: "Tareas",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRoles_RolID",
                table: "UsuarioRoles",
                column: "RolID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRoles_UsuarioID",
                table: "UsuarioRoles",
                column: "UsuarioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Archivos");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "EquipoMiembros");

            migrationBuilder.DropTable(
                name: "ProyectoEquipos");

            migrationBuilder.DropTable(
                name: "Reportes");

            migrationBuilder.DropTable(
                name: "ReunionesParticipantes");

            migrationBuilder.DropTable(
                name: "UsuarioRoles");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropTable(
                name: "Reuniones");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Proyectos");
        }
    }
}
