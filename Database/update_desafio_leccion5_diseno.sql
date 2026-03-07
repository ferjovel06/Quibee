USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 5
  AND section_type = 'desafio';

DELETE FROM EXERCISE
WHERE id_lesson = 5
  AND section_type = 'desafio';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    5,
    'heading',
    '{"text":"Desafío","level":1,"color":"#FFFFFF"}',
    1,
    'desafio',
    TRUE
),
(
    5,
    'text',
    '{"text":"Selecciona la respuesta que corresponde a la operación de cada ejercicio.","fontSize":16,"color":"#FFFFFF"}',
    2,
    'desafio',
    TRUE
)
;

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
    5,
    'desafio',
    'Selecciona una de las 4 opciones.',
    'matching',
    'Problema 1: Imagina que estás organizando una fiesta de cumpleaños. Al inicio, tienes 7 globos y tus amigos traen 8 globos más para decorar. ¿Cuántos globos tienes en total para la fiesta?',
    '{"layout":"order_tokens","showOrderPrompt":false,"selectOnly":true,"symbols":["7+8=9","7+8=15","7+8=13","7+8=18"],"rows":[{"correctSymbolAnswer":"7+8=15"}]}',
    '{"answers":["7+8=15"]}',
    'easy',
    10,
    4,
    TRUE,
    NOW(),
    NULL
),
(
    5,
    'desafio',
    'Selecciona una de las 4 opciones.',
    'matching',
    'Problema 2: En la cocina hay 9 naranjas y traes 4 más del mercado. ¿Cuántas naranjas tienes en total?',
    '{"layout":"order_tokens","showOrderPrompt":false,"selectOnly":true,"symbols":["9+4=17","9+4=8","9+4=13","9+4=12"],"rows":[{"correctSymbolAnswer":"9+4=13"}]}',
    '{"answers":["9+4=13"]}',
    'easy',
    10,
    8,
    TRUE,
    NOW(),
    NULL
),
(
    5,
    'desafio',
    'Selecciona una de las 4 opciones.',
    'matching',
    'Problema 3: Tienes 2 peluches y recibes 5 más como regalo. ¿Cuántos peluches tienes ahora?',
    '{"layout":"order_tokens","showOrderPrompt":false,"selectOnly":true,"symbols":["2+5=7","2+5=11","2+5=9","2+5=4"],"rows":[{"correctSymbolAnswer":"2+5=7"}]}',
    '{"answers":["2+5=7"]}',
    'easy',
    10,
    12,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 5 AND section_type = 'desafio'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text
FROM EXERCISE
WHERE id_lesson = 5 AND section_type = 'desafio'
ORDER BY order_index;
