-- ========================================
-- Migration 002: Create Main Process Tables
-- Sistema de Adquisiciones CAASIM
-- ========================================

-- ========================================
-- USER TABLE (Simplified for authentication)
-- ========================================

CREATE TABLE spartan_user (
    id_user SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    email VARCHAR(200) NOT NULL,
    password_hash VARCHAR(500) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT uk_spartan_user_email UNIQUE (email)
);

COMMENT ON TABLE spartan_user IS 'Usuarios del Sistema';

-- ========================================
-- PROVIDER/SUPPLIER TABLE
-- ========================================

CREATE TABLE cat_proveedor (
    id_proveedor SERIAL PRIMARY KEY,
    razon_social VARCHAR(300) NOT NULL,
    rfc VARCHAR(13) NOT NULL,
    tipo_persona VARCHAR(20) NOT NULL,
    nombre_contacto VARCHAR(200),
    correo_electronico VARCHAR(200),
    telefono VARCHAR(50),
    direccion VARCHAR(500),
    activo BOOLEAN NOT NULL DEFAULT true,
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    id_usuario_registro INTEGER NOT NULL,
    CONSTRAINT uk_proveedor_rfc UNIQUE (rfc),
    CONSTRAINT fk_proveedor_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user),
    CONSTRAINT chk_tipo_persona CHECK (tipo_persona IN ('FISICA', 'MORAL'))
);

COMMENT ON TABLE cat_proveedor IS 'Catálogo de Proveedores';

CREATE INDEX idx_proveedor_activo ON cat_proveedor(activo);
CREATE INDEX idx_proveedor_rfc ON cat_proveedor(rfc);
CREATE INDEX idx_proveedor_razon_social ON cat_proveedor(razon_social);

-- ========================================
-- SUPPLY/INSUMO TABLE
-- ========================================

CREATE TABLE cat_insumo (
    id_insumo SERIAL PRIMARY KEY,
    codigo VARCHAR(50) NOT NULL,
    nombre VARCHAR(300) NOT NULL,
    descripcion VARCHAR(1000),
    id_unidad_medida INTEGER,
    id_categoria_insumo INTEGER,
    precio_referencia DECIMAL(18,2),
    activo BOOLEAN NOT NULL DEFAULT true,
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    id_usuario_registro INTEGER NOT NULL,
    CONSTRAINT uk_insumo_codigo UNIQUE (codigo),
    CONSTRAINT fk_insumo_unidad_medida FOREIGN KEY (id_unidad_medida)
        REFERENCES cat_unidad_medida(id_unidad_medida),
    CONSTRAINT fk_insumo_categoria FOREIGN KEY (id_categoria_insumo)
        REFERENCES cat_categoria_insumo(id_categoria_insumo),
    CONSTRAINT fk_insumo_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE cat_insumo IS 'Catálogo de Insumos';

CREATE INDEX idx_insumo_activo ON cat_insumo(activo);
CREATE INDEX idx_insumo_codigo ON cat_insumo(codigo);
CREATE INDEX idx_insumo_nombre ON cat_insumo(nombre);

-- ========================================
-- BUDGET KEY TABLE
-- ========================================

CREATE TABLE clave_presupuestal (
    id_clave_presupuestal SERIAL PRIMARY KEY,
    anio INTEGER NOT NULL,
    clave_ramo VARCHAR(20) NOT NULL,
    nombre_ramo VARCHAR(300),
    clave_partida VARCHAR(20) NOT NULL,
    nombre_partida VARCHAR(300),
    clave_proyecto VARCHAR(50),
    nombre_proyecto VARCHAR(300),
    clave_completa VARCHAR(200) NOT NULL,
    presupuesto_asignado DECIMAL(18,2),
    presupuesto_disponible DECIMAL(18,2),
    presupuesto_ejercido DECIMAL(18,2),
    presupuesto_restante DECIMAL(18,2),
    activo BOOLEAN NOT NULL DEFAULT true,
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    id_usuario_registro INTEGER NOT NULL,
    CONSTRAINT uk_clave_presupuestal_completa UNIQUE (clave_completa, anio),
    CONSTRAINT fk_clave_presupuestal_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE clave_presupuestal IS 'Clave Presupuestal';

CREATE INDEX idx_clave_presupuestal_anio ON clave_presupuestal(anio);
CREATE INDEX idx_clave_presupuestal_activo ON clave_presupuestal(activo);
CREATE INDEX idx_clave_presupuestal_completa ON clave_presupuestal(clave_completa);

-- ========================================
-- ACQUISITION PROCEDURE TABLE
-- ========================================

CREATE TABLE procedimiento_adquisicion (
    id_procedimiento_adquisicion SERIAL PRIMARY KEY,
    identificador VARCHAR(50) NOT NULL,
    prefijo VARCHAR(5) NOT NULL,
    consecutivo INTEGER NOT NULL,
    anio INTEGER NOT NULL,
    descripcion VARCHAR(1000),
    id_modalidad_compra INTEGER NOT NULL,
    id_estado_procedimiento INTEGER NOT NULL,
    monto_total DECIMAL(18,2) NOT NULL DEFAULT 0,
    cantidad_requisiciones INTEGER NOT NULL DEFAULT 0,
    cantidad_partidas INTEGER NOT NULL DEFAULT 0,
    aplica_capitulo_3000 BOOLEAN NOT NULL DEFAULT false,
    indicaciones_especiales VARCHAR(2000),
    observaciones VARCHAR(2000),
    fecha_creacion DATE NOT NULL DEFAULT CURRENT_DATE,
    fecha_validacion DATE,
    fecha_asignacion DATE NOT NULL DEFAULT CURRENT_DATE,
    hora_asignacion TIME NOT NULL DEFAULT CURRENT_TIME,
    id_usuario_responsable INTEGER NOT NULL,
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    hora_registro TIME NOT NULL DEFAULT CURRENT_TIME,
    id_usuario_registro INTEGER NOT NULL,
    fecha_modifica DATE,
    hora_modifica TIME,
    id_usuario_modifica INTEGER,
    CONSTRAINT uk_procedimiento_identificador UNIQUE (identificador),
    CONSTRAINT fk_procedimiento_modalidad_compra FOREIGN KEY (id_modalidad_compra)
        REFERENCES cat_modalidad_compra(id_modalidad_compra),
    CONSTRAINT fk_procedimiento_estado FOREIGN KEY (id_estado_procedimiento)
        REFERENCES cat_estado_procedimiento(id_estado_procedimiento),
    CONSTRAINT fk_procedimiento_usuario_responsable FOREIGN KEY (id_usuario_responsable)
        REFERENCES spartan_user(id_user),
    CONSTRAINT fk_procedimiento_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user),
    CONSTRAINT fk_procedimiento_usuario_modifica FOREIGN KEY (id_usuario_modifica)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE procedimiento_adquisicion IS 'Procedimiento de Adquisición';

CREATE INDEX idx_procedimiento_identificador ON procedimiento_adquisicion(identificador);
CREATE INDEX idx_procedimiento_anio ON procedimiento_adquisicion(anio);
CREATE INDEX idx_procedimiento_estado ON procedimiento_adquisicion(id_estado_procedimiento);

-- ========================================
-- REQUISITION TABLE
-- ========================================

CREATE TABLE requisicion (
    id_requisicion SERIAL PRIMARY KEY,
    folio VARCHAR(50) NOT NULL,
    numero_requisicion_srm VARCHAR(50),
    descripcion VARCHAR(1000) NOT NULL,
    monto_total DECIMAL(18,2) NOT NULL DEFAULT 0,
    id_departamento INTEGER NOT NULL,
    id_fuente_presupuestal INTEGER NOT NULL,
    id_nivel_urgencia INTEGER NOT NULL,
    id_categoria_requisicion INTEGER NOT NULL,
    id_estado_requisicion INTEGER NOT NULL,
    folio_consecutivo VARCHAR(50),
    fecha_recepcion DATE,
    palabras_clave VARCHAR(500),
    justificacion VARCHAR(2000),
    observaciones VARCHAR(2000),
    id_procedimiento_adquisicion INTEGER,
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    hora_registro TIME NOT NULL DEFAULT CURRENT_TIME,
    id_usuario_solicitante INTEGER NOT NULL,
    fecha_modifica DATE,
    hora_modifica TIME,
    id_usuario_modifica INTEGER,
    CONSTRAINT uk_requisicion_folio UNIQUE (folio),
    CONSTRAINT fk_requisicion_departamento FOREIGN KEY (id_departamento)
        REFERENCES cat_departamento(id_departamento),
    CONSTRAINT fk_requisicion_fuente_presupuestal FOREIGN KEY (id_fuente_presupuestal)
        REFERENCES cat_fuente_presupuestal(id_fuente_presupuestal),
    CONSTRAINT fk_requisicion_nivel_urgencia FOREIGN KEY (id_nivel_urgencia)
        REFERENCES cat_nivel_urgencia(id_nivel_urgencia),
    CONSTRAINT fk_requisicion_categoria FOREIGN KEY (id_categoria_requisicion)
        REFERENCES cat_categoria_requisicion(id_categoria_requisicion),
    CONSTRAINT fk_requisicion_estado FOREIGN KEY (id_estado_requisicion)
        REFERENCES cat_estado_requisicion(id_estado_requisicion),
    CONSTRAINT fk_requisicion_procedimiento FOREIGN KEY (id_procedimiento_adquisicion)
        REFERENCES procedimiento_adquisicion(id_procedimiento_adquisicion),
    CONSTRAINT fk_requisicion_usuario_solicitante FOREIGN KEY (id_usuario_solicitante)
        REFERENCES spartan_user(id_user),
    CONSTRAINT fk_requisicion_usuario_modifica FOREIGN KEY (id_usuario_modifica)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE requisicion IS 'Requisición';

CREATE INDEX idx_requisicion_folio ON requisicion(folio);
CREATE INDEX idx_requisicion_estado ON requisicion(id_estado_requisicion);
CREATE INDEX idx_requisicion_procedimiento ON requisicion(id_procedimiento_adquisicion);

-- ========================================
-- REQUISITION DETAIL TABLE
-- ========================================

CREATE TABLE requisicion_detalle (
    id_requisicion_detalle SERIAL PRIMARY KEY,
    id_requisicion INTEGER NOT NULL,
    id_detalle_srm VARCHAR(50),
    consecutivo_bien_servicio VARCHAR(50),
    descripcion VARCHAR(1000) NOT NULL,
    cantidad DECIMAL(18,6) NOT NULL,
    id_unidad_medida INTEGER NOT NULL,
    precio_unitario DECIMAL(18,2) NOT NULL,
    subtotal DECIMAL(18,2) NOT NULL,
    iva DECIMAL(18,2) NOT NULL DEFAULT 0,
    total DECIMAL(18,2) NOT NULL,
    clave_ramo VARCHAR(20),
    clave_partida VARCHAR(20),
    clave_proyecto VARCHAR(50),
    clave_presupuestal_completa VARCHAR(200),
    CONSTRAINT fk_requisicion_detalle_requisicion FOREIGN KEY (id_requisicion)
        REFERENCES requisicion(id_requisicion) ON DELETE CASCADE,
    CONSTRAINT fk_requisicion_detalle_unidad_medida FOREIGN KEY (id_unidad_medida)
        REFERENCES cat_unidad_medida(id_unidad_medida)
);

COMMENT ON TABLE requisicion_detalle IS 'Detalle de Requisición';

CREATE INDEX idx_requisicion_detalle_requisicion ON requisicion_detalle(id_requisicion);

-- ========================================
-- ORDER (PEDIDO) TABLE - MAIN ENTITY
-- ========================================

CREATE TABLE pedido (
    id_pedido SERIAL PRIMARY KEY,
    folio VARCHAR(50) NOT NULL,
    consecutivo_completo VARCHAR(100),
    id_tipo_documento_pedido INTEGER NOT NULL,
    id_tipo_pedido INTEGER,
    id_proveedor INTEGER NOT NULL,
    id_procedimiento_adquisicion INTEGER,
    fecha_pedido DATE NOT NULL,
    numero_contrato VARCHAR(100),
    destinatario_factura VARCHAR(300),
    direccion_entrega VARCHAR(500),
    fecha_entrega DATE,
    tiempo_entrega VARCHAR(100),
    persona_elaboro VARCHAR(200),
    persona_autorizo VARCHAR(200),
    iniciales VARCHAR(50),
    subtotal DECIMAL(18,2) NOT NULL DEFAULT 0,
    total_iva DECIMAL(18,2) NOT NULL DEFAULT 0,
    total_retenciones DECIMAL(18,2) NOT NULL DEFAULT 0,
    monto_total DECIMAL(18,2) NOT NULL DEFAULT 0,
    id_estado_pedido INTEGER NOT NULL,
    id_estado_surtido INTEGER,
    observaciones VARCHAR(2000),
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    hora_registro TIME NOT NULL DEFAULT CURRENT_TIME,
    id_usuario_registro INTEGER NOT NULL,
    fecha_modifica DATE,
    hora_modifica TIME,
    id_usuario_modifica INTEGER,
    fecha_aprueba DATE,
    hora_aprueba TIME,
    id_usuario_aprueba INTEGER,
    id_archivo_firma INTEGER,
    CONSTRAINT uk_pedido_folio UNIQUE (folio),
    CONSTRAINT fk_pedido_tipo_documento FOREIGN KEY (id_tipo_documento_pedido)
        REFERENCES cat_tipo_documento_pedido(id_tipo_documento_pedido),
    CONSTRAINT fk_pedido_tipo_pedido FOREIGN KEY (id_tipo_pedido)
        REFERENCES cat_tipo_pedido(id_tipo_pedido),
    CONSTRAINT fk_pedido_proveedor FOREIGN KEY (id_proveedor)
        REFERENCES cat_proveedor(id_proveedor),
    CONSTRAINT fk_pedido_procedimiento FOREIGN KEY (id_procedimiento_adquisicion)
        REFERENCES procedimiento_adquisicion(id_procedimiento_adquisicion),
    CONSTRAINT fk_pedido_estado_pedido FOREIGN KEY (id_estado_pedido)
        REFERENCES cat_estado_pedido(id_estado_pedido),
    CONSTRAINT fk_pedido_estado_surtido FOREIGN KEY (id_estado_surtido)
        REFERENCES cat_estado_surtido(id_estado_surtido),
    CONSTRAINT fk_pedido_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user),
    CONSTRAINT fk_pedido_usuario_modifica FOREIGN KEY (id_usuario_modifica)
        REFERENCES spartan_user(id_user),
    CONSTRAINT fk_pedido_usuario_aprueba FOREIGN KEY (id_usuario_aprueba)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE pedido IS 'Pedido';

CREATE INDEX idx_pedido_folio ON pedido(folio);
CREATE INDEX idx_pedido_fecha ON pedido(fecha_pedido);
CREATE INDEX idx_pedido_proveedor ON pedido(id_proveedor);
CREATE INDEX idx_pedido_estado ON pedido(id_estado_pedido);
CREATE INDEX idx_pedido_estado_surtido ON pedido(id_estado_surtido);
CREATE INDEX idx_pedido_procedimiento ON pedido(id_procedimiento_adquisicion);

-- ========================================
-- ORDER DETAIL TABLE
-- ========================================

CREATE TABLE pedido_detalle (
    id_pedido_detalle SERIAL PRIMARY KEY,
    id_pedido INTEGER NOT NULL,
    id_requisicion_detalle VARCHAR(50),
    id_requisicion INTEGER,
    clave_presupuestal VARCHAR(100),
    nombre_partida VARCHAR(300),
    id_insumo INTEGER NOT NULL,
    descripcion VARCHAR(1000) NOT NULL,
    numero_partida VARCHAR(50),
    anio INTEGER,
    cantidad DECIMAL(18,6) NOT NULL,
    cantidad_surtida DECIMAL(18,6) DEFAULT 0,
    unidad VARCHAR(50) NOT NULL,
    precio_unitario DECIMAL(18,2) NOT NULL,
    subtotal DECIMAL(18,2) NOT NULL,
    iva DECIMAL(18,6) NOT NULL DEFAULT 0,
    retenciones DECIMAL(18,2) NOT NULL DEFAULT 0,
    total DECIMAL(18,2) NOT NULL,
    CONSTRAINT fk_pedido_detalle_pedido FOREIGN KEY (id_pedido)
        REFERENCES pedido(id_pedido) ON DELETE CASCADE,
    CONSTRAINT fk_pedido_detalle_requisicion FOREIGN KEY (id_requisicion)
        REFERENCES requisicion(id_requisicion),
    CONSTRAINT fk_pedido_detalle_insumo FOREIGN KEY (id_insumo)
        REFERENCES cat_insumo(id_insumo)
);

COMMENT ON TABLE pedido_detalle IS 'Detalle del Pedido';

CREATE INDEX idx_pedido_detalle_pedido ON pedido_detalle(id_pedido);
CREATE INDEX idx_pedido_detalle_requisicion ON pedido_detalle(id_requisicion);
CREATE INDEX idx_pedido_detalle_insumo ON pedido_detalle(id_insumo);

-- ========================================
-- ORDER BUDGET KEYS TABLE
-- ========================================

CREATE TABLE pedido_claves_presupuestales (
    id_pedido_clave SERIAL PRIMARY KEY,
    id_pedido INTEGER NOT NULL,
    clave_presupuestal VARCHAR(100) NOT NULL,
    descripcion VARCHAR(500),
    monto_asignado DECIMAL(18,2) NOT NULL,
    CONSTRAINT fk_pedido_clave_pedido FOREIGN KEY (id_pedido)
        REFERENCES pedido(id_pedido) ON DELETE CASCADE
);

COMMENT ON TABLE pedido_claves_presupuestales IS 'Claves Presupuestales del Pedido';

CREATE INDEX idx_pedido_clave_pedido ON pedido_claves_presupuestales(id_pedido);

-- ========================================
-- FEDERAL CONTRACT TABLE
-- ========================================

CREATE TABLE contrato_federal (
    id_contrato_federal SERIAL PRIMARY KEY,
    numero_contrato VARCHAR(100) NOT NULL,
    id_tipo_documento_federal INTEGER NOT NULL,
    id_proveedor INTEGER NOT NULL,
    id_pedido INTEGER,
    fecha_pedido DATE NOT NULL,
    destinatario_factura VARCHAR(300),
    tiempo_entrega VARCHAR(100),
    persona_elaboro VARCHAR(200) NOT NULL,
    persona_autorizo VARCHAR(200) NOT NULL,
    iniciales VARCHAR(50) NOT NULL,
    monto_total DECIMAL(18,2) NOT NULL,
    observaciones VARCHAR(2000),
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    hora_registro TIME NOT NULL DEFAULT CURRENT_TIME,
    id_usuario_registro INTEGER NOT NULL,
    fecha_modifica DATE,
    hora_modifica TIME,
    id_usuario_modifica INTEGER,
    CONSTRAINT uk_contrato_federal_numero UNIQUE (numero_contrato),
    CONSTRAINT fk_contrato_federal_tipo_documento FOREIGN KEY (id_tipo_documento_federal)
        REFERENCES cat_tipo_documento_federal(id_tipo_documento_federal),
    CONSTRAINT fk_contrato_federal_proveedor FOREIGN KEY (id_proveedor)
        REFERENCES cat_proveedor(id_proveedor),
    CONSTRAINT fk_contrato_federal_pedido FOREIGN KEY (id_pedido)
        REFERENCES pedido(id_pedido),
    CONSTRAINT fk_contrato_federal_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user),
    CONSTRAINT fk_contrato_federal_usuario_modifica FOREIGN KEY (id_usuario_modifica)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE contrato_federal IS 'Contrato Federal';

CREATE INDEX idx_contrato_federal_numero ON contrato_federal(numero_contrato);
CREATE INDEX idx_contrato_federal_proveedor ON contrato_federal(id_proveedor);

-- ========================================
-- FEDERAL BUDGET KEY TABLE
-- ========================================

CREATE TABLE clave_presupuestal_federal (
    id_clave_presupuestal_federal SERIAL PRIMARY KEY,
    id_contrato_federal INTEGER NOT NULL,
    anio INTEGER NOT NULL,
    ramo VARCHAR(20) NOT NULL,
    partida VARCHAR(20) NOT NULL,
    proyecto VARCHAR(50),
    componente VARCHAR(50),
    actividad VARCHAR(50),
    clave_completa VARCHAR(200) NOT NULL,
    id_fuente_financiamiento INTEGER,
    observaciones VARCHAR(1000),
    CONSTRAINT fk_clave_federal_contrato FOREIGN KEY (id_contrato_federal)
        REFERENCES contrato_federal(id_contrato_federal) ON DELETE CASCADE,
    CONSTRAINT fk_clave_federal_fuente FOREIGN KEY (id_fuente_financiamiento)
        REFERENCES cat_fuente_financiamiento(id_fuente_financiamiento)
);

COMMENT ON TABLE clave_presupuestal_federal IS 'Clave Presupuestal Federal';

CREATE INDEX idx_clave_federal_contrato ON clave_presupuestal_federal(id_contrato_federal);

-- ========================================
-- WAREHOUSE ENTRY TABLE
-- ========================================

CREATE TABLE entrada_almacen (
    id_entrada_almacen SERIAL PRIMARY KEY,
    folio VARCHAR(50) NOT NULL,
    fecha_vale DATE NOT NULL,
    id_pedido INTEGER NOT NULL,
    id_proveedor INTEGER NOT NULL,
    numero_factura VARCHAR(100),
    fecha_factura DATE,
    clave_presupuestal VARCHAR(100),
    descripcion VARCHAR(1000),
    cantidad DECIMAL(18,6),
    costo_unitario DECIMAL(18,2),
    unidad_medida VARCHAR(50),
    monto_total DECIMAL(18,2),
    id_estado_entrada INTEGER NOT NULL,
    fecha_entrada DATE NOT NULL DEFAULT CURRENT_DATE,
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    hora_registro TIME NOT NULL DEFAULT CURRENT_TIME,
    id_usuario_registro INTEGER NOT NULL,
    CONSTRAINT uk_entrada_almacen_folio UNIQUE (folio),
    CONSTRAINT fk_entrada_almacen_pedido FOREIGN KEY (id_pedido)
        REFERENCES pedido(id_pedido),
    CONSTRAINT fk_entrada_almacen_proveedor FOREIGN KEY (id_proveedor)
        REFERENCES cat_proveedor(id_proveedor),
    CONSTRAINT fk_entrada_almacen_estado FOREIGN KEY (id_estado_entrada)
        REFERENCES cat_estado_entrada_almacen(id_estado_entrada),
    CONSTRAINT fk_entrada_almacen_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE entrada_almacen IS 'Entrada de Almacén';

CREATE INDEX idx_entrada_almacen_folio ON entrada_almacen(folio);
CREATE INDEX idx_entrada_almacen_pedido ON entrada_almacen(id_pedido);

-- ========================================
-- PROVIDER VALIDATION TABLE
-- ========================================

CREATE TABLE cuadro_validacion_proveedor_propuesta (
    id_cuadro_validacion SERIAL PRIMARY KEY,
    id_procedimiento_adquisicion INTEGER NOT NULL,
    id_requisicion INTEGER NOT NULL,
    id_proveedor INTEGER NOT NULL,
    monto_propuesta DECIMAL(18,2) NOT NULL,
    evaluacion_tecnica VARCHAR(500),
    evaluacion_economica VARCHAR(500),
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    id_usuario_registro INTEGER NOT NULL,
    CONSTRAINT fk_cuadro_validacion_procedimiento FOREIGN KEY (id_procedimiento_adquisicion)
        REFERENCES procedimiento_adquisicion(id_procedimiento_adquisicion),
    CONSTRAINT fk_cuadro_validacion_requisicion FOREIGN KEY (id_requisicion)
        REFERENCES requisicion(id_requisicion),
    CONSTRAINT fk_cuadro_validacion_proveedor FOREIGN KEY (id_proveedor)
        REFERENCES cat_proveedor(id_proveedor),
    CONSTRAINT fk_cuadro_validacion_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE cuadro_validacion_proveedor_propuesta IS 'Cuadro de Validación de Propuestas de Proveedores';

CREATE INDEX idx_cuadro_validacion_procedimiento ON cuadro_validacion_proveedor_propuesta(id_procedimiento_adquisicion);

-- ========================================
-- PROVIDER DOCUMENTS TABLE
-- ========================================

CREATE TABLE documento_proveedor (
    id_documento_proveedor SERIAL PRIMARY KEY,
    id_proveedor INTEGER NOT NULL,
    id_tipo_documento INTEGER NOT NULL,
    archivo_path VARCHAR(500),
    fecha_vigencia DATE,
    estado_vigencia VARCHAR(50),
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    id_usuario_registro INTEGER NOT NULL,
    CONSTRAINT fk_documento_proveedor_proveedor FOREIGN KEY (id_proveedor)
        REFERENCES cat_proveedor(id_proveedor) ON DELETE CASCADE,
    CONSTRAINT fk_documento_proveedor_tipo_documento FOREIGN KEY (id_tipo_documento)
        REFERENCES cat_tipo_documento(id_tipo_documento),
    CONSTRAINT fk_documento_proveedor_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE documento_proveedor IS 'Documento del Proveedor';

CREATE INDEX idx_documento_proveedor_proveedor ON documento_proveedor(id_proveedor);

-- ========================================
-- PROCEDURE PROVIDERS TABLE
-- ========================================

CREATE TABLE procedimiento_proveedores (
    id_procedimiento_proveedor SERIAL PRIMARY KEY,
    id_procedimiento_adquisicion INTEGER NOT NULL,
    id_proveedor INTEGER NOT NULL,
    fecha_asignacion DATE NOT NULL DEFAULT CURRENT_DATE,
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    id_usuario_registro INTEGER NOT NULL,
    CONSTRAINT fk_procedimiento_prov_procedimiento FOREIGN KEY (id_procedimiento_adquisicion)
        REFERENCES procedimiento_adquisicion(id_procedimiento_adquisicion) ON DELETE CASCADE,
    CONSTRAINT fk_procedimiento_prov_proveedor FOREIGN KEY (id_proveedor)
        REFERENCES cat_proveedor(id_proveedor),
    CONSTRAINT fk_procedimiento_prov_usuario_registro FOREIGN KEY (id_usuario_registro)
        REFERENCES spartan_user(id_user),
    CONSTRAINT uk_procedimiento_proveedor UNIQUE (id_procedimiento_adquisicion, id_proveedor)
);

COMMENT ON TABLE procedimiento_proveedores IS 'Proveedores Asignados al Procedimiento';

CREATE INDEX idx_procedimiento_prov_procedimiento ON procedimiento_proveedores(id_procedimiento_adquisicion);

-- ========================================
-- LEGAL OPINION TABLE
-- ========================================

CREATE TABLE dictamen_juridico (
    id_dictamen_juridico SERIAL PRIMARY KEY,
    id_procedimiento_adquisicion INTEGER NOT NULL,
    resultado_dictamen VARCHAR(100) NOT NULL,
    observaciones VARCHAR(2000),
    archivo_dictamen_path VARCHAR(500),
    fecha_emision DATE NOT NULL,
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    id_usuario_abogado INTEGER NOT NULL,
    CONSTRAINT fk_dictamen_procedimiento FOREIGN KEY (id_procedimiento_adquisicion)
        REFERENCES procedimiento_adquisicion(id_procedimiento_adquisicion),
    CONSTRAINT fk_dictamen_usuario_abogado FOREIGN KEY (id_usuario_abogado)
        REFERENCES spartan_user(id_user)
);

COMMENT ON TABLE dictamen_juridico IS 'Dictamen Jurídico';

CREATE INDEX idx_dictamen_procedimiento ON dictamen_juridico(id_procedimiento_adquisicion);
