-- Script para actualizar los temas con posiciones, iconos y propiedades del mapa
-- Fecha: 2026-01-23

USE quibee_db;

-- ============================================
-- PRIMER GRADO - Temas con posiciones
-- ============================================

-- Tema 1: Conozcamos y escribamos los números del 1 al 10
UPDATE TOPIC SET 
    description = 'Tema 1: Conozcamos y escribamos\nlos números del 1 al 10',
    icon = 'avares://Quibee/Assets/Images/SmallStar.png',
    position_x = 740,
    position_y = 120,
    icon_width = 80,
    icon_height = 80,
    text_on_left = TRUE,
    text_on_right = FALSE,
    rotation_angle = 0
WHERE id_level = 1 AND order_index = 1;

-- Tema 2: Relacionemos números y objetos
UPDATE TOPIC SET 
    description = 'Tema 2: Relacionemos\nnúmeros y objetos',
    icon = 'avares://Quibee/Assets/Images/LilacPlanet2.png',
    position_x = 580,
    position_y = 220,
    icon_width = 100,
    icon_height = 100,
    text_on_left = TRUE,
    text_on_right = FALSE,
    rotation_angle = 0
WHERE id_level = 1 AND order_index = 2;

-- Tema 3: Números cardinales del 1° al 10°
UPDATE TOPIC SET 
    topic_name = 'Números cardinales del 1° al 10°',
    description = 'Tema 3: Números cardinales del\n1° al 10°',
    icon = 'avares://Quibee/Assets/Images/Meteor.png',
    position_x = 550,
    position_y = 400,
    icon_width = 90,
    icon_height = 90,
    text_on_left = FALSE,
    text_on_right = TRUE,
    rotation_angle = 0
WHERE id_level = 1 AND order_index = 3;

-- Tema 4: Suma y resta de números
UPDATE TOPIC SET 
    topic_name = 'Suma y resta de números',
    description = 'Tema 4: Suma y resta de\nnúmeros',
    icon = 'avares://Quibee/Assets/Images/Earth2.png',
    position_x = 270,
    position_y = 330,
    icon_width = 100,
    icon_height = 100,
    text_on_left = TRUE,
    text_on_right = FALSE,
    rotation_angle = 0
WHERE id_level = 1 AND order_index = 4;

-- Tema 5: Unidades y decenas
UPDATE TOPIC SET 
    topic_name = 'Unidades y decenas',
    description = 'Tema 5: Unidades y\ndecenas',
    icon = 'avares://Quibee/Assets/Images/Saturn2.png',
    position_x = 280,
    position_y = 520,
    icon_width = 100,
    icon_height = 100,
    text_on_left = FALSE,
    text_on_right = TRUE,
    rotation_angle = 0
WHERE id_level = 1 AND order_index = 5;

-- ============================================
-- SEGUNDO GRADO - Temas con posiciones
-- ============================================

-- Tema 1: Sumas y restas
UPDATE TOPIC SET 
    topic_name = 'Sumas y restas',
    description = 'Tema 1: Sumas y restas',
    icon = 'avares://Quibee/Assets/Images/Calculator.png',
    position_x = 780,
    position_y = 120,
    icon_width = 80,
    icon_height = 80,
    text_on_left = TRUE,
    text_on_right = FALSE,
    rotation_angle = 0
WHERE id_level = 2 AND order_index = 1;

-- Tema 2: Pictogramas
UPDATE TOPIC SET 
    topic_name = 'Pictogramas',
    description = 'Tema 2: Pictogramas',
    icon = 'avares://Quibee/Assets/Images/WhiteRocket.png',
    position_x = 600,
    position_y = 235,
    icon_width = 100,
    icon_height = 100,
    text_on_left = TRUE,
    text_on_right = FALSE,
    rotation_angle = 0
WHERE id_level = 2 AND order_index = 2;

-- Tema 3: Suma horizontal y vertical
UPDATE TOPIC SET 
    topic_name = 'Suma horizontal y vertical',
    description = 'Tema 3: Suma horizontal y vertical',
    icon = 'avares://Quibee/Assets/Images/SkyBlueAlien.png',
    position_x = 550,
    position_y = 400,
    icon_width = 100,
    icon_height = 100,
    text_on_left = FALSE,
    text_on_right = TRUE,
    rotation_angle = 0
WHERE id_level = 2 AND order_index = 3;

-- Tema 4: Multiplicación
UPDATE TOPIC SET 
    topic_name = 'Multiplicación',
    description = 'Tema 4: Multiplicación',
    icon = 'avares://Quibee/Assets/Images/Star2.png',
    position_x = 285,
    position_y = 330,
    icon_width = 90,
    icon_height = 90,
    text_on_left = TRUE,
    text_on_right = FALSE,
    rotation_angle = 0
WHERE id_level = 2 AND order_index = 4;

-- Tema 5: Longitud
UPDATE TOPIC SET 
    topic_name = 'Longitud',
    description = 'Tema 5: Longitud',
    icon = 'avares://Quibee/Assets/Images/Neptune.png',
    position_x = 280,
    position_y = 520,
    icon_width = 70,
    icon_height = 70,
    text_on_left = FALSE,
    text_on_right = TRUE,
    rotation_angle = 0
WHERE id_level = 2 AND order_index = 5;

-- ============================================
-- TERCER GRADO - Temas con posiciones
-- ============================================

-- Tema 1: Cuerpos geométricos
UPDATE TOPIC SET 
    topic_name = 'Cuerpos geométricos',
    description = 'Tema 1: Cuerpos geométricos',
    icon = 'avares://Quibee/Assets/Images/LilacPlanet.png',
    position_x = 780,
    position_y = 120,
    icon_width = 80,
    icon_height = 80,
    text_on_left = TRUE,
    text_on_right = FALSE,
    rotation_angle = 0
WHERE id_level = 3 AND order_index = 1;

-- Tema 2: Fracciones
UPDATE TOPIC SET 
    topic_name = 'Fracciones',
    description = 'Tema 2: Fracciones',
    icon = 'avares://Quibee/Assets/Images/Star2.png',
    position_x = 640,
    position_y = 220,
    icon_width = 80,
    icon_height = 80,
    text_on_left = TRUE,
    text_on_right = FALSE,
    rotation_angle = 0
WHERE id_level = 3 AND order_index = 2;

-- Tema 3: Números decimales
UPDATE TOPIC SET 
    topic_name = 'Números decimales',
    description = 'Tema 3: Números decimales',
    icon = 'avares://Quibee/Assets/Images/SquirrelRocket.png',
    position_x = 540,
    position_y = 400,
    icon_width = 100,
    icon_height = 100,
    text_on_left = FALSE,
    text_on_right = TRUE,
    rotation_angle = 0
WHERE id_level = 3 AND order_index = 3;

-- Tema 4: Operaciones combinadas
UPDATE TOPIC SET 
    topic_name = 'Operaciones combinadas',
    description = 'Tema 4: Operaciones combinadas',
    icon = 'avares://Quibee/Assets/Images/AlienFullBody.png',
    position_x = 210,
    position_y = 340,
    icon_width = 100,
    icon_height = 100,
    text_on_left = TRUE,
    text_on_right = FALSE,
    rotation_angle = 0
WHERE id_level = 3 AND order_index = 4;

-- Tema 5: Medidas de capacidad
UPDATE TOPIC SET 
    topic_name = 'Medidas de capacidad',
    description = 'Tema 5: Medidas de capacidad',
    icon = 'avares://Quibee/Assets/Images/GreenPlanet.png',
    position_x = 280,
    position_y = 520,
    icon_width = 70,
    icon_height = 70,
    text_on_left = FALSE,
    text_on_right = TRUE,
    rotation_angle = 0
WHERE id_level = 3 AND order_index = 5;

-- Verificar los cambios
SELECT 
    l.level_name,
    t.order_index as 'Tema',
    t.topic_name as 'Nombre',
    SUBSTRING(t.icon, 36) as 'Icono',
    t.position_x as 'X',
    t.position_y as 'Y',
    CONCAT(t.icon_width, 'x', t.icon_height) as 'Tamaño',
    t.text_on_left as 'Izq',
    t.text_on_right as 'Der'
FROM TOPIC t
JOIN LEVEL l ON t.id_level = l.id_level
ORDER BY l.level_number, t.order_index;
