using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionTareas.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__MigrationHistory",
                columns: table => new
                {
                    MigrationId = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ContextKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Model = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ProductVersion = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.__MigrationHistory", x => new { x.MigrationId, x.ContextKey });
                });

            migrationBuilder.CreateTable(
                name: "Equipo",
                columns: table => new
                {
                    EquipoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Equipo", x => x.EquipoID);
                });

            migrationBuilder.CreateTable(
                name: "Proyecto",
                columns: table => new
                {
                    ProyectoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Proyecto", x => x.ProyectoID);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Rol", x => x.RolID);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Contraseña = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Usuario", x => x.UsuarioID);
                });

            migrationBuilder.CreateTable(
                name: "ProyectoEquipo",
                columns: table => new
                {
                    ProyectoEquipoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoID = table.Column<int>(type: "int", nullable: false),
                    EquipoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.ProyectoEquipo", x => x.ProyectoEquipoID);
                    table.ForeignKey(
                        name: "FK_dbo.ProyectoEquipo_dbo.Equipo_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipo",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.ProyectoEquipo_dbo.Proyecto_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyecto",
                        principalColumn: "ProyectoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reunion",
                columns: table => new
                {
                    ReunionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoID = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime", nullable: false),
                    Notas = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Reunion", x => x.ReunionID);
                    table.ForeignKey(
                        name: "FK_dbo.Reunion_dbo.Proyecto_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyecto",
                        principalColumn: "ProyectoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipoMiembro",
                columns: table => new
                {
                    EquipoMiembroID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipoID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    RolEnEquipo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.EquipoMiembro", x => x.EquipoMiembroID);
                    table.ForeignKey(
                        name: "FK_dbo.EquipoMiembro_dbo.Equipo_EquipoID",
                        column: x => x.EquipoID,
                        principalTable: "Equipo",
                        principalColumn: "EquipoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.EquipoMiembro_dbo.Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reporte",
                columns: table => new
                {
                    ReporteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    TipoReporte = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaGeneracion = table.Column<DateTime>(type: "datetime", nullable: false),
                    Detalles = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Reporte", x => x.ReporteID);
                    table.ForeignKey(
                        name: "FK_dbo.Reporte_dbo.Proyecto_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyecto",
                        principalColumn: "ProyectoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.Reporte_dbo.Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tarea",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProyectoID = table.Column<int>(type: "int", nullable: false),
                    AsignadoA = table.Column<int>(type: "int", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Prioridad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaFin = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Tarea", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK_dbo.Tarea_dbo.Proyecto_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyecto",
                        principalColumn: "ProyectoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.Tarea_dbo.Usuario_AsignadoA",
                        column: x => x.AsignadoA,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                columns: table => new
                {
                    UsuarioRolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    RolID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.UsuarioRol", x => x.UsuarioRolID);
                    table.ForeignKey(
                        name: "FK_dbo.UsuarioRol_dbo.Rol_RolID",
                        column: x => x.RolID,
                        principalTable: "Rol",
                        principalColumn: "RolID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.UsuarioRol_dbo.Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReunionParticipante",
                columns: table => new
                {
                    ReunionParticipanteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReunionID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.ReunionParticipante", x => x.ReunionParticipanteID);
                    table.ForeignKey(
                        name: "FK_dbo.ReunionParticipante_dbo.Reunion_ReunionID",
                        column: x => x.ReunionID,
                        principalTable: "Reunion",
                        principalColumn: "ReunionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.ReunionParticipante_dbo.Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Archivo",
                columns: table => new
                {
                    ArchivoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TareaID = table.Column<int>(type: "int", nullable: true),
                    ProyectoID = table.Column<int>(type: "int", nullable: true),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    NombreArchivo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ruta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Archivo", x => x.ArchivoID);
                    table.ForeignKey(
                        name: "FK_dbo.Archivo_dbo.Proyecto_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyecto",
                        principalColumn: "ProyectoID");
                    table.ForeignKey(
                        name: "FK_dbo.Archivo_dbo.Tarea_TareaID",
                        column: x => x.TareaID,
                        principalTable: "Tarea",
                        principalColumn: "TareaID");
                    table.ForeignKey(
                        name: "FK_dbo.Archivo_dbo.Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentario",
                columns: table => new
                {
                    ComentarioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TareaID = table.Column<int>(type: "int", nullable: false),
                    UsuarioID = table.Column<int>(type: "int", nullable: false),
                    ComentarioTexto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaComentario = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.Comentario", x => x.ComentarioID);
                    table.ForeignKey(
                        name: "FK_dbo.Comentario_dbo.Tarea_TareaID",
                        column: x => x.TareaID,
                        principalTable: "Tarea",
                        principalColumn: "TareaID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.Comentario_dbo.Usuario_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoID",
                table: "Archivo",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_TareaID",
                table: "Archivo",
                column: "TareaID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioID",
                table: "Archivo",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_TareaID",
                table: "Comentario",
                column: "TareaID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioID",
                table: "Comentario",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipoID",
                table: "EquipoMiembro",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioID",
                table: "EquipoMiembro",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipoID",
                table: "ProyectoEquipo",
                column: "EquipoID");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoID",
                table: "ProyectoEquipo",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoID",
                table: "Reporte",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioID",
                table: "Reporte",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoID",
                table: "Reunion",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_ReunionID",
                table: "ReunionParticipante",
                column: "ReunionID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioID",
                table: "ReunionParticipante",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_AsignadoA",
                table: "Tarea",
                column: "AsignadoA");

            migrationBuilder.CreateIndex(
                name: "IX_ProyectoID",
                table: "Tarea",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_RolID",
                table: "UsuarioRol",
                column: "RolID");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioID",
                table: "UsuarioRol",
                column: "UsuarioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__MigrationHistory");

            migrationBuilder.DropTable(
                name: "Archivo");

            migrationBuilder.DropTable(
                name: "Comentario");

            migrationBuilder.DropTable(
                name: "EquipoMiembro");

            migrationBuilder.DropTable(
                name: "ProyectoEquipo");

            migrationBuilder.DropTable(
                name: "Reporte");

            migrationBuilder.DropTable(
                name: "ReunionParticipante");

            migrationBuilder.DropTable(
                name: "UsuarioRol");

            migrationBuilder.DropTable(
                name: "Tarea");

            migrationBuilder.DropTable(
                name: "Equipo");

            migrationBuilder.DropTable(
                name: "Reunion");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Proyecto");
        }
    }
}
