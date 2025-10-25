# DICCIONARIO DE DATOS - SISTEMA DE ADQUISICIONES CAASIM

## ESTRUCTURA DEL DOCUMENTO

Este diccionario de datos contiene la definición completa de todas las tablas necesarias para soportar el Sistema de Adquisiciones de CAASIM. Las tablas están organizadas en tres secciones principales:

1. **CATALOGOS_SIMPLES**: Listas de valores con ID y descripción básica
2. **CATALOGOS_ADMINISTRATIVOS_PROCESOS**: Entidades maestras y tablas de procesos del sistema

---

<CATALOGOS_SIMPLES>

/* Opciones: Pendiente de Asignación, Asignada, En Proceso, Completada, Cancelada */
CREATE TABLE [cat_estado_requisicion] [Logico:Catálogo de Estados de Requisición] (
   [id_estado_requisicion] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Estado Requisición]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [color_badge] NVARCHAR(20) NULL, --[Logico:Color Badge]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Borrador, Activo, Pendiente, Aprobado, En Proceso, Completado, Cancelado */
CREATE TABLE [cat_estado_pedido] [Logico:Catálogo de Estados de Pedido] (
   [id_estado_pedido] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Estado Pedido]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [color_badge] NVARCHAR(20) NULL, --[Logico:Color Badge]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Orden de Compra, Pedido, Orden de Servicio, Pedido de Servicio */
CREATE TABLE [cat_tipo_documento_pedido] [Logico:Catálogo de Tipos de Documento de Pedido] (
   [id_tipo_documento_pedido] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Tipo Documento Pedido]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Contrato, Convenio, Pedido, Requisición */
CREATE TABLE [cat_tipo_documento_federal] [Logico:Catálogo de Tipos de Documento Federal] (
   [id_tipo_documento_federal] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Tipo Documento Federal]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [prefijo] NVARCHAR(5) NOT NULL, --[Logico:Prefijo para Número]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: FAIS, FORTAMUN, FASSA, FAEB, FAM, FASP */
CREATE TABLE [cat_fuente_financiamiento] [Logico:Catálogo de Fuentes de Financiamiento Federal] (
   [id_fuente_financiamiento] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Fuente Financiamiento]
   [codigo] NVARCHAR(20) NOT NULL, --[Logico:Código]
   [descripcion] NVARCHAR(300) NOT NULL, --[Logico:Descripción]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Licitación Pública, Invitación a Tres Proveedores, Adjudicación Directa, Pago de Derechos */
CREATE TABLE [cat_modalidad_compra] [Logico:Catálogo de Modalidades de Compra] (
   [id_modalidad_compra] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Modalidad de Compra]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [prefijo] NVARCHAR(5) NOT NULL, --[Logico:Prefijo para Identificador]
   [monto_minimo] DECIMAL(18,2) NULL, --[Logico:Monto Mínimo]
   [monto_maximo] DECIMAL(18,2) NULL, --[Logico:Monto Máximo]
   [plazo_minimo_dias] INT NULL, --[Logico:Plazo Mínimo en Días]
   [requiere_publicacion] BIT NOT NULL, --[Logico:Requiere Publicación]
   [color_badge] NVARCHAR(20) NULL, --[Logico:Color Badge]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Borrador, En Clasificación, Asignado, En Validación, Validado, En Compra, Completado, Cancelado */
CREATE TABLE [cat_estado_procedimiento] [Logico:Catálogo de Estados de Procedimiento de Adquisición] (
   [id_estado_procedimiento] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Estado Procedimiento]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [color_badge] NVARCHAR(20) NULL, --[Logico:Color Badge]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Recursos Estatales, Recursos Federales, Recursos Mixtos */
CREATE TABLE [cat_fuente_presupuestal] [Logico:Catálogo de Fuentes Presupuestales] (
   [id_fuente_presupuestal] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Fuente Presupuestal]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Urgente, Alta, Media, Baja */
CREATE TABLE [cat_nivel_urgencia] [Logico:Catálogo de Niveles de Urgencia] (
   [id_nivel_urgencia] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Nivel de Urgencia]
   [descripcion] NVARCHAR(50) NOT NULL, --[Logico:Descripción]
   [color_badge] NVARCHAR(20) NULL, --[Logico:Color Badge]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Bienes Muebles, Servicios, Obras, Arrendamientos */
CREATE TABLE [cat_categoria_requisicion] [Logico:Catálogo de Categorías de Requisición] (
   [id_categoria_requisicion] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Categoría Requisición]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: PIEZA, KILOGRAMO, LITRO, METRO, SERVICIO, PAQUETE, CAJA, ROLLO, JUEGO */
CREATE TABLE [cat_unidad_medida] [Logico:Catálogo de Unidades de Medida] (
   [id_unidad_medida] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Unidad de Medida]
   [codigo] NVARCHAR(10) NOT NULL, --[Logico:Código]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: EQUIPO DE CÓMPUTO, MOBILIARIO, PAPELERÍA, SERVICIOS, CONSUMIBLES */
CREATE TABLE [cat_categoria_insumo] [Logico:Catálogo de Categorías de Insumo] (
   [id_categoria_insumo] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Categoría Insumo]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [color_badge] NVARCHAR(20) NULL, --[Logico:Color Badge]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Recibido, En Verificación, Aprobado, Rechazado */
CREATE TABLE [cat_estado_entrada_almacen] [Logico:Catálogo de Estados de Entrada de Almacén] (
   [id_estado_entrada] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Estado Entrada]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [color_badge] NVARCHAR(20) NULL, --[Logico:Color Badge]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Normal, Urgente, Programado, Especial */
CREATE TABLE [cat_tipo_pedido] [Logico:Catálogo de Tipos de Pedido] (
   [id_tipo_pedido] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Tipo Pedido]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/* Opciones: Completo, Pendiente, Parcial */
CREATE TABLE [cat_estado_surtido] [Logico:Catálogo de Estados de Surtido] (
   [id_estado_surtido] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Estado Surtido]
   [descripcion] NVARCHAR(100) NOT NULL, --[Logico:Descripción]
   [color_badge] NVARCHAR(20) NULL, --[Logico:Color Badge]
   [activo] BIT NOT NULL --[Logico:Activo]
);

</CATALOGOS_SIMPLES>

<CATALOGOS_ADMINISTRATIVOS_PROCESOS>

/*
{"Componentes donde se utilizan":"MOD-001\COM-0001,MOD-001\COM-0001-1,MOD-001\COM-0001-2,MOD-001\COM-0001-3,MOD-003\COM-0003,MOD-003\COM-0003-1,MOD-003\COM-0003-4"}
*/
CREATE TABLE [requisicion] [Logico:Requisición] (
   [id_requisicion] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Requisición]
   [folio] NVARCHAR(50) NOT NULL, --[Logico:Folio]
   [numero_requisicion_srm] NVARCHAR(50) NULL, --[Logico:Número de Requisición SRM]
   [descripcion] NVARCHAR(1000) NOT NULL, --[Logico:Descripción]
   [monto_total] DECIMAL(18,2) NOT NULL, --[Logico:Monto Total]
   [id_departamento] INT NOT NULL, -- FOREIGN KEY [cat_departamento] PK[id_departamento] DESC[nombre] --[Logico:Departamento]
   [id_fuente_presupuestal] INT NOT NULL, -- FOREIGN KEY [cat_fuente_presupuestal] PK[id_fuente_presupuestal] DESC[descripcion] --[Logico:Fuente Presupuestal]
   [id_nivel_urgencia] INT NOT NULL, -- FOREIGN KEY [cat_nivel_urgencia] PK[id_nivel_urgencia] DESC[descripcion] --[Logico:Nivel de Urgencia]
   [id_categoria_requisicion] INT NOT NULL, -- FOREIGN KEY [cat_categoria_requisicion] PK[id_categoria_requisicion] DESC[descripcion] --[Logico:Categoría]
   [id_estado_requisicion] INT NOT NULL, -- FOREIGN KEY [cat_estado_requisicion] PK[id_estado_requisicion] DESC[descripcion] --[Logico:Estado]
   [folio_consecutivo] NVARCHAR(50) NULL, --[Logico:Folio Consecutivo]
   [fecha_recepcion] DATE NULL, --[Logico:Fecha de Recepción]
   [palabras_clave] NVARCHAR(500) NULL, --[Logico:Palabras Clave]
   [justificacion] NVARCHAR(2000) NULL, --[Logico:Justificación]
   [observaciones] NVARCHAR(2000) NULL, --[Logico:Observaciones]
   [id_procedimiento_adquisicion] INT NULL, -- FOREIGN KEY [procedimiento_adquisicion] PK[id_procedimiento_adquisicion] DESC[identificador] --[Logico:Procedimiento de Adquisición]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [hora_registro] TIME NOT NULL, --[Logico:Hora de Registro]
   [id_usuario_solicitante] INT NOT NULL, -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario Solicitante]
   [fecha_modifica] DATE NULL, --[Logico:Fecha de Modificación]
   [hora_modifica] TIME NULL, --[Logico:Hora de Modificación]
   [id_usuario_modifica] INT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Modifica]
);

/*
{"Componentes donde se utilizan":"MOD-003\COM-0003,MOD-003\COM-0003-3,MOD-003\COM-0003-4"}
*/
CREATE TABLE [requisicion_detalle] [Logico:Detalle de Requisición] (
   [id_requisicion_detalle] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Requisición Detalle]
   [id_requisicion] INT NOT NULL, -- FOREIGN KEY [requisicion] PK[id_requisicion] DESC[folio] --[Logico:Requisición]
   [id_detalle_srm] NVARCHAR(50) NULL, --[Logico:ID Detalle SRM]
   [consecutivo_bien_servicio] NVARCHAR(50) NULL, --[Logico:Consecutivo Bien/Servicio]
   [descripcion] NVARCHAR(1000) NOT NULL, --[Logico:Descripción del Bien/Servicio]
   [cantidad] DECIMAL(18,6) NOT NULL, --[Logico:Cantidad]
   [id_unidad_medida] INT NOT NULL, -- FOREIGN KEY [cat_unidad_medida] PK[id_unidad_medida] DESC[descripcion] --[Logico:Unidad de Medida]
   [precio_unitario] DECIMAL(18,2) NOT NULL, --[Logico:Precio Unitario]
   [subtotal] DECIMAL(18,2) NOT NULL, --[Logico:Subtotal]
   [iva] DECIMAL(18,2) NOT NULL, --[Logico:IVA]
   [total] DECIMAL(18,2) NOT NULL, --[Logico:Total]
   [clave_ramo] NVARCHAR(20) NULL, --[Logico:Clave Ramo]
   [clave_partida] NVARCHAR(20) NULL, --[Logico:Clave Partida]
   [clave_proyecto] NVARCHAR(50) NULL, --[Logico:Clave Proyecto]
   [clave_presupuestal_completa] NVARCHAR(200) NULL --[Logico:Clave Presupuestal Completa]
);

/*
{"Componentes donde se utilizan":"MOD-003\COM-0003,MOD-003\COM-0003-1,MOD-003\COM-0003-4"}
*/
CREATE TABLE [clave_presupuestal] [Logico:Clave Presupuestal] (
   [id_clave_presupuestal] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Clave Presupuestal]
   [anio] INT NOT NULL, --[Logico:Año]
   [clave_ramo] NVARCHAR(20) NOT NULL, --[Logico:Clave Ramo]
   [nombre_ramo] NVARCHAR(300) NULL, --[Logico:Nombre del Ramo]
   [clave_partida] NVARCHAR(20) NOT NULL, --[Logico:Clave Partida]
   [nombre_partida] NVARCHAR(300) NULL, --[Logico:Nombre de la Partida]
   [clave_proyecto] NVARCHAR(50) NULL, --[Logico:Clave Proyecto]
   [nombre_proyecto] NVARCHAR(300) NULL, --[Logico:Nombre del Proyecto]
   [clave_completa] NVARCHAR(200) NOT NULL, --[Logico:Clave Completa]
   [presupuesto_asignado] DECIMAL(18,2) NULL, --[Logico:Presupuesto Asignado]
   [presupuesto_disponible] DECIMAL(18,2) NULL, --[Logico:Presupuesto Disponible]
   [presupuesto_ejercido] DECIMAL(18,2) NULL, --[Logico:Presupuesto Ejercido]
   [presupuesto_restante] DECIMAL(18,2) NULL, --[Logico:Presupuesto Restante]
   [activo] BIT NOT NULL, --[Logico:Activo]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [id_usuario_registro] INT NOT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
);

/*
{"Componentes donde se utilizan":"MOD-001\COM-0001-10,MOD-001\COM-0001-11,MOD-001\COM-0001-12,MOD-001\COM-0001-13,MOD-002\COM-0002,MOD-003\COM-0002,MOD-003\COM-0002-1"}
*/
CREATE TABLE [procedimiento_adquisicion] [Logico:Procedimiento de Adquisición] (
   [id_procedimiento_adquisicion] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Procedimiento de Adquisición]
   [identificador] NVARCHAR(50) NOT NULL, --[Logico:Identificador Único]
   [prefijo] NVARCHAR(5) NOT NULL, --[Logico:Prefijo]
   [consecutivo] INT NOT NULL, --[Logico:Consecutivo]
   [anio] INT NOT NULL, --[Logico:Año]
   [descripcion] NVARCHAR(1000) NULL, --[Logico:Descripción]
   [id_modalidad_compra] INT NOT NULL, -- FOREIGN KEY [cat_modalidad_compra] PK[id_modalidad_compra] DESC[descripcion] --[Logico:Modalidad de Compra]
   [id_estado_procedimiento] INT NOT NULL, -- FOREIGN KEY [cat_estado_procedimiento] PK[id_estado_procedimiento] DESC[descripcion] --[Logico:Estado]
   [monto_total] DECIMAL(18,2) NOT NULL, --[Logico:Monto Total]
   [cantidad_requisiciones] INT NOT NULL, --[Logico:Cantidad de Requisiciones]
   [cantidad_partidas] INT NOT NULL, --[Logico:Cantidad de Partidas]
   [aplica_capitulo_3000] BIT NOT NULL, --[Logico:Aplica Capítulo 3000]
   [indicaciones_especiales] NVARCHAR(2000) NULL, --[Logico:Indicaciones Especiales]
   [observaciones] NVARCHAR(2000) NULL, --[Logico:Observaciones]
   [fecha_creacion] DATE NOT NULL, --[Logico:Fecha de Creación]
   [fecha_validacion] DATE NULL, --[Logico:Fecha de Validación]
   [fecha_asignacion] DATE NOT NULL, --[Logico:Fecha de Asignación]
   [hora_asignacion] TIME NOT NULL, --[Logico:Hora de Asignación]
   [id_usuario_responsable] INT NOT NULL, -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario Responsable]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [hora_registro] TIME NOT NULL, --[Logico:Hora de Registro]
   [id_usuario_registro] INT NOT NULL, -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
   [fecha_modifica] DATE NULL, --[Logico:Fecha de Modificación]
   [hora_modifica] TIME NULL, --[Logico:Hora de Modificación]
   [id_usuario_modifica] INT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Modifica]
);

/*
{"Componentes donde se utilizan":"MOD-001\COM-0001-3"}
*/
CREATE TABLE [cat_departamento] [Logico:Catálogo de Departamentos] (
   [id_departamento] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Departamento]
   [codigo] NVARCHAR(20) NOT NULL, --[Logico:Código]
   [nombre] NVARCHAR(200) NOT NULL, --[Logico:Nombre]
   [descripcion] NVARCHAR(500) NULL, --[Logico:Descripción]
   [activo] BIT NOT NULL, --[Logico:Activo]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [id_usuario_registro] INT NOT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
);

/*
{"Componentes donde se utilizan":"MOD-002\COM-0001,MOD-002\COM-0002,MOD-002\COM-0003,MOD-002\COM-0004,MOD-002\COM-0005,MOD-003\COM-0001,MOD-003\COM-0001-1,MOD-003\COM-0001-4,MOD-003\COM-0002,MOD-003\COM-0002-1,MOD-003\COM-0002-4,MOD-006\COM-XXXX"}
*/
CREATE TABLE [cat_proveedor] [Logico:Catálogo de Proveedores] (
   [id_proveedor] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Proveedor]
   [razon_social] NVARCHAR(300) NOT NULL, --[Logico:Razón Social]
   [rfc] NVARCHAR(13) NOT NULL, --[Logico:RFC]
   [tipo_persona] NVARCHAR(20) NOT NULL, --[Logico:Tipo de Persona]
   [nombre_contacto] NVARCHAR(200) NULL, --[Logico:Nombre de Contacto]
   [correo_electronico] NVARCHAR(200) NULL, --[Logico:Correo Electrónico]
   [telefono] NVARCHAR(50) NULL, --[Logico:Teléfono]
   [direccion] NVARCHAR(500) NULL, --[Logico:Dirección]
   [activo] BIT NOT NULL, --[Logico:Activo]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [id_usuario_registro] INT NOT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
);

/*
{"Componentes donde se utilizan":"MOD-002\COM-0003,MOD-002\COM-0003-2,MOD-002\COM-0005,MOD-002\COM-0005-2"}
*/
CREATE TABLE [cat_insumo] [Logico:Catálogo de Insumos] (
   [id_insumo] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Insumo]
   [codigo] NVARCHAR(50) NOT NULL, --[Logico:Código]
   [nombre] NVARCHAR(300) NOT NULL, --[Logico:Nombre]
   [descripcion] NVARCHAR(1000) NULL, --[Logico:Descripción]
   [id_unidad_medida] INT NULL, -- FOREIGN KEY [cat_unidad_medida] PK[id_unidad_medida] DESC[descripcion] --[Logico:Unidad de Medida]
   [id_categoria_insumo] INT NULL, -- FOREIGN KEY [cat_categoria_insumo] PK[id_categoria_insumo] DESC[descripcion] --[Logico:Categoría]
   [precio_referencia] DECIMAL(18,2) NULL, --[Logico:Precio de Referencia]
   [activo] BIT NOT NULL, --[Logico:Activo]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [id_usuario_registro] INT NOT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
);

/*
{"Componentes donde se utilizan":"MOD-002\COM-0001,MOD-002\COM-0001-4,MOD-002\COM-0002,MOD-002\COM-0003,MOD-002\COM-0005,MOD-003\COM-0001,MOD-003\COM-0001-4,MOD-003\COM-0002,MOD-003\COM-0002-1,MOD-003\COM-0002-3,MOD-003\COM-0002-4"}
*/
CREATE TABLE [pedido] [Logico:Pedido] (
   [id_pedido] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Pedido]
   [folio] NVARCHAR(50) NOT NULL, --[Logico:Folio]
   [consecutivo_completo] NVARCHAR(100) NULL, --[Logico:Consecutivo-ID Requisición-Modalidad-Asignación]
   [id_tipo_documento_pedido] INT NOT NULL, -- FOREIGN KEY [cat_tipo_documento_pedido] PK[id_tipo_documento_pedido] DESC[descripcion] --[Logico:Tipo de Documento]
   [id_tipo_pedido] INT NULL, -- FOREIGN KEY [cat_tipo_pedido] PK[id_tipo_pedido] DESC[descripcion] --[Logico:Tipo de Pedido]
   [id_proveedor] INT NOT NULL, -- FOREIGN KEY [cat_proveedor] PK[id_proveedor] DESC[razon_social] --[Logico:Proveedor]
   [id_procedimiento_adquisicion] INT NULL, -- FOREIGN KEY [procedimiento_adquisicion] PK[id_procedimiento_adquisicion] DESC[identificador] --[Logico:Procedimiento de Adquisición]
   [fecha_pedido] DATE NOT NULL, --[Logico:Fecha del Pedido]
   [numero_contrato] NVARCHAR(100) NULL, --[Logico:Número de Contrato Asociado]
   [destinatario_factura] NVARCHAR(300) NULL, --[Logico:Destinatario de Factura]
   [direccion_entrega] NVARCHAR(500) NULL, --[Logico:Dirección de Entrega]
   [fecha_entrega] DATE NULL, --[Logico:Fecha de Entrega]
   [tiempo_entrega] NVARCHAR(100) NULL, --[Logico:Tiempo de Entrega]
   [persona_elaboro] NVARCHAR(200) NULL, --[Logico:Persona que Elaboró]
   [persona_autorizo] NVARCHAR(200) NULL, --[Logico:Persona que Autorizó]
   [iniciales] NVARCHAR(50) NULL, --[Logico:Iniciales]
   [subtotal] DECIMAL(18,2) NOT NULL, --[Logico:Subtotal]
   [total_iva] DECIMAL(18,2) NOT NULL, --[Logico:Total IVA]
   [total_retenciones] DECIMAL(18,2) NOT NULL, --[Logico:Total Retenciones]
   [monto_total] DECIMAL(18,2) NOT NULL, --[Logico:Monto Total]
   [id_estado_pedido] INT NOT NULL, -- FOREIGN KEY [cat_estado_pedido] PK[id_estado_pedido] DESC[descripcion] --[Logico:Estado del Pedido]
   [id_estado_surtido] INT NULL, -- FOREIGN KEY [cat_estado_surtido] PK[id_estado_surtido] DESC[descripcion] --[Logico:Estado de Surtido]
   [observaciones] NVARCHAR(2000) NULL, --[Logico:Observaciones]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [hora_registro] TIME NOT NULL, --[Logico:Hora de Registro]
   [id_usuario_registro] INT NOT NULL, -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
   [fecha_modifica] DATE NULL, --[Logico:Fecha de Modificación]
   [hora_modifica] TIME NULL, --[Logico:Hora de Modificación]
   [id_usuario_modifica] INT NULL, -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Modifica]
   [fecha_aprueba] DATE NULL, --[Logico:Fecha de Aprobación]
   [hora_aprueba] TIME NULL, --[Logico:Hora de Aprobación]
   [id_usuario_aprueba] INT NULL, -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Aprueba]
   [id_archivo_firma] INT NULL -- FOREIGN KEY [SpartanFile] PK[File_Id] DESC[Description] --[Logico:Archivo de Firma Digital]
);

/*
{"Componentes donde se utilizan":"MOD-002\COM-0002,MOD-002\COM-0003,MOD-002\COM-0003-2,MOD-002\COM-0003-3,MOD-002\COM-0005,MOD-002\COM-0005-2,MOD-003\COM-0002,MOD-003\COM-0002-3"}
*/
CREATE TABLE [pedido_detalle] [Logico:Detalle del Pedido] (
   [id_pedido_detalle] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Pedido Detalle]
   [id_pedido] INT NOT NULL, -- FOREIGN KEY [pedido] PK[id_pedido] DESC[folio] --[Logico:Pedido]
   [id_requisicion_detalle] NVARCHAR(50) NULL, --[Logico:ID Detalle Requisición]
   [id_requisicion] INT NULL, -- FOREIGN KEY [requisicion] PK[id_requisicion] DESC[folio] --[Logico:Requisición]
   [clave_presupuestal] NVARCHAR(100) NULL, --[Logico:Clave Presupuestal]
   [nombre_partida] NVARCHAR(300) NULL, --[Logico:Nombre de la Partida]
   [id_insumo] INT NOT NULL, -- FOREIGN KEY [cat_insumo] PK[id_insumo] DESC[nombre] --[Logico:Insumo]
   [descripcion] NVARCHAR(1000) NOT NULL, --[Logico:Descripción]
   [numero_partida] NVARCHAR(50) NULL, --[Logico:Número de Partida]
   [anio] INT NULL, --[Logico:Año]
   [cantidad] DECIMAL(18,6) NOT NULL, --[Logico:Cantidad]
   [cantidad_surtida] DECIMAL(18,6) NULL, --[Logico:Cantidad Surtida]
   [unidad] NVARCHAR(50) NOT NULL, --[Logico:Unidad]
   [precio_unitario] DECIMAL(18,2) NOT NULL, --[Logico:Precio Unitario]
   [subtotal] DECIMAL(18,2) NOT NULL, --[Logico:Subtotal]
   [iva] DECIMAL(18,6) NOT NULL, --[Logico:IVA]
   [retenciones] DECIMAL(18,2) NOT NULL, --[Logico:Retenciones]
   [total] DECIMAL(18,2) NOT NULL --[Logico:Total]
);

/*
{"Componentes donde se utilizan":"MOD-002\COM-0004,MOD-002\COM-0004-1"}
*/
CREATE TABLE [contrato_federal] [Logico:Contrato Federal] (
   [id_contrato_federal] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Contrato Federal]
   [numero_contrato] NVARCHAR(100) NOT NULL, --[Logico:Número de Contrato/Convenio]
   [id_tipo_documento_federal] INT NOT NULL, -- FOREIGN KEY [cat_tipo_documento_federal] PK[id_tipo_documento_federal] DESC[descripcion] --[Logico:Tipo de Documento]
   [id_proveedor] INT NOT NULL, -- FOREIGN KEY [cat_proveedor] PK[id_proveedor] DESC[razon_social] --[Logico:Proveedor]
   [id_pedido] INT NULL, -- FOREIGN KEY [pedido] PK[id_pedido] DESC[folio] --[Logico:Pedido Asociado]
   [fecha_pedido] DATE NOT NULL, --[Logico:Fecha de Pedido]
   [destinatario_factura] NVARCHAR(300) NULL, --[Logico:Destinatario de Factura]
   [tiempo_entrega] NVARCHAR(100) NULL, --[Logico:Tiempo de Entrega]
   [persona_elaboro] NVARCHAR(200) NOT NULL, --[Logico:Persona que Elaboró]
   [persona_autorizo] NVARCHAR(200) NOT NULL, --[Logico:Persona que Autorizó]
   [iniciales] NVARCHAR(50) NOT NULL, --[Logico:Iniciales]
   [monto_total] DECIMAL(18,2) NOT NULL, --[Logico:Monto Total]
   [observaciones] NVARCHAR(2000) NULL, --[Logico:Observaciones]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [hora_registro] TIME NOT NULL, --[Logico:Hora de Registro]
   [id_usuario_registro] INT NOT NULL, -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
   [fecha_modifica] DATE NULL, --[Logico:Fecha de Modificación]
   [hora_modifica] TIME NULL, --[Logico:Hora de Modificación]
   [id_usuario_modifica] INT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Modifica]
);

/*
{"Componentes donde se utilizan":"MOD-002\COM-0001,MOD-002\COM-0001-4,MOD-002\COM-0004,MOD-002\COM-0004-2"}
*/
CREATE TABLE [pedido_claves_presupuestales] [Logico:Claves Presupuestales del Pedido] (
   [id_pedido_clave] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Pedido Clave]
   [id_pedido] INT NOT NULL, -- FOREIGN KEY [pedido] PK[id_pedido] DESC[folio] --[Logico:Pedido]
   [clave_presupuestal] NVARCHAR(100) NOT NULL, --[Logico:Clave Presupuestal]
   [descripcion] NVARCHAR(500) NULL, --[Logico:Descripción]
   [monto_asignado] DECIMAL(18,2) NOT NULL --[Logico:Monto Asignado]
);

/*
{"Componentes donde se utilizan":"MOD-002\COM-0004,MOD-002\COM-0004-2"}
*/
CREATE TABLE [clave_presupuestal_federal] [Logico:Clave Presupuestal Federal] (
   [id_clave_presupuestal_federal] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Clave Presupuestal Federal]
   [id_contrato_federal] INT NOT NULL, -- FOREIGN KEY [contrato_federal] PK[id_contrato_federal] DESC[numero_contrato] --[Logico:Contrato Federal]
   [anio] INT NOT NULL, --[Logico:Año]
   [ramo] NVARCHAR(20) NOT NULL, --[Logico:Ramo]
   [partida] NVARCHAR(20) NOT NULL, --[Logico:Partida]
   [proyecto] NVARCHAR(50) NULL, --[Logico:Proyecto]
   [componente] NVARCHAR(50) NULL, --[Logico:Componente]
   [actividad] NVARCHAR(50) NULL, --[Logico:Actividad]
   [clave_completa] NVARCHAR(200) NOT NULL, --[Logico:Clave Completa]
   [id_fuente_financiamiento] INT NULL, -- FOREIGN KEY [cat_fuente_financiamiento] PK[id_fuente_financiamiento] DESC[descripcion] --[Logico:Fuente de Financiamiento]
   [observaciones] NVARCHAR(1000) NULL --[Logico:Observaciones]
);

/*
{"Componentes donde se utilizan":"MOD-002\COM-XXXX,MOD-003\COM-0001,MOD-003\COM-0001-1,MOD-003\COM-0001-2,MOD-003\COM-0001-3,MOD-003\COM-0001-4"}
*/
CREATE TABLE [entrada_almacen] [Logico:Entrada de Almacén] (
   [id_entrada_almacen] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Entrada de Almacén]
   [folio] NVARCHAR(50) NOT NULL, --[Logico:Folio de Almacén]
   [fecha_vale] DATE NOT NULL, --[Logico:Fecha de Vale]
   [id_pedido] INT NOT NULL, -- FOREIGN KEY [pedido] PK[id_pedido] DESC[folio] --[Logico:Pedido]
   [id_proveedor] INT NOT NULL, -- FOREIGN KEY [cat_proveedor] PK[id_proveedor] DESC[razon_social] --[Logico:Proveedor]
   [numero_factura] NVARCHAR(100) NULL, --[Logico:Número de Factura]
   [fecha_factura] DATE NULL, --[Logico:Fecha de Factura]
   [clave_presupuestal] NVARCHAR(100) NULL, --[Logico:Clave Presupuestal]
   [descripcion] NVARCHAR(1000) NULL, --[Logico:Descripción]
   [cantidad] DECIMAL(18,6) NULL, --[Logico:Cantidad]
   [costo_unitario] DECIMAL(18,2) NULL, --[Logico:Costo Unitario]
   [unidad_medida] NVARCHAR(50) NULL, --[Logico:Unidad de Medida]
   [monto_total] DECIMAL(18,2) NULL, --[Logico:Monto Total]
   [id_estado_entrada] INT NOT NULL, -- FOREIGN KEY [cat_estado_entrada_almacen] PK[id_estado_entrada] DESC[descripcion] --[Logico:Estado de la Entrada]
   [fecha_entrada] DATE NOT NULL, --[Logico:Fecha de Entrada]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [hora_registro] TIME NOT NULL, --[Logico:Hora de Registro]
   [id_usuario_registro] INT NOT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
);

/*
{"Componentes donde se utilizan":"MOD-004\COM-XXXX"}
*/
CREATE TABLE [cuadro_validacion_proveedor_propuesta] [Logico:Cuadro de Validación de Propuestas de Proveedores] (
   [id_cuadro_validacion] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Cuadro Validación]
   [id_procedimiento_adquisicion] INT NOT NULL, -- FOREIGN KEY [procedimiento_adquisicion] PK[id_procedimiento_adquisicion] DESC[identificador] --[Logico:Procedimiento de Adquisición]
   [id_requisicion] INT NOT NULL, -- FOREIGN KEY [requisicion] PK[id_requisicion] DESC[folio] --[Logico:Requisición]
   [id_proveedor] INT NOT NULL, -- FOREIGN KEY [cat_proveedor] PK[id_proveedor] DESC[razon_social] --[Logico:Proveedor]
   [monto_propuesta] DECIMAL(18,2) NOT NULL, --[Logico:Monto de la Propuesta]
   [evaluacion_tecnica] NVARCHAR(500) NULL, --[Logico:Evaluación Técnica]
   [evaluacion_economica] NVARCHAR(500) NULL, --[Logico:Evaluación Económica]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [id_usuario_registro] INT NOT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
);

/*
{"Componentes donde se utilizan":"MOD-005\COM-XXXX"}
*/
CREATE TABLE [cat_tipo_documento] [Logico:Catálogo de Tipos de Documento] (
   [id_tipo_documento] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Tipo de Documento]
   [nombre] NVARCHAR(200) NOT NULL, --[Logico:Nombre]
   [descripcion] NVARCHAR(500) NULL, --[Logico:Descripción]
   [aplica_persona_fisica] BIT NOT NULL, --[Logico:Aplica a Persona Física]
   [aplica_persona_moral] BIT NOT NULL, --[Logico:Aplica a Persona Moral]
   [es_obligatorio] BIT NOT NULL, --[Logico:Es Obligatorio]
   [activo] BIT NOT NULL --[Logico:Activo]
);

/*
{"Componentes donde se utilizan":"MOD-006\COM-XXXX"}
*/
CREATE TABLE [documento_proveedor] [Logico:Documento del Proveedor] (
   [id_documento_proveedor] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Documento Proveedor]
   [id_proveedor] INT NOT NULL, -- FOREIGN KEY [cat_proveedor] PK[id_proveedor] DESC[razon_social] --[Logico:Proveedor]
   [id_tipo_documento] INT NOT NULL, -- FOREIGN KEY [cat_tipo_documento] PK[id_tipo_documento] DESC[nombre] --[Logico:Tipo de Documento]
   [id_archivo] INT NOT NULL, -- FOREIGN KEY [SpartanFile] PK[File_Id] DESC[Description] --[Logico:Archivo]
   [fecha_vigencia] DATE NULL, --[Logico:Fecha de Vigencia]
   [estado_vigencia] NVARCHAR(50) NULL, --[Logico:Estado de Vigencia]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [id_usuario_registro] INT NOT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
);

/*
{"Componentes donde se utilizan":"MOD-001\COM-XXXX,MOD-006\COM-XXXX"}
*/
CREATE TABLE [procedimiento_proveedores] [Logico:Proveedores Asignados al Procedimiento] (
   [id_procedimiento_proveedor] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Procedimiento Proveedor]
   [id_procedimiento_adquisicion] INT NOT NULL, -- FOREIGN KEY [procedimiento_adquisicion] PK[id_procedimiento_adquisicion] DESC[identificador] --[Logico:Procedimiento de Adquisición]
   [id_proveedor] INT NOT NULL, -- FOREIGN KEY [cat_proveedor] PK[id_proveedor] DESC[razon_social] --[Logico:Proveedor]
   [fecha_asignacion] DATE NOT NULL, --[Logico:Fecha de Asignación]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [id_usuario_registro] INT NOT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Usuario que Registra]
);

/*
{"Componentes donde se utilizan":"MOD-005\COM-XXXX"}
*/
CREATE TABLE [dictamen_juridico] [Logico:Dictamen Jurídico] (
   [id_dictamen_juridico] INT IDENTITY(1,1) PRIMARY KEY, --[Logico:ID Dictamen Jurídico]
   [id_procedimiento_adquisicion] INT NOT NULL, -- FOREIGN KEY [procedimiento_adquisicion] PK[id_procedimiento_adquisicion] DESC[identificador] --[Logico:Procedimiento de Adquisición]
   [resultado_dictamen] NVARCHAR(100) NOT NULL, --[Logico:Resultado del Dictamen]
   [observaciones] NVARCHAR(2000) NULL, --[Logico:Observaciones]
   [id_archivo_dictamen] INT NULL, -- FOREIGN KEY [SpartanFile] PK[File_Id] DESC[Description] --[Logico:Archivo del Dictamen]
   [fecha_emision] DATE NOT NULL, --[Logico:Fecha de Emisión]
   [fecha_registro] DATE NOT NULL, --[Logico:Fecha de Registro]
   [id_usuario_abogado] INT NOT NULL -- FOREIGN KEY [Spartan_User] PK[Id_User] DESC[Name] --[Logico:Abogado que Emite]
);

</CATALOGOS_ADMINISTRATIVOS_PROCESOS>