-- Script para sincronizar datos de TOPIC con el fallback del código
-- Este script actualiza los iconos, posiciones y tamaños para que coincidan exactamente
-- con los datos hardcodeados en LessonsMapViewModel.cs

USE quibee_db;

-- ============================================
-- GRADO 1 (PRIMER GRADO)
-- ============================================

-- Tema 1: Conozcamos y escribamos los números del 1 al 10
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.icon = 'avares://Quibee/Assets/Images/SmallStar.png',
    t.position_x = 740,
    t.position_y = 120,
    t.icon_width = 80,
    t.icon_height = 80,
    t.text_on_left = 1,
    t.text_on_right = 0
WHERE l.level_number = 1 AND t.order_index = 1;

-- Tema 2: Relacionemos números y objetos
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.icon = 'avares://Quibee/Assets/Images/LilacPlanet2.png',
    t.position_x = 580,
    t.position_y = 220,
    t.icon_width = 100,
    t.icon_height = 100,
    t.text_on_left = 1,
    t.text_on_right = 0
WHERE l.level_number = 1 AND t.order_index = 2;

-- Tema 3: Números cardinales del 1° al 10°
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Números cardinales del 1° al 10°',
    t.description = 'Tema 3: Números cardinales del\n1° al 10°',
    t.icon = 'avares://Quibee/Assets/Images/Meteor.png',
    t.position_x = 550,
    t.position_y = 400,
    t.icon_width = 90,
    t.icon_height = 90,
    t.text_on_left = 0,
    t.text_on_right = 1
WHERE l.level_number = 1 AND t.order_index = 3;

-- Tema 4: Suma y resta de números
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Suma y resta de números',
    t.description = 'Tema 4: Suma y resta de\nnúmeros',
    t.icon = 'avares://Quibee/Assets/Images/Earth2.png',
    t.position_x = 270,
    t.position_y = 330,
    t.icon_width = 100,
    t.icon_height = 100,
    t.text_on_left = 1,
    t.text_on_right = 0
WHERE l.level_number = 1 AND t.order_index = 4;

-- Tema 5: Unidades y decenas
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Unidades y decenas',
    t.description = 'Tema 5: Unidades y\ndecenas',
    t.icon = 'avares://Quibee/Assets/Images/Saturn2.png',
    t.position_x = 280,
    t.position_y = 520,
    t.icon_width = 100,
    t.icon_height = 100,
    t.text_on_left = 0,
    t.text_on_right = 1
WHERE l.level_number = 1 AND t.order_index = 5;

-- ============================================
-- GRADO 2 (SEGUNDO GRADO)
-- ============================================

-- Tema 1: Sumas y restas
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Sumas y restas',
    t.description = 'Tema 1: Sumas y restas',
    t.icon = 'avares://Quibee/Assets/Images/Calculator.png',
    t.position_x = 780,
    t.position_y = 120,
    t.icon_width = 80,
    t.icon_height = 80,
    t.text_on_left = 1,
    t.text_on_right = 0
WHERE l.level_number = 2 AND t.order_index = 1;

-- Tema 2: Pictogramas
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Pictogramas',
    t.description = 'Tema 2: Pictogramas',
    t.icon = 'avares://Quibee/Assets/Images/WhiteRocket.png',
    t.position_x = 600,
    t.position_y = 235,
    t.icon_width = 100,
    t.icon_height = 100,
    t.text_on_left = 1,
    t.text_on_right = 0
WHERE l.level_number = 2 AND t.order_index = 2;

-- Tema 3: Suma horizontal y vertical
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Suma horizontal y vertical',
    t.description = 'Tema 3: Suma horizontal y vertical',
    t.icon = 'avares://Quibee/Assets/Images/SkyBlueAlien.png',
    t.position_x = 550,
    t.position_y = 400,
    t.icon_width = 100,
    t.icon_height = 100,
    t.text_on_left = 0,
    t.text_on_right = 1
WHERE l.level_number = 2 AND t.order_index = 3;

-- Tema 4: Multiplicación
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Multiplicación',
    t.description = 'Tema 4: Multiplicación',
    t.icon = 'avares://Quibee/Assets/Images/Star2.png',
    t.position_x = 285,
    t.position_y = 330,
    t.icon_width = 90,
    t.icon_height = 90,
    t.text_on_left = 1,
    t.text_on_right = 0
WHERE l.level_number = 2 AND t.order_index = 4;

-- Tema 5: Longitud
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Longitud',
    t.description = 'Tema 5: Longitud',
    t.icon = 'avares://Quibee/Assets/Images/Neptune.png',
    t.position_x = 280,
    t.position_y = 520,
    t.icon_width = 70,
    t.icon_height = 70,
    t.text_on_left = 0,
    t.text_on_right = 1
WHERE l.level_number = 2 AND t.order_index = 5;

-- ============================================
-- GRADO 3 (TERCER GRADO)
-- ============================================

-- Tema 1: Cuerpos geométricos
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Cuerpos geométricos',
    t.description = 'Tema 1: Cuerpos geométricos',
    t.icon = 'avares://Quibee/Assets/Images/LilacPlanet.png',
    t.position_x = 780,
    t.position_y = 120,
    t.icon_width = 80,
    t.icon_height = 80,
    t.text_on_left = 1,
    t.text_on_right = 0
WHERE l.level_number = 3 AND t.order_index = 1;

-- Tema 2: Fracciones
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Fracciones',
    t.description = 'Tema 2: Fracciones',
    t.icon = 'avares://Quibee/Assets/Images/Star2.png',
    t.position_x = 640,
    t.position_y = 220,
    t.icon_width = 80,
    t.icon_height = 80,
    t.text_on_left = 1,
    t.text_on_right = 0
WHERE l.level_number = 3 AND t.order_index = 2;

-- Tema 3: Números decimales
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Números decimales',
    t.description = 'Tema 3: Números decimales',
    t.icon = 'avares://Quibee/Assets/Images/SquirrelRocket.png',
    t.position_x = 540,
    t.position_y = 400,
    t.icon_width = 100,
    t.icon_height = 100,
    t.text_on_left = 0,
    t.text_on_right = 1
WHERE l.level_number = 3 AND t.order_index = 3;

-- Tema 4: Operaciones combinadas
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Operaciones combinadas',
    t.description = 'Tema 4: Operaciones combinadas',
    t.icon = 'avares://Quibee/Assets/Images/AlienFullBody.png',
    t.position_x = 210,
    t.position_y = 340,
    t.icon_width = 100,
    t.icon_height = 100,
    t.text_on_left = 1,
    t.text_on_right = 0
WHERE l.level_number = 3 AND t.order_index = 4;

-- Tema 5: Medidas de capacidad
UPDATE TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
SET 
    t.topic_name = 'Medidas de capacidad',
    t.description = 'Tema 5: Medidas de capacidad',
    t.icon = 'avares://Quibee/Assets/Images/GreenPlanet.png',
    t.position_x = 280,
    t.position_y = 520,
    t.icon_width = 70,
    t.icon_height = 70,
    t.text_on_left = 0,
    t.text_on_right = 1
WHERE l.level_number = 3 AND t.order_index = 5;

-- Verificar los cambios
SELECT 
    l.level_number AS 'Grado',
    t.order_index AS '#',
    t.topic_name AS 'Tema',
    RIGHT(t.icon, 25) AS 'Icono',
    t.position_x AS 'X',
    t.position_y AS 'Y',
    CONCAT(t.icon_width, 'x', t.icon_height) AS 'Tamaño'
FROM TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
ORDER BY l.level_number, t.order_index;
