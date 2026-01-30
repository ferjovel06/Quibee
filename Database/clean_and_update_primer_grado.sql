-- Script para LIMPIAR completamente y reinsertar temas de primer grado
-- Fecha: 2026-01-28

USE quibee_db;

-- ============================================
-- LIMPIAR TODOS LOS TEMAS DE NIVEL 1 (sin importar cómo estén referenciados)
-- ============================================
DELETE FROM TOPIC WHERE id_level = 1;

-- También limpiar por si hay alguno con referencia incorrecta
DELETE FROM TOPIC WHERE id_level IN (SELECT id_level FROM LEVEL WHERE level_number = 1);

-- Verificar que se limpiaron
SELECT COUNT(*) as 'Temas nivel 1 después de DELETE' FROM TOPIC WHERE id_level = 1;

-- ============================================
-- ASEGURAR QUE EXISTE EL NIVEL 1 (PRIMER GRADO)
-- ============================================
INSERT IGNORE INTO LEVEL (id_level, level_number, level_name, description, is_active, created_at, updated_at)
VALUES (1, 1, 'Primer Grado', 'Primer grado de educación primaria', TRUE, NOW(), NOW());

-- Actualizar level_number si ya existe el registro
UPDATE LEVEL SET level_number = 1 WHERE id_level = 1;

-- ============================================
-- INSERTAR 19 TEMAS DE PRIMER GRADO
-- Camino vertical tipo serpentina (zigzag hacia abajo)
-- Canvas: 1200x2000 con scroll vertical
-- ============================================

INSERT INTO TOPIC (id_level, topic_name, description, order_index, icon, position_x, position_y, icon_width, icon_height, text_on_left, text_on_right, rotation_angle, is_active, created_at, updated_at) VALUES
-- Serpentina derecha → izquierda → derecha
(1, 'Conozcamos los números del 0 al 50', 'Tema 1:\nNúmeros del 0 al 50', 1, 'avares://Quibee/Assets/Images/SmallStar.png', 850, 50, 90, 90, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Relacionemos números y objetos', 'Tema 2:\nNúmeros y objetos', 2, 'avares://Quibee/Assets/Images/Moon.png', 650, 150, 95, 95, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Comparemos cantidades', 'Tema 3:\nComparemos cantidades', 3, 'avares://Quibee/Assets/Images/LilacPlanet2.png', 450, 250, 90, 90, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Suma', 'Tema 4:\nSuma', 4, 'avares://Quibee/Assets/Images/Meteor.png', 250, 350, 85, 85, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Resta', 'Tema 5:\nResta', 5, 'avares://Quibee/Assets/Images/Earth2.png', 150, 480, 90, 90, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Números cardinales del 1 al 10', 'Tema 6:\nCardinales 1 al 10', 6, 'avares://Quibee/Assets/Images/Saturn2.png', 350, 600, 95, 95, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Partes curvas y planas en objetos', 'Tema 7:\nCurvas y planas', 7, 'avares://Quibee/Assets/Images/3.png', 550, 700, 85, 85, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Agrupación de objetos por formas', 'Tema 8:\nAgrupación por formas', 8, 'avares://Quibee/Assets/Images/Robot.png', 750, 800, 90, 90, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Líneas abiertas y cerradas', 'Tema 9:\nLíneas abiertas/cerradas', 9, 'avares://Quibee/Assets/Images/SmallStar.png', 900, 920, 85, 85, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'La recta numérica', 'Tema 10:\nLa recta numérica', 10, 'avares://Quibee/Assets/Images/Moon.png', 700, 1030, 90, 90, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Números del 10 al 50', 'Tema 11:\nNúmeros 10 al 50', 11, 'avares://Quibee/Assets/Images/LilacPlanet2.png', 500, 1140, 95, 95, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Números del 51 al 100', 'Tema 12:\nNúmeros 51 al 100', 12, 'avares://Quibee/Assets/Images/Meteor.png', 300, 1250, 85, 85, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Unidades y decenas', 'Tema 13:\nUnidades y decenas', 13, 'avares://Quibee/Assets/Images/Earth2.png', 200, 1370, 90, 90, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'El número 100', 'Tema 14:\nEl número 100', 14, 'avares://Quibee/Assets/Images/Saturn2.png', 400, 1490, 95, 95, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'La recta numérica hasta 100', 'Tema 15:\nRecta hasta 100', 15, 'avares://Quibee/Assets/Images/3.png', 600, 1600, 85, 85, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Comparación de Números hasta el 100', 'Tema 16:\nComparación hasta 100', 16, 'avares://Quibee/Assets/Images/Robot.png', 800, 1710, 90, 90, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Comparación de longitudes', 'Tema 17:\nComparación longitudes', 17, 'avares://Quibee/Assets/Images/SmallStar.png', 950, 1830, 85, 85, FALSE, TRUE, 0, TRUE, NOW(), NOW()),
(1, 'Medimos usando nuestro cuerpo', 'Tema 18:\nMedimos con el cuerpo', 18, 'avares://Quibee/Assets/Images/Moon.png', 650, 1920, 90, 90, TRUE, FALSE, 0, TRUE, NOW(), NOW()),
(1, 'Lección de repaso', 'Tema 19:\nLección de repaso', 19, 'avares://Quibee/Assets/Images/LilacPlanet2.png', 400, 2050, 100, 100, FALSE, TRUE, 0, TRUE, NOW(), NOW());

-- Verificar inserción
SELECT COUNT(*) as 'Total Temas Primer Grado' FROM TOPIC WHERE id_level = 1;
SELECT order_index, topic_name, position_x, position_y FROM TOPIC WHERE id_level = 1 ORDER BY order_index;
