-- Migración: Agregar campos de posicionamiento a la tabla TOPIC
-- Fecha: 2026-01-23
-- Descripción: Agrega campos para almacenar posiciones, tamaños y alineaciones de los temas en el mapa de lecciones

USE quibee_db;

-- Agregar campos de posición
ALTER TABLE TOPIC ADD COLUMN position_x DOUBLE DEFAULT 0 AFTER order_index;
ALTER TABLE TOPIC ADD COLUMN position_y DOUBLE DEFAULT 0 AFTER position_x;

-- Agregar campos de tamaño de icono
ALTER TABLE TOPIC ADD COLUMN icon_width DOUBLE DEFAULT 100 AFTER position_y;
ALTER TABLE TOPIC ADD COLUMN icon_height DOUBLE DEFAULT 100 AFTER icon_width;

-- Agregar campos de alineación de texto
ALTER TABLE TOPIC ADD COLUMN text_on_left BOOLEAN DEFAULT FALSE AFTER icon_height;
ALTER TABLE TOPIC ADD COLUMN text_on_right BOOLEAN DEFAULT FALSE AFTER text_on_left;

-- Agregar campo de rotación
ALTER TABLE TOPIC ADD COLUMN rotation_angle DOUBLE DEFAULT 0 AFTER text_on_right;

-- Verificar que se agregaron correctamente
DESCRIBE TOPIC;
