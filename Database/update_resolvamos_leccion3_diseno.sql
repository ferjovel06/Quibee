USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 3
  AND section_type = 'resolvamos';

DELETE FROM EXERCISE
WHERE id_lesson = 3
  AND section_type = 'resolvamos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    3,
    'heading',
    '{"text":"Resolvamos","level":1,"color":"#FFFFFF"}',
    1,
    'resolvamos',
    TRUE
),
(
    3,
    'heading',
    '{"text":"Problema 1:","level":3,"color":"#FFFFFF"}',
    2,
    'resolvamos',
    TRUE
),
(
    3,
    'text',
    '{"text":"En la cocina hay 5 galletas y 3 caramelos. ¿Hay más galletas o caramelos? Agrega los números y operador de comparación de manera correcta.","fontSize":16,"color":"#FFFFFF"}',
    3,
    'resolvamos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Ordena tu respuesta:","fontSize":16,"color":"#FFFFFF"}',
    4,
    'resolvamos',
    TRUE
),
(
    3,
    'heading',
    '{"text":"Problema 2:","level":3,"color":"#FFFFFF"}',
    7,
    'resolvamos',
    TRUE
),
(
    3,
    'text',
    '{"text":"En la tienda hay 6 helados y 7 cupcakes. ¿Hay más cupcakes o helados? Agrega los números y operador de comparación de manera correcta.","fontSize":16,"color":"#FFFFFF"}',
    8,
    'resolvamos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Ordena tu respuesta:","fontSize":16,"color":"#FFFFFF"}',
    9,
    'resolvamos',
    TRUE
);

INSERT INTO EXERCISE (
    id_lesson,
    section_type,
    instructions,
    exercise_type,
    question_text,
    config,
    solution,
    difficulty,
    points,
    order_index,
    is_active,
    created_at,
    updated_at
)
VALUES
(
    3,
    'resolvamos',
    'Ordena tu respuesta arrastrando fichas.',
    'matching',
    'Compara 5 galletas con 3 caramelos',
    '{"layout":"order_tokens","symbols":["3","5","<",">"],"rows":[{"imageUrl":"avares://Quibee/Assets/Images/Cookie.png","count":5,"imageWidth":32,"imageHeight":32,"rightImageUrl":"avares://Quibee/Assets/Images/Candy.png","rightCount":3,"rightImageWidth":34,"rightImageHeight":34,"correctSymbolAnswer":"5"},{"correctSymbolAnswer":">"},{"correctSymbolAnswer":"3"}]}',
    '{"answers":["5",">","3"]}',
    'easy',
    15,
    6,
    TRUE,
    NOW(),
    NULL
),
(
    3,
    'resolvamos',
    'Ordena tu respuesta arrastrando fichas.',
    'matching',
    'Compara 6 helados con 7 cupcakes',
    '{"layout":"order_tokens","symbols":["6","7","<",">"],"rows":[{"imageUrl":"avares://Quibee/Assets/Images/IceCream.png","count":6,"imageWidth":30,"imageHeight":40,"rightImageUrl":"avares://Quibee/Assets/Images/Cupcake.png","rightCount":7,"rightImageWidth":34,"rightImageHeight":34,"correctSymbolAnswer":"6"},{"correctSymbolAnswer":"<"},{"correctSymbolAnswer":"7"}]}',
    '{"answers":["6","<","7"]}',
    'easy',
    15,
    11,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 3 AND section_type = 'resolvamos'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text
FROM EXERCISE
WHERE id_lesson = 3 AND section_type = 'resolvamos'
ORDER BY order_index;