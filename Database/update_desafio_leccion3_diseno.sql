USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 3
  AND section_type = 'desafio';

DELETE FROM EXERCISE
WHERE id_lesson = 3
  AND section_type = 'desafio';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    3,
    'heading',
    '{"text":"Desafío","level":1,"color":"#FFFFFF"}',
    1,
    'desafio',
    TRUE
),
(
    3,
    'heading',
    '{"text":"Comparando puestos de frutas","level":3,"color":"#FFFFFF"}',
    2,
    'desafio',
    TRUE
),
(
    3,
    'text',
    '{"text":"Imagina que estás en una feria de frutas. Observa cuántas frutas hay en cada puesto y responde.","fontSize":16,"color":"#FFFFFF"}',
    3,
    'desafio',
    TRUE
),
(
    3,
    'text',
    '{"text":"Puesto de frutas","fontSize":16,"color":"#FFFFFF"}',
    4,
    'desafio',
    TRUE
),
(
    3,
    'visual_example',
    '{"layout":"columns_board","spacing":24,"centerAlign":true,"objects":[{"emoji":"🍌","count":6,"fontSize":34,"symbol":"Puesto A","width":140},{"emoji":"🍎","count":10,"fontSize":34,"symbol":"Puesto B","width":190},{"emoji":"🍊","count":6,"fontSize":34,"symbol":"Puesto C","width":140}]}',
    5,
    'desafio',
    TRUE
),
(
    3,
    'text',
    '{"text":"Responde:","fontSize":16,"color":"#FFFFFF"}',
    8,
    'desafio',
    TRUE
),
(
    3,
    'text',
    '{"text":"a) ¿Cuál es el puesto con más frutas?","fontSize":16,"color":"#FFFFFF"}',
    9,
    'desafio',
    TRUE
),
(
    3,
    'text',
    '{"text":"b) Arrastra los puestos con la misma cantidad de frutas.","fontSize":16,"color":"#FFFFFF"}',
    11,
    'desafio',
    TRUE
),
(
    3,
    'text',
    '{"text":"c) Agrega el operador de comparación en cada cuadro.","fontSize":16,"color":"#FFFFFF"}',
    13,
    'desafio',
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
    'desafio',
    'Arrastra la opción correcta.',
    'matching',
    '¿Cuál puesto tiene más frutas?',
    '{"layout":"order_tokens","symbols":["A","B","C"],"rows":[{"correctSymbolAnswer":"B"}]}',
    '{"answers":["B"]}',
    'easy',
    10,
    10,
    TRUE,
    NOW(),
    NULL
),
(
    3,
    'desafio',
    'Arrastra los dos puestos que tienen la misma cantidad.',
    'matching',
    'Selecciona los puestos con la misma cantidad',
    '{"layout":"order_tokens","allowAnyOrder":true,"symbols":["A","B","C"],"rows":[{"correctSymbolAnswer":"A"},{"correctSymbolAnswer":"C"}]}',
    '{"answers":["A","C"]}',
    'easy',
    15,
    12,
    TRUE,
    NOW(),
    NULL
),
(
    3,
    'desafio',
    'Operadores de comparación:',
    'matching',
    'Agrega el operador de comparación en cada cuadro.',
    '{"layout":"operator_compare_compact","symbols":["=",">","<"],"rows":[{"emoji":"🍎","count":1,"rightEmoji":"🍌","rightCount":1,"correctSymbolAnswer":">"},{"emoji":"🍌","count":1,"rightEmoji":"🍊","rightCount":1,"correctSymbolAnswer":"="},{"emoji":"🍊","count":1,"rightEmoji":"🍎","rightCount":1,"correctSymbolAnswer":"<"}]}',
    '{"answers":[">","=","<"]}',
    'medium',
    20,
    14,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 3 AND section_type = 'desafio'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text, JSON_EXTRACT(config,'$.layout') AS layout_mode
FROM EXERCISE
WHERE id_lesson = 3 AND section_type = 'desafio'
ORDER BY order_index;
