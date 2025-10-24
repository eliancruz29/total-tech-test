-- ========================================
-- Migration 001: Create Catalog Tables
-- Sistema de Adquisiciones CAASIM
-- ========================================

-- Enable UUID extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- ========================================
-- SIMPLE CATALOG TABLES
-- ========================================

-- Estados de Requisición
CREATE TABLE cat_estado_requisicion (
    id_estado_requisicion SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    color_badge VARCHAR(20),
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_estado_requisicion_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_estado_requisicion IS 'Catálogo de Estados de Requisición';
COMMENT ON COLUMN cat_estado_requisicion.id_estado_requisicion IS 'ID Estado Requisición';
COMMENT ON COLUMN cat_estado_requisicion.descripcion IS 'Descripción';
COMMENT ON COLUMN cat_estado_requisicion.color_badge IS 'Color Badge';
COMMENT ON COLUMN cat_estado_requisicion.activo IS 'Activo';

-- Estados de Pedido
CREATE TABLE cat_estado_pedido (
    id_estado_pedido SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    color_badge VARCHAR(20),
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_estado_pedido_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_estado_pedido IS 'Catálogo de Estados de Pedido';

-- Tipos de Documento de Pedido
CREATE TABLE cat_tipo_documento_pedido (
    id_tipo_documento_pedido SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_tipo_documento_pedido_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_tipo_documento_pedido IS 'Catálogo de Tipos de Documento de Pedido';

-- Tipos de Documento Federal
CREATE TABLE cat_tipo_documento_federal (
    id_tipo_documento_federal SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    prefijo VARCHAR(5) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_tipo_documento_federal_descripcion UNIQUE (descripcion),
    CONSTRAINT uk_tipo_documento_federal_prefijo UNIQUE (prefijo)
);

COMMENT ON TABLE cat_tipo_documento_federal IS 'Catálogo de Tipos de Documento Federal';

-- Fuentes de Financiamiento Federal
CREATE TABLE cat_fuente_financiamiento (
    id_fuente_financiamiento SERIAL PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL,
    descripcion VARCHAR(300) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_fuente_financiamiento_codigo UNIQUE (codigo)
);

COMMENT ON TABLE cat_fuente_financiamiento IS 'Catálogo de Fuentes de Financiamiento Federal';

-- Modalidades de Compra
CREATE TABLE cat_modalidad_compra (
    id_modalidad_compra SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    prefijo VARCHAR(5) NOT NULL,
    monto_minimo DECIMAL(18,2),
    monto_maximo DECIMAL(18,2),
    plazo_minimo_dias INTEGER,
    requiere_publicacion BOOLEAN NOT NULL DEFAULT false,
    color_badge VARCHAR(20),
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_modalidad_compra_descripcion UNIQUE (descripcion),
    CONSTRAINT uk_modalidad_compra_prefijo UNIQUE (prefijo)
);

COMMENT ON TABLE cat_modalidad_compra IS 'Catálogo de Modalidades de Compra';

-- Estados de Procedimiento de Adquisición
CREATE TABLE cat_estado_procedimiento (
    id_estado_procedimiento SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    color_badge VARCHAR(20),
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_estado_procedimiento_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_estado_procedimiento IS 'Catálogo de Estados de Procedimiento de Adquisición';

-- Fuentes Presupuestales
CREATE TABLE cat_fuente_presupuestal (
    id_fuente_presupuestal SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_fuente_presupuestal_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_fuente_presupuestal IS 'Catálogo de Fuentes Presupuestales';

-- Niveles de Urgencia
CREATE TABLE cat_nivel_urgencia (
    id_nivel_urgencia SERIAL PRIMARY KEY,
    descripcion VARCHAR(50) NOT NULL,
    color_badge VARCHAR(20),
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_nivel_urgencia_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_nivel_urgencia IS 'Catálogo de Niveles de Urgencia';

-- Categorías de Requisición
CREATE TABLE cat_categoria_requisicion (
    id_categoria_requisicion SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_categoria_requisicion_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_categoria_requisicion IS 'Catálogo de Categorías de Requisición';

-- Unidades de Medida
CREATE TABLE cat_unidad_medida (
    id_unidad_medida SERIAL PRIMARY KEY,
    codigo VARCHAR(10) NOT NULL,
    descripcion VARCHAR(100) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_unidad_medida_codigo UNIQUE (codigo)
);

COMMENT ON TABLE cat_unidad_medida IS 'Catálogo de Unidades de Medida';

-- Categorías de Insumo
CREATE TABLE cat_categoria_insumo (
    id_categoria_insumo SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    color_badge VARCHAR(20),
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_categoria_insumo_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_categoria_insumo IS 'Catálogo de Categorías de Insumo';

-- Estados de Entrada de Almacén
CREATE TABLE cat_estado_entrada_almacen (
    id_estado_entrada SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    color_badge VARCHAR(20),
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_estado_entrada_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_estado_entrada_almacen IS 'Catálogo de Estados de Entrada de Almacén';

-- Tipos de Pedido
CREATE TABLE cat_tipo_pedido (
    id_tipo_pedido SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_tipo_pedido_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_tipo_pedido IS 'Catálogo de Tipos de Pedido';

-- Estados de Surtido
CREATE TABLE cat_estado_surtido (
    id_estado_surtido SERIAL PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL,
    color_badge VARCHAR(20),
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_estado_surtido_descripcion UNIQUE (descripcion)
);

COMMENT ON TABLE cat_estado_surtido IS 'Catálogo de Estados de Surtido';

-- Tipos de Documento (para proveedores)
CREATE TABLE cat_tipo_documento (
    id_tipo_documento SERIAL PRIMARY KEY,
    nombre VARCHAR(200) NOT NULL,
    descripcion VARCHAR(500),
    aplica_persona_fisica BOOLEAN NOT NULL DEFAULT false,
    aplica_persona_moral BOOLEAN NOT NULL DEFAULT false,
    es_obligatorio BOOLEAN NOT NULL DEFAULT false,
    activo BOOLEAN NOT NULL DEFAULT true,
    CONSTRAINT uk_tipo_documento_nombre UNIQUE (nombre)
);

COMMENT ON TABLE cat_tipo_documento IS 'Catálogo de Tipos de Documento';

-- Departamentos
CREATE TABLE cat_departamento (
    id_departamento SERIAL PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL,
    nombre VARCHAR(200) NOT NULL,
    descripcion VARCHAR(500),
    activo BOOLEAN NOT NULL DEFAULT true,
    fecha_registro DATE NOT NULL DEFAULT CURRENT_DATE,
    id_usuario_registro INTEGER NOT NULL,
    CONSTRAINT uk_departamento_codigo UNIQUE (codigo)
);

COMMENT ON TABLE cat_departamento IS 'Catálogo de Departamentos';

-- ========================================
-- INDEXES FOR CATALOG TABLES
-- ========================================

CREATE INDEX idx_cat_estado_requisicion_activo ON cat_estado_requisicion(activo);
CREATE INDEX idx_cat_estado_pedido_activo ON cat_estado_pedido(activo);
CREATE INDEX idx_cat_tipo_documento_pedido_activo ON cat_tipo_documento_pedido(activo);
CREATE INDEX idx_cat_tipo_documento_federal_activo ON cat_tipo_documento_federal(activo);
CREATE INDEX idx_cat_fuente_financiamiento_activo ON cat_fuente_financiamiento(activo);
CREATE INDEX idx_cat_modalidad_compra_activo ON cat_modalidad_compra(activo);
CREATE INDEX idx_cat_estado_procedimiento_activo ON cat_estado_procedimiento(activo);
CREATE INDEX idx_cat_fuente_presupuestal_activo ON cat_fuente_presupuestal(activo);
CREATE INDEX idx_cat_nivel_urgencia_activo ON cat_nivel_urgencia(activo);
CREATE INDEX idx_cat_categoria_requisicion_activo ON cat_categoria_requisicion(activo);
CREATE INDEX idx_cat_unidad_medida_activo ON cat_unidad_medida(activo);
CREATE INDEX idx_cat_categoria_insumo_activo ON cat_categoria_insumo(activo);
CREATE INDEX idx_cat_estado_entrada_almacen_activo ON cat_estado_entrada_almacen(activo);
CREATE INDEX idx_cat_tipo_pedido_activo ON cat_tipo_pedido(activo);
CREATE INDEX idx_cat_estado_surtido_activo ON cat_estado_surtido(activo);
CREATE INDEX idx_cat_tipo_documento_activo ON cat_tipo_documento(activo);
CREATE INDEX idx_cat_departamento_activo ON cat_departamento(activo);
