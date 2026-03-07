USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 4
  AND section_type = 'desafio';

DELETE FROM EXERCISE
WHERE id_lesson = 4
  AND section_type = 'desafio';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    4,
    'heading',
    '{"text":"Desafío","level":1,"color":"#FFFFFF"}',
    1,
    'desafio',
    TRUE
),
(
    4,
    'text',
    '{"text":"Arrastra el número que falta para formar la cantidad dada.","fontSize":16,"color":"#FFFFFF"}',
    2,
    'desafio',
    TRUE
),
(
    4,
    'text',
    '{"text":"Completa cada número arrastrando el dígito faltante.","fontSize":15,"color":"#FFFFFF"}',
    3,
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
    4,
    'desafio',
    'Arrastra cada dígito faltante a su casilla.',
    'matching',
    'Completa: Once, Trece, Dieciocho, Veinte, Doce, Diecisiete',
    '{"layout":"order_tokens","symbols":["8","1","1","2","7","2"],"rows":[{"promptText":"Once","fixedSymbol":"1","fixedSymbolPosition":"left","correctSymbolAnswer":"1"},{"promptText":"Trece","fixedSymbol":"3","fixedSymbolPosition":"right","correctSymbolAnswer":"1"},{"promptText":"Dieciocho","fixedSymbol":"1","fixedSymbolPosition":"left","correctSymbolAnswer":"8"},{"promptText":"Veinte","fixedSymbol":"0","fixedSymbolPosition":"right","correctSymbolAnswer":"2"},{"promptText":"Doce","fixedSymbol":"1","fixedSymbolPosition":"left","correctSymbolAnswer":"2"},{"promptText":"Diecisiete","fixedSymbol":"1","fixedSymbolPosition":"left","correctSymbolAnswer":"7"}]}',
    '{"answers":["1","1","8","2","2","7"]}',
    'medium',
    25,
    4,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 4 AND section_type = 'desafio'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text, JSON_EXTRACT(config,'$.layout') AS layout_mode
FROM EXERCISE
WHERE id_lesson = 4 AND section_type = 'desafio'
ORDER BY order_index;
