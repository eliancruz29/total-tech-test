-- ========================================
-- Migration 003: Seed Initial Data
-- Sistema de Adquisiciones CAASIM
-- ========================================

-- ========================================
-- SEED USERS
-- ========================================
-- Default password: "admin123" (hashed with bcrypt)
-- You should change this in production!

INSERT INTO spartan_user (name, email, password_hash, is_active) VALUES
('Administrador Sistema', 'admin@caasim.gob.mx', '$2a$11$vZ8qLKHPvXDKJX7M9wYl3.rO7ZB9yX7j/GKOmYJQqHzJqGEp5L8Eq', true),
('Juan García Hernández', 'jgarcia@caasim.gob.mx', '$2a$11$vZ8qLKHPvXDKJX7M9wYl3.rO7ZB9yX7j/GKOmYJQqHzJqGEp5L8Eq', true),
('María Rodríguez López', 'mrodriguez@caasim.gob.mx', '$2a$11$vZ8qLKHPvXDKJX7M9wYl3.rO7ZB9yX7j/GKOmYJQqHzJqGEp5L8Eq', true),
('Carlos Álvarez Martínez', 'calvarez@caasim.gob.mx', '$2a$11$vZ8qLKHPvXDKJX7M9wYl3.rO7ZB9yX7j/GKOmYJQqHzJqGEp5L8Eq', true);

-- ========================================
-- SEED CATALOG: ESTADOS DE REQUISICIÓN
-- ========================================

INSERT INTO cat_estado_requisicion (descripcion, color_badge, activo) VALUES
('Pendiente de Asignación', '#FFC107', true),
('Asignada', '#2196F3', true),
('En Proceso', '#FF9800', true),
('Completada', '#4CAF50', true),
('Cancelada', '#F44336', true);

-- ========================================
-- SEED CATALOG: ESTADOS DE PEDIDO
-- ========================================

INSERT INTO cat_estado_pedido (descripcion, color_badge, activo) VALUES
('Borrador', '#9E9E9E', true),
('Activo', '#2196F3', true),
('Pendiente', '#FFC107', true),
('Aprobado', '#4CAF50', true),
('En Proceso', '#FF9800', true),
('Completado', '#4CAF50', true),
('Cancelado', '#F44336', true);

-- ========================================
-- SEED CATALOG: TIPOS DE DOCUMENTO DE PEDIDO
-- ========================================

INSERT INTO cat_tipo_documento_pedido (descripcion, activo) VALUES
('Orden de Compra', true),
('Pedido', true),
('Orden de Servicio', true),
('Pedido de Servicio', true);

-- ========================================
-- SEED CATALOG: TIPOS DE DOCUMENTO FEDERAL
-- ========================================

INSERT INTO cat_tipo_documento_federal (descripcion, prefijo, activo) VALUES
('Contrato', 'CTR', true),
('Convenio', 'CNV', true),
('Pedido', 'PED', true),
('Requisición', 'REQ', true);

-- ========================================
-- SEED CATALOG: FUENTES DE FINANCIAMIENTO
-- ========================================

INSERT INTO cat_fuente_financiamiento (codigo, descripcion, activo) VALUES
('FAIS', 'Fondo de Aportaciones para la Infraestructura Social', true),
('FORTAMUN', 'Fondo de Aportaciones para el Fortalecimiento de los Municipios', true),
('FASSA', 'Fondo de Aportaciones para los Servicios de Salud', true),
('FAEB', 'Fondo de Aportaciones para la Educación Básica', true),
('FAM', 'Fondo de Aportaciones Múltiples', true),
('FASP', 'Fondo de Aportaciones para la Seguridad Pública', true);

-- ========================================
-- SEED CATALOG: MODALIDADES DE COMPRA
-- ========================================

INSERT INTO cat_modalidad_compra (descripcion, prefijo, monto_minimo, monto_maximo, plazo_minimo_dias, requiere_publicacion, color_badge, activo) VALUES
('Licitación Pública', 'LP', 950000.00, NULL, 30, true, '#4CAF50', true),
('Invitación a Tres Proveedores', 'I3P', 350000.00, 949999.99, 15, true, '#2196F3', true),
('Adjudicación Directa', 'AD', 0.00, 349999.99, 5, false, '#FF9800', true),
('Pago de Derechos', 'PD', 0.00, NULL, 0, false, '#9C27B0', true);

-- ========================================
-- SEED CATALOG: ESTADOS DE PROCEDIMIENTO
-- ========================================

INSERT INTO cat_estado_procedimiento (descripcion, color_badge, activo) VALUES
('Borrador', '#9E9E9E', true),
('En Clasificación', '#2196F3', true),
('Asignado', '#00BCD4', true),
('En Validación', '#FF9800', true),
('Validado', '#4CAF50', true),
('En Compra', '#FFC107', true),
('Completado', '#4CAF50', true),
('Cancelado', '#F44336', true);

-- ========================================
-- SEED CATALOG: FUENTES PRESUPUESTALES
-- ========================================

INSERT INTO cat_fuente_presupuestal (descripcion, activo) VALUES
('Recursos Estatales', true),
('Recursos Federales', true),
('Recursos Mixtos', true);

-- ========================================
-- SEED CATALOG: NIVELES DE URGENCIA
-- ========================================

INSERT INTO cat_nivel_urgencia (descripcion, color_badge, activo) VALUES
('Urgente', '#F44336', true),
('Alta', '#FF9800', true),
('Media', '#FFC107', true),
('Baja', '#4CAF50', true);

-- ========================================
-- SEED CATALOG: CATEGORÍAS DE REQUISICIÓN
-- ========================================

INSERT INTO cat_categoria_requisicion (descripcion, activo) VALUES
('Bienes Muebles', true),
('Servicios', true),
('Obras', true),
('Arrendamientos', true);

-- ========================================
-- SEED CATALOG: UNIDADES DE MEDIDA
-- ========================================

INSERT INTO cat_unidad_medida (codigo, descripcion, activo) VALUES
('PZA', 'PIEZA', true),
('KG', 'KILOGRAMO', true),
('LT', 'LITRO', true),
('M', 'METRO', true),
('M2', 'METRO CUADRADO', true),
('M3', 'METRO CÚBICO', true),
('SRV', 'SERVICIO', true),
('PAQ', 'PAQUETE', true),
('CJA', 'CAJA', true),
('ROL', 'ROLLO', true),
('JGO', 'JUEGO', true),
('TON', 'TONELADA', true),
('HR', 'HORA', true),
('DIA', 'DÍA', true),
('MES', 'MES', true);

-- ========================================
-- SEED CATALOG: CATEGORÍAS DE INSUMO
-- ========================================

INSERT INTO cat_categoria_insumo (descripcion, color_badge, activo) VALUES
('EQUIPO DE CÓMPUTO', '#2196F3', true),
('MOBILIARIO', '#795548', true),
('PAPELERÍA', '#FFC107', true),
('SERVICIOS', '#9C27B0', true),
('CONSUMIBLES', '#FF9800', true),
('MATERIAL DE CONSTRUCCIÓN', '#607D8B', true),
('MATERIAL ELÉCTRICO', '#FF5722', true),
('MATERIAL DE LIMPIEZA', '#4CAF50', true);

-- ========================================
-- SEED CATALOG: ESTADOS DE ENTRADA DE ALMACÉN
-- ========================================

INSERT INTO cat_estado_entrada_almacen (descripcion, color_badge, activo) VALUES
('Recibido', '#2196F3', true),
('En Verificación', '#FF9800', true),
('Aprobado', '#4CAF50', true),
('Rechazado', '#F44336', true);

-- ========================================
-- SEED CATALOG: TIPOS DE PEDIDO
-- ========================================

INSERT INTO cat_tipo_pedido (descripcion, activo) VALUES
('Normal', true),
('Urgente', true),
('Programado', true),
('Especial', true);

-- ========================================
-- SEED CATALOG: ESTADOS DE SURTIDO
-- ========================================

INSERT INTO cat_estado_surtido (descripcion, color_badge, activo) VALUES
('Completo', '#4CAF50', true),
('Pendiente', '#FFC107', true),
('Parcial', '#2196F3', true);

-- ========================================
-- SEED CATALOG: TIPOS DE DOCUMENTO
-- ========================================

INSERT INTO cat_tipo_documento (nombre, descripcion, aplica_persona_fisica, aplica_persona_moral, es_obligatorio, activo) VALUES
('RFC', 'Registro Federal de Contribuyentes', true, true, true, true),
('Acta Constitutiva', 'Acta Constitutiva de la Empresa', false, true, true, true),
('Identificación Oficial', 'Identificación Oficial del Representante Legal', true, true, true, true),
('Comprobante de Domicilio', 'Comprobante de Domicilio Fiscal', true, true, true, true),
('Constancia de Situación Fiscal', 'Constancia de Situación Fiscal SAT', true, true, true, true),
('Opinión de Cumplimiento', 'Opinión de Cumplimiento de Obligaciones Fiscales', true, true, false, true),
('Poder Notarial', 'Poder Notarial del Representante Legal', false, true, true, true),
('CURP', 'Clave Única de Registro de Población', true, false, false, true);

-- ========================================
-- SEED CATALOG: DEPARTAMENTOS
-- ========================================

INSERT INTO cat_departamento (codigo, nombre, descripcion, activo, fecha_registro, id_usuario_registro) VALUES
('ADMIN', 'Administración', 'Departamento de Administración General', true, CURRENT_DATE, 1),
('OBRAS', 'Obras Públicas', 'Departamento de Obras Públicas y Desarrollo Urbano', true, CURRENT_DATE, 1),
('SALUD', 'Salud', 'Departamento de Salud Municipal', true, CURRENT_DATE, 1),
('EDUCACION', 'Educación', 'Departamento de Educación y Cultura', true, CURRENT_DATE, 1),
('SEGURIDAD', 'Seguridad Pública', 'Departamento de Seguridad Pública y Tránsito', true, CURRENT_DATE, 1),
('FINANZAS', 'Finanzas', 'Departamento de Finanzas y Tesorería', true, CURRENT_DATE, 1);

-- ========================================
-- SEED: PROVEEDORES (SAMPLE DATA)
-- ========================================

INSERT INTO cat_proveedor (razon_social, rfc, tipo_persona, nombre_contacto, correo_electronico, telefono, direccion, activo, fecha_registro, id_usuario_registro) VALUES
('Distribuidora Industrial del Centro S.A. de C.V.', 'DIC850315ABC', 'MORAL', 'Ing. Roberto Sánchez', 'ventas@dic.com.mx', '771-123-4567', 'Av. Revolución 123, Pachuca, Hidalgo', true, CURRENT_DATE, 1),
('Suministros Técnicos Hidalgo S.A. de C.V.', 'STH920428DEF', 'MORAL', 'Lic. Ana María Torres', 'compras@sth.com.mx', '771-234-5678', 'Blvd. Colosio 456, Pachuca, Hidalgo', true, CURRENT_DATE, 1),
('Comercializadora de Equipos Gubernamentales', 'CEG780912GHI', 'MORAL', 'Arq. Fernando López', 'proyectos@ceg.gob.mx', '771-345-6789', 'Calle Hidalgo 789, Pachuca, Hidalgo', true, CURRENT_DATE, 1),
('Papelería y Suministros Oficina Total', 'PSO650203JKL', 'MORAL', 'Sr. Miguel Hernández', 'ventas@psot.com', '771-456-7890', 'Plaza Principal 101, Pachuca, Hidalgo', true, CURRENT_DATE, 1),
('Constructora y Servicios Múltiples del Estado', 'CSM890715MNO', 'MORAL', 'Ing. Patricia Ruiz', 'construccion@csme.gob.mx', '771-567-8901', 'Zona Industrial 202, Pachuca, Hidalgo', true, CURRENT_DATE, 1);

-- ========================================
-- SEED: INSUMOS (SAMPLE DATA)
-- ========================================

INSERT INTO cat_insumo (codigo, nombre, descripcion, id_unidad_medida, id_categoria_insumo, precio_referencia, activo, fecha_registro, id_usuario_registro) VALUES
('PAP-001', 'Papel bond tamaño carta', 'Papel bond tamaño carta 75gr, resma de 500 hojas', 1, 3, 125.50, true, CURRENT_DATE, 1),
('COMP-001', 'Computadora de escritorio', 'Computadora de escritorio Core i5, 8GB RAM, 1TB HDD', 1, 1, 8500.00, true, CURRENT_DATE, 1),
('PINT-001', 'Pintura vinílica blanca', 'Pintura vinílica blanca para interiores, cubeta de 19 litros', 5, 6, 450.75, true, CURRENT_DATE, 1),
('TON-001', 'Tóner para impresora láser', 'Tóner para impresora láser HP LaserJet Pro M404dn', 1, 5, 1525.00, true, CURRENT_DATE, 1),
('VAR-001', 'Varilla corrugada', 'Varilla corrugada #4 de 12 metros para construcción', 2, 6, 320.80, true, CURRENT_DATE, 1),
('DET-001', 'Detergente líquido biodegradable', 'Detergente líquido biodegradable para limpieza general', 3, 8, 28.90, true, CURRENT_DATE, 1),
('ARCH-001', 'Archivero metálico 4 gavetas', 'Archivero metálico de 4 gavetas con cerradura', 1, 2, 2850.00, true, CURRENT_DATE, 1),
('PROY-001', 'Proyector multimedia', 'Proyector multimedia 3500 lúmenes con accesorios', 1, 1, 12500.00, true, CURRENT_DATE, 1);

-- ========================================
-- SEED: CLAVES PRESUPUESTALES (SAMPLE DATA)
-- ========================================

INSERT INTO clave_presupuestal (anio, clave_ramo, nombre_ramo, clave_partida, nombre_partida, clave_proyecto, nombre_proyecto, clave_completa, presupuesto_asignado, presupuesto_disponible, presupuesto_ejercido, presupuesto_restante, activo, fecha_registro, id_usuario_registro) VALUES
(2024, '33', 'Aportaciones Federales', '2110', 'Materiales de Administración, Emisión de Documentos y Artículos Oficiales', 'PROY-2024-001', 'Fortalecimiento Administrativo', '33-2110-PROY-2024-001', 500000.00, 350000.00, 150000.00, 350000.00, true, CURRENT_DATE, 1),
(2024, '33', 'Aportaciones Federales', '2120', 'Materiales y Artículos de Construcción y de Reparación', 'PROY-2024-002', 'Obra Pública Municipal', '33-2120-PROY-2024-002', 1000000.00, 750000.00, 250000.00, 750000.00, true, CURRENT_DATE, 1),
(2024, '33', 'Aportaciones Federales', '2140', 'Materiales y Artículos Metálicos', 'PROY-2024-003', 'Infraestructura Urbana', '33-2140-PROY-2024-003', 800000.00, 600000.00, 200000.00, 600000.00, true, CURRENT_DATE, 1),
(2024, '33', 'Aportaciones Federales', '2150', 'Material Eléctrico y Electrónico', 'PROY-2024-004', 'Modernización Tecnológica', '33-2150-PROY-2024-004', 600000.00, 450000.00, 150000.00, 450000.00, true, CURRENT_DATE, 1),
(2024, '33', 'Aportaciones Federales', '2160', 'Material de Limpieza', 'PROY-2024-005', 'Servicios Generales', '33-2160-PROY-2024-005', 300000.00, 200000.00, 100000.00, 200000.00, true, CURRENT_DATE, 1),
(2024, '33', 'Aportaciones Federales', '2170', 'Materiales y Útiles de Impresión y Reproducción', 'PROY-2024-006', 'Comunicación Institucional', '33-2170-PROY-2024-006', 250000.00, 180000.00, 70000.00, 180000.00, true, CURRENT_DATE, 1);

-- ========================================
-- SEED: SAMPLE PEDIDOS (TEST DATA)
-- ========================================

-- Pedido 1: Completo
INSERT INTO pedido (folio, consecutivo_completo, id_tipo_documento_pedido, id_tipo_pedido, id_proveedor, fecha_pedido, numero_contrato, destinatario_factura, direccion_entrega, fecha_entrega, tiempo_entrega, persona_elaboro, persona_autorizo, iniciales, subtotal, total_iva, total_retenciones, monto_total, id_estado_pedido, id_estado_surtido, observaciones, fecha_registro, hora_registro, id_usuario_registro) VALUES
('PED-2024-001', 'PED-2024-001-AD-001', 1, 1, 1, '2024-01-15', NULL, 'CAASIM', 'Oficinas Centrales CAASIM', '2024-01-20', '5 días hábiles', 'Juan García Hernández', 'María Rodríguez López', 'JGH', 6275.00, 1004.00, 0, 7279.00, 6, 1, 'Entrega completa en tiempo', CURRENT_DATE, CURRENT_TIME, 1);

INSERT INTO pedido_detalle (id_pedido, clave_presupuestal, nombre_partida, id_insumo, descripcion, numero_partida, anio, cantidad, cantidad_surtida, unidad, precio_unitario, subtotal, iva, retenciones, total) VALUES
(1, '33-2110-PROY-2024-001', 'Materiales de Administración', 1, 'Papel bond tamaño carta 75gr, resma de 500 hojas', '001', 2024, 50, 50, 'PZA', 125.50, 6275.00, 1004.00, 0, 7279.00);

-- Pedido 2: Parcial
INSERT INTO pedido (folio, consecutivo_completo, id_tipo_documento_pedido, id_tipo_pedido, id_proveedor, fecha_pedido, destinatario_factura, direccion_entrega, fecha_entrega, tiempo_entrega, persona_elaboro, persona_autorizo, iniciales, subtotal, total_iva, total_retenciones, monto_total, id_estado_pedido, id_estado_surtido, observaciones, fecha_registro, hora_registro, id_usuario_registro) VALUES
('PED-2024-002', 'PED-2024-002-AD-002', 1, 2, 2, '2024-01-18', 'CAASIM', 'Departamento de Sistemas', '2024-01-25', '7 días hábiles', 'María Rodríguez López', 'Carlos Álvarez Martínez', 'MRL', 212500.00, 34000.00, 0, 246500.00, 5, 3, 'Pendiente entrega de 5 unidades', CURRENT_DATE, CURRENT_TIME, 1);

INSERT INTO pedido_detalle (id_pedido, clave_presupuestal, nombre_partida, id_insumo, descripcion, numero_partida, anio, cantidad, cantidad_surtida, unidad, precio_unitario, subtotal, iva, retenciones, total) VALUES
(2, '33-2150-PROY-2024-004', 'Material Eléctrico y Electrónico', 2, 'Computadora de escritorio Core i5, 8GB RAM, 1TB HDD', '002', 2024, 25, 20, 'PZA', 8500.00, 212500.00, 34000.00, 0, 246500.00);

-- Pedido 3: Pendiente
INSERT INTO pedido (folio, consecutivo_completo, id_tipo_documento_pedido, id_tipo_pedido, id_proveedor, fecha_pedido, destinatario_factura, direccion_entrega, tiempo_entrega, persona_elaboro, persona_autorizo, iniciales, subtotal, total_iva, total_retenciones, monto_total, id_estado_pedido, id_estado_surtido, observaciones, fecha_registro, hora_registro, id_usuario_registro) VALUES
('PED-2024-003', 'PED-2024-003-AD-003', 1, 3, 3, '2024-01-22', 'CAASIM', 'Obras Públicas', '10 días hábiles', 'Carlos Álvarez Martínez', 'Juan García Hernández', 'CAM', 45075.00, 7212.00, 0, 52287.00, 3, 2, 'En proceso de fabricación', CURRENT_DATE, CURRENT_TIME, 1);

INSERT INTO pedido_detalle (id_pedido, clave_presupuestal, nombre_partida, id_insumo, descripcion, numero_partida, anio, cantidad, cantidad_surtida, unidad, precio_unitario, subtotal, iva, retenciones, total) VALUES
(3, '33-2120-PROY-2024-002', 'Materiales y Artículos de Construcción', 3, 'Pintura vinílica blanca para interiores, cubeta de 19 litros', '003', 2024, 100, 0, 'LT', 450.75, 45075.00, 7212.00, 0, 52287.00);

-- ========================================
-- VERIFY SEEDED DATA
-- ========================================

-- Show counts
SELECT 'Users' as table_name, COUNT(*) as count FROM spartan_user
UNION ALL
SELECT 'Estados Requisición', COUNT(*) FROM cat_estado_requisicion
UNION ALL
SELECT 'Estados Pedido', COUNT(*) FROM cat_estado_pedido
UNION ALL
SELECT 'Tipos Documento Pedido', COUNT(*) FROM cat_tipo_documento_pedido
UNION ALL
SELECT 'Proveedores', COUNT(*) FROM cat_proveedor
UNION ALL
SELECT 'Insumos', COUNT(*) FROM cat_insumo
UNION ALL
SELECT 'Departamentos', COUNT(*) FROM cat_departamento
UNION ALL
SELECT 'Pedidos', COUNT(*) FROM pedido
UNION ALL
SELECT 'Pedido Detalles', COUNT(*) FROM pedido_detalle;
