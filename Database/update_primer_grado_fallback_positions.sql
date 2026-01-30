-- Script para actualizar PRIMER GRADO con posiciones del fallback + continuación vertical
-- Fecha: 2026-01-28

USE quibee_db;

-- ============================================
-- LIMPIAR TODOS LOS TEMAS DE NIVEL 1
-- ============================================
DELETE FROM TOPIC WHERE id_level = 1;

-- Verificar que se limpiaron
SELECT COUNT(*) as 'Temas nivel 1 después de DELETE' FROM TOPIC WHERE id_level = 1;

-- ============================================
-- ASEGURAR QUE EXISTE EL NIVEL 1 (PRIMER GRADO)
-- ============================================
INSERT IGNORE INTO LEVEL (id_level, level_number, level_name, description, is_active, created_at, updated_at)
VALUES (1, 1, 'Primer Grado', 'Primer grado de educación primaria', TRUE, NOW(), NOW());

UPDATE LEVEL SET level_number = 1 WHERE id_level = 1;

-- ============================================
-- INSERTAR 19 TEMAS DE PRIMER GRADO
-- Primeros 5 temas: Posiciones del fallback (que funcionaban bien)
-- Temas 6-19: Continuación del camino hacia abajo
-- Canvas: 1200x2000 con scroll vertical
-- ============================================

INSERT INTO TOPIC (id_level, topic_name, description, order_index, icon, position_x, position_y, icon_width, icon_height, text_on_left, text_on_right, rotation_angle, is_active, created_at, updated_at) VALUES
-- === TEMAS 1-5: Posiciones del fallback (ya probadas) ===
(1, 'Conozcamos los números del 0 al 50', 'Números del 0 al 50', 1, 'avares://Quibee/Assets/Images/SmallStar.png', 740, 190, 80, 80, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Relacionemos números y objetos', 'Números y objetos', 2, 'avares://Quibee/Assets/Images/LilacPlanet2.png', 580, 290, 100, 100, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Comparemos cantidades', 'Comparemos cantidades', 3, 'avares://Quibee/Assets/Images/Meteor.png', 550, 470, 90, 90, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Suma', 'Suma', 4, 'avares://Quibee/Assets/Images/Earth2.png', 270, 400, 100, 100, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Resta', 'Resta', 5, 'avares://Quibee/Assets/Images/Saturn2.png', 180, 590, 100, 100, FALSE, TRUE, 0, TRUE, NOW(), NOW()),

-- === TEMAS 6-19: Continuación hacia abajo (patrón zigzag) ===
(1, 'Números cardinales del 1 al 10', 'Cardinales 1 al 10', 6, 'avares://Quibee/Assets/Images/3.png', 180, 720, 95, 95, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Partes curvas y planas en objetos', 'Curvas y planas', 7, 'avares://Quibee/Assets/Images/Robot.png', 340, 820, 85, 85, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Agrupación de objetos por formas', 'Agrupación por formas', 8, 'avares://Quibee/Assets/Images/SmallStar.png', 370, 1000, 90, 90, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Líneas abiertas y cerradas', 'Líneas abiertas/cerradas', 9, 'avares://Quibee/Assets/Images/Star2.png', 650, 930, 85, 85, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'La recta numérica', 'La recta numérica', 10, 'avares://Quibee/Assets/Images/LilacPlanet2.png', 740, 1120, 90, 90, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Números del 10 al 50', 'Números 10 al 50', 11, 'avares://Quibee/Assets/Images/Meteor.png', 740, 1250, 95, 95, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Números del 51 al 100', 'Números 51 al 100', 12, 'avares://Quibee/Assets/Images/Earth2.png', 580, 1350, 85, 85, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Unidades y decenas', 'Unidades y decenas', 13, 'avares://Quibee/Assets/Images/Saturn2.png', 550, 1530, 90, 90, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'El número 100', 'El número 100', 14, 'avares://Quibee/Assets/Images/3.png', 270, 1460, 95, 95, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'La recta numérica hasta 100', 'Recta hasta 100', 15, 'avares://Quibee/Assets/Images/Robot.png', 180, 1650, 85, 85, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Comparación de Números hasta el 100', 'Comparación hasta 100', 16, 'avares://Quibee/Assets/Images/SmallStar.png', 180, 1820, 90, 90, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Comparación de longitudes', 'Comparación longitudes', 17, 'avares://Quibee/Assets/Images/Neptune.png', 340, 1920, 85, 85, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Medimos usando nuestro cuerpo', 'Medimos con el cuerpo', 18, 'avares://Quibee/Assets/Images/LilacPlanet2.png', 370, 2100, 90, 90, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Lección de repaso', 'Lección de repaso', 19, 'avares://Quibee/Assets/Images/Meteor.png', 650, 2030, 100, 100, FALSE, TRUE, 0, TRUE, NOW(), NOW());

-- Verificar inserción
SELECT COUNT(*) as 'Total Temas Primer Grado' FROM TOPIC WHERE id_level = 1;
SELECT order_index, topic_name, position_x, position_y, text_on_left, text_on_right FROM TOPIC WHERE id_level = 1 ORDER BY order_index;
