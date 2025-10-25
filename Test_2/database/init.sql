-- Create database if not exists
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RequirementsAnalysisDB')
BEGIN
    CREATE DATABASE RequirementsAnalysisDB;
END
GO

USE RequirementsAnalysisDB;
GO

-- Tabla: proceso
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[proceso]') AND type in (N'U'))
BEGIN
    CREATE TABLE proceso (
        id_proceso INT IDENTITY(1,1) PRIMARY KEY,
        nombre NVARCHAR(150) NOT NULL,
        descripcion NVARCHAR(MAX),
        requirement_text NVARCHAR(MAX),
        created_at DATETIME2 DEFAULT GETDATE()
    );
END
GO

-- Tabla: subproceso
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[subproceso]') AND type in (N'U'))
BEGIN
    CREATE TABLE subproceso (
        id_subproceso INT IDENTITY(1,1) PRIMARY KEY,
        id_proceso INT NOT NULL,
        nombre NVARCHAR(150) NOT NULL,
        descripcion NVARCHAR(MAX),
        FOREIGN KEY (id_proceso) REFERENCES proceso(id_proceso) ON DELETE CASCADE
    );
END
GO

-- Tabla: caso_uso
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[caso_uso]') AND type in (N'U'))
BEGIN
    CREATE TABLE caso_uso (
        id_caso_uso INT IDENTITY(1,1) PRIMARY KEY,
        id_subproceso INT NOT NULL,
        nombre NVARCHAR(150) NOT NULL,
        descripcion NVARCHAR(MAX),
        actor_principal NVARCHAR(150),
        tipo_caso_uso SMALLINT CHECK (tipo_caso_uso IN (1, 2, 3)),
        -- 1=Funcional, 2=No Funcional, 3=Sistema
        precondiciones NVARCHAR(MAX),
        postcondiciones NVARCHAR(MAX),
        criterios_de_aceptacion NVARCHAR(MAX),
        FOREIGN KEY (id_subproceso) REFERENCES subproceso(id_subproceso) ON DELETE CASCADE
    );
END
GO

