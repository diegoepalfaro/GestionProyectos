-- Crear la base de datos
CREATE DATABASE GESTION_PROYECTOS;
GO

-- Usar la base de datos
USE GESTION_PROYECTOS;
GO

/* ========================
   TABLAS PRINCIPALES
   ======================== */

-- Tabla de Usuarios (colaboradores, clientes, administradores)
CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) UNIQUE NOT NULL,
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(255),
    Contraseña NVARCHAR(255) NOT NULL,
    Estado BIT DEFAULT 1 -- Activo/Inactivo
);

-- Tabla de Roles
CREATE TABLE Roles (
    RolID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL
);

-- Relación Usuario-Rol (un usuario puede tener varios roles)
CREATE TABLE UsuarioRoles (
    UsuarioRolID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    RolID INT NOT NULL,
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
    FOREIGN KEY (RolID) REFERENCES Roles(RolID)
);

-- Tabla de Proyectos
CREATE TABLE Proyectos (
    ProyectoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    FechaInicio DATE NOT NULL,
    FechaFin DATE,
	EquipoID INT NOT NULL,
    Estado NVARCHAR(50) DEFAULT 'En Progreso',
	
	ADD CONSTRAINT FK_Proyecto_Equipo
	FOREIGN KEY (EquipoId) REFERENCES Equipo(EquipoId);

);

-- Tabla de Equipos de Proyecto
CREATE TABLE Equipos (
    EquipoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255)
);

-- Relación Proyecto - Equipo
CREATE TABLE ProyectoEquipos (
    ProyectoEquipoID INT PRIMARY KEY IDENTITY(1,1),
    ProyectoID INT NOT NULL,
    EquipoID INT NOT NULL,
    FOREIGN KEY (ProyectoID) REFERENCES Proyectos(ProyectoID),
    FOREIGN KEY (EquipoID) REFERENCES Equipos(EquipoID)
);

-- Relación Equipo - Miembros
CREATE TABLE EquipoMiembros (
    EquipoMiembroID INT PRIMARY KEY IDENTITY(1,1),
    EquipoID INT NOT NULL,
    UsuarioID INT NOT NULL,
    RolEnEquipo NVARCHAR(100),
    FOREIGN KEY (EquipoID) REFERENCES Equipos(EquipoID),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

/* ========================
   TABLAS RELACIONADAS CON TAREAS Y AVANCES
   ======================== */

-- Tabla de Tareas
CREATE TABLE Tareas (
    TareaID INT PRIMARY KEY IDENTITY(1,1),
    ProyectoID INT NOT NULL,
    AsignadoA INT NULL, -- Usuario asignado
    Titulo NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Estado NVARCHAR(50) DEFAULT 'Pendiente', -- Pendiente, En Progreso, Completada
    Prioridad NVARCHAR(20) DEFAULT 'Media',  -- Baja, Media, Alta
    FechaInicio DATE,
    FechaFin DATE,
    FOREIGN KEY (ProyectoID) REFERENCES Proyectos(ProyectoID),
    FOREIGN KEY (AsignadoA) REFERENCES Usuarios(UsuarioID)
);

-- Tabla de Comentarios en Tareas
CREATE TABLE Comentarios (
    ComentarioID INT PRIMARY KEY IDENTITY(1,1),
    TareaID INT NOT NULL,
    UsuarioID INT NOT NULL,
    Comentario NVARCHAR(MAX) NOT NULL,
    FechaComentario DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TareaID) REFERENCES Tareas(TareaID),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

-- Tabla de Archivos adjuntos
CREATE TABLE Archivos (
    ArchivoID INT PRIMARY KEY IDENTITY(1,1),
    TareaID INT NULL,
    ProyectoID INT NULL,
    UsuarioID INT NOT NULL,
    NombreArchivo NVARCHAR(200) NOT NULL,
    Ruta NVARCHAR(MAX) NOT NULL, -- Ruta o URL
    FechaSubida DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (TareaID) REFERENCES Tareas(TareaID),
    FOREIGN KEY (ProyectoID) REFERENCES Proyectos(ProyectoID),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

/* ========================
   TABLAS RELACIONADAS CON REPORTES Y SEGUIMIENTO
   ======================== */

-- Tabla de Reportes
CREATE TABLE Reportes (
    ReporteID INT PRIMARY KEY IDENTITY(1,1),
    ProyectoID INT NOT NULL,
    UsuarioID INT NOT NULL,
    TipoReporte NVARCHAR(100), -- Avance, Riesgo, Resumen
    FechaGeneracion DATETIME DEFAULT GETDATE(),
    Detalles NVARCHAR(MAX),
    FOREIGN KEY (ProyectoID) REFERENCES Proyectos(ProyectoID),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

-- Tabla de Reuniones
CREATE TABLE Reuniones (
    ReunionID INT PRIMARY KEY IDENTITY(1,1),
    ProyectoID INT NOT NULL,
    Titulo NVARCHAR(200) NOT NULL,
    Fecha DATETIME NOT NULL,
    Notas NVARCHAR(MAX),
    FOREIGN KEY (ProyectoID) REFERENCES Proyectos(ProyectoID)
);

-- Tabla de Participantes en Reuniones
CREATE TABLE ReunionesParticipantes (
    ReunionParticipanteID INT PRIMARY KEY IDENTITY(1,1),
    ReunionID INT NOT NULL,
    UsuarioID INT NOT NULL,
    FOREIGN KEY (ReunionID) REFERENCES Reuniones(ReunionID),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);