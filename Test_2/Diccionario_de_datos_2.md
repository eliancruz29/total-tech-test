# DICCIONARIO DE DATOS

Base de datos:		
-- Tabla: proceso
CREATE TABLE proceso (
   id_proceso INT IDENTITY(1,1) PRIMARY KEY,
   nombre NVARCHAR(150) NOT NULL,
   descripcion NVARCHAR(MAX)
);
   
-- Tabla: subproceso
CREATE TABLE subproceso (
   id_subproceso INT IDENTITY(1,1) PRIMARY KEY,
   id_proceso INT NOT NULL,
   nombre NVARCHAR(150) NOT NULL,
   descripcion NVARCHAR(MAX),
   FOREIGN KEY (id_proceso) REFERENCES proceso(id_proceso)
);

-- Tabla: caso_uso
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
   FOREIGN KEY (id_subproceso) REFERENCES subproceso(id_subproceso)
);
