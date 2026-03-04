USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 3
  AND section_type = 'analicemos';

DELETE FROM EXERCISE
WHERE id_lesson = 3
  AND section_type = 'analicemos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    3,
    'heading',
    '{"text":"Analicemos","level":1,"color":"#FFFFFF"}',
    1,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Para empezar, es importante conocer cómo se representan los operadores de comparación. Con estos podremos indicar si un grupo de objetos o cosas es mayor o menor a otro. Veamos cuáles son.","fontSize":16,"color":"#FFFFFF"}',
    2,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Ejemplo:","fontSize":20,"color":"#FFFFFF"}',
    3,
    'analicemos',
    TRUE
),
(
    3,
    'visual_example',
    '{"objects":[{"symbol":">","fontSize":96,"color":"#22B7E9"},{"symbol":"Se lee: Mayor que","fontSize":24,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":56,"height":78},{"symbol":">","fontSize":64,"color":"#22B7E9"},{"imageUrl":"avares://Quibee/Assets/Images/3.png","width":56,"height":78},{"symbol":"→","fontSize":52,"color":"#D05A8A"},{"symbol":"Se lee: 5 es mayor que 3.","fontSize":24,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    4,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Ejemplo:","fontSize":20,"color":"#FFFFFF"}',
    5,
    'analicemos',
    TRUE
),
(
    3,
    'visual_example',
    '{"objects":[{"symbol":"<","fontSize":96,"color":"#F29A2E"},{"symbol":"Se lee: Menor que","fontSize":24,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/3.png","width":56,"height":78},{"symbol":"<","fontSize":64,"color":"#F29A2E"},{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":56,"height":78},{"symbol":"→","fontSize":52,"color":"#D05A8A"},{"symbol":"Se lee: 3 es menor que 5.","fontSize":24,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    6,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Ejemplo:","fontSize":20,"color":"#FFFFFF"}',
    7,
    'analicemos',
    TRUE
),
(
    3,
    'visual_example',
    '{"objects":[{"symbol":"=","fontSize":96,"color":"#B5473B"},{"symbol":"Se lee: Igual a","fontSize":24,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/2.png","width":56,"height":78},{"symbol":"=","fontSize":64,"color":"#B5473B"},{"imageUrl":"avares://Quibee/Assets/Images/2.png","width":56,"height":78},{"symbol":"→","fontSize":52,"color":"#D05A8A"},{"symbol":"Se lee: 2 es igual a 2.","fontSize":24,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    8,
    'analicemos',
    TRUE
),
(
    3,
    'heading',
    '{"text":"Aplicaciones en la vida diaria","level":3,"color":"#FFFFFF"}',
    9,
    'analicemos',
    TRUE
),
(
    3,
    'heading',
    '{"text":"Ejemplo 1: Comparando Frutas","level":3,"color":"#FFFFFF"}',
    10,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Imagina que tienes una canasta con 5 manzanas y otra con 3. Queremos saber cuál tiene más frutas.","fontSize":16,"color":"#FFFFFF"}',
    11,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Cesta A: 5 manzanas. Cesta B: 3 manzanas.","fontSize":16,"color":"#FFFFFF"}',
    12,
    'analicemos',
    TRUE
),
(
    3,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":5,"width":44,"height":44},{"symbol":">","fontSize":72,"color":"#22B7E9"},{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":3,"width":44,"height":44}],"layout":"horizontal","spacing":18,"centerAlign":true}',
    13,
    'analicemos',
    TRUE
),
(
    3,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":60,"height":84},{"symbol":">","fontSize":68,"color":"#22B7E9"},{"imageUrl":"avares://Quibee/Assets/Images/3.png","width":60,"height":84}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    14,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Al comparar, vemos que la Cesta A tiene más manzanas que la Cesta B.","fontSize":16,"color":"#FFFFFF"}',
    15,
    'analicemos',
    TRUE
),
(
    3,
    'heading',
    '{"text":"Ejemplo 2: Comparando Juguetes","level":3,"color":"#FFFFFF"}',
    16,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Tienes 7 balones de fútbol y tu amigo tiene 4 balones de basket. Vamos a comparar quién tiene más.","fontSize":16,"color":"#FFFFFF"}',
    17,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Tus balones: 7 de fútbol. Balones de tu amigo: 4 de basket.","fontSize":16,"color":"#FFFFFF"}',
    18,
    'analicemos',
    TRUE
),
(
    3,
    'visual_example',
    '{"objects":[{"emoji":"🏀","count":4,"fontSize":40},{"symbol":"<","fontSize":72,"color":"#F29A2E"},{"imageUrl":"avares://Quibee/Assets/Images/Ball.png","count":7,"width":42,"height":42}],"layout":"horizontal","spacing":18,"centerAlign":true}',
    19,
    'analicemos',
    TRUE
),
(
    3,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Ball.png","count":7,"width":42,"height":42},{"symbol":">","fontSize":72,"color":"#22B7E9"},{"emoji":"🏀","count":4,"fontSize":40}],"layout":"horizontal","spacing":18,"centerAlign":true}',
    20,
    'analicemos',
    TRUE
),
(
    3,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/7.png","width":60,"height":84},{"symbol":">","fontSize":68,"color":"#22B7E9"},{"imageUrl":"avares://Quibee/Assets/Images/4.png","width":60,"height":84}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    21,
    'analicemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Al comparar, vemos que tú tienes más balones que tu amigo.","fontSize":16,"color":"#FFFFFF"}',
    22,
    'analicemos',
    TRUE
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 3 AND section_type = 'analicemos'
ORDER BY order_index;
