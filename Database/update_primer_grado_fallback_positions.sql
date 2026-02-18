-- Script para actualizar PRIMER NIVEL sin columnas de posición.
-- El mapa calcula posiciones automáticamente por order_index.
-- Fecha: 2026-02-17

USE quibee_db;

DELETE FROM TOPIC WHERE id_level = 1;

INSERT IGNORE INTO LEVEL (id_level, level_number, level_name, description, is_active, created_at, updated_at)
VALUES (1, 1, 'Primer Grado', 'Tus primeros pasos con los números', TRUE, NOW(), NOW());

UPDATE LEVEL
SET level_number = 1,
    description = 'Tus primeros pasos con los números'
WHERE id_level = 1;

INSERT INTO TOPIC (id_level, topic_name, description, order_index, icon, icon_width, icon_height, is_active, created_at, updated_at) VALUES
(1, 'Conozcamos los números del 0 al 50', 'Números del 0 al 50', 1, 'avares://Quibee/Assets/Images/SmallStar.png', 80, 80, TRUE, NOW(), NOW()),
(1, 'Relacionemos números y objetos', 'Números y objetos', 2, 'avares://Quibee/Assets/Images/LilacPlanet2.png', 100, 100, TRUE, NOW(), NOW()),
(1, 'Comparemos cantidades', 'Comparemos cantidades', 3, 'avares://Quibee/Assets/Images/Meteor.png', 90, 90, TRUE, NOW(), NOW()),
(1, 'Suma', 'Suma', 4, 'avares://Quibee/Assets/Images/Earth2.png', 100, 100, TRUE, NOW(), NOW()),
(1, 'Resta', 'Resta', 5, 'avares://Quibee/Assets/Images/Saturn2.png', 100, 100, TRUE, NOW(), NOW()),
(1, 'Números cardinales del 1 al 10', 'Cardinales 1 al 10', 6, 'avares://Quibee/Assets/Images/3.png', 95, 95, TRUE, NOW(), NOW()),
(1, 'Partes curvas y planas en objetos', 'Curvas y planas', 7, 'avares://Quibee/Assets/Images/Robot.png', 85, 85, TRUE, NOW(), NOW()),
(1, 'Agrupación de objetos por formas', 'Agrupación por formas', 8, 'avares://Quibee/Assets/Images/SmallStar.png', 90, 90, TRUE, NOW(), NOW()),
(1, 'Líneas abiertas y cerradas', 'Líneas abiertas/cerradas', 9, 'avares://Quibee/Assets/Images/Star2.png', 85, 85, TRUE, NOW(), NOW()),
(1, 'La recta numérica', 'La recta numérica', 10, 'avares://Quibee/Assets/Images/LilacPlanet2.png', 90, 90, TRUE, NOW(), NOW()),
(1, 'Números del 10 al 50', 'Números 10 al 50', 11, 'avares://Quibee/Assets/Images/Meteor.png', 95, 95, TRUE, NOW(), NOW()),
(1, 'Números del 51 al 100', 'Números 51 al 100', 12, 'avares://Quibee/Assets/Images/Earth2.png', 85, 85, TRUE, NOW(), NOW()),
(1, 'Unidades y decenas', 'Unidades y decenas', 13, 'avares://Quibee/Assets/Images/Saturn2.png', 90, 90, TRUE, NOW(), NOW()),
(1, 'El número 100', 'El número 100', 14, 'avares://Quibee/Assets/Images/3.png', 95, 95, TRUE, NOW(), NOW()),
(1, 'La recta numérica hasta 100', 'Recta hasta 100', 15, 'avares://Quibee/Assets/Images/Robot.png', 85, 85, TRUE, NOW(), NOW()),
(1, 'Comparación de Números hasta el 100', 'Comparación hasta 100', 16, 'avares://Quibee/Assets/Images/SmallStar.png', 90, 90, TRUE, NOW(), NOW()),
(1, 'Comparación de longitudes', 'Comparación longitudes', 17, 'avares://Quibee/Assets/Images/Neptune.png', 85, 85, TRUE, NOW(), NOW()),
(1, 'Medimos usando nuestro cuerpo', 'Medimos con el cuerpo', 18, 'avares://Quibee/Assets/Images/LilacPlanet2.png', 90, 90, TRUE, NOW(), NOW()),
(1, 'Lección de repaso', 'Lección de repaso', 19, 'avares://Quibee/Assets/Images/Meteor.png', 100, 100, TRUE, NOW(), NOW());

SELECT order_index, topic_name
FROM TOPIC
WHERE id_level = 1
ORDER BY order_index;
