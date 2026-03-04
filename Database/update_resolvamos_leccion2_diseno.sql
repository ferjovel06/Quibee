USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 2
  AND section_type = 'resolvamos';

DELETE FROM EXERCISE
WHERE id_lesson = 2
  AND section_type = 'resolvamos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    2,
    'heading',
    '{"text":"Resolvamos","level":1,"color":"#FFFFFF"}',
    1,
    'resolvamos',
    TRUE
),
(
    2,
    'heading',
    '{"text":"Ejercicio 1","level":3,"color":"#FFFFFF"}',
    2,
    'resolvamos',
    TRUE
),
(
    2,
    'text',
    '{"text":"En el jardín hay 5 mariposas y 3 abejas. Arrastra el número que corresponde a cada cantidad.","fontSize":16,"color":"#FFFFFF"}',
    3,
    'resolvamos',
    TRUE
),
(
    2,
    'heading',
    '{"text":"Ejercicio 2","level":3,"color":"#FFFFFF"}',
    5,
    'resolvamos',
    TRUE
),
(
    2,
    'text',
    '{"text":"Tienes 4 lápices en tu estuche y encuentras 2 más en tu mochila. Arrastra el número que corresponde a cada cantidad.","fontSize":16,"color":"#FFFFFF"}',
    6,
    'resolvamos',
    TRUE
),
(
    2,
    'heading',
    '{"text":"Ejercicio 3","level":3,"color":"#FFFFFF"}',
    8,
    'resolvamos',
    TRUE
),
(
    2,
    'text',
    '{"text":"En un estanque hay 6 patos nadando y 2 patos en la orilla. Arrastra el número que corresponde a cada cantidad.","fontSize":16,"color":"#FFFFFF"}',
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
    2,
    'resolvamos',
    'Relaciona cada grupo con su número.',
    'matching',
    'Mariposas y abejas',
    '{"layout":"paired_counts","numberImagePattern":"avares://Quibee/Assets/Images/{0}.png","numberImageWidth":44,"numberImageHeight":64,"rows":[{"imageUrl":"avares://Quibee/Assets/Images/Butterfly.png","count":5,"correctAnswer":5,"imageWidth":44,"imageHeight":44},{"imageUrl":"avares://Quibee/Assets/Images/Bee.png","count":3,"correctAnswer":3,"imageWidth":44,"imageHeight":44}]}',
    '{"answers":[5,3]}',
    'easy',
    10,
    4,
    TRUE,
    NOW(),
    NULL
),
(
    2,
    'resolvamos',
    'Relaciona cada grupo con su número.',
    'matching',
    'Lápices',
    '{"layout":"paired_counts","numberImagePattern":"avares://Quibee/Assets/Images/{0}.png","numberImageWidth":44,"numberImageHeight":64,"rows":[{"imageUrl":"avares://Quibee/Assets/Images/Pencil.png","count":4,"correctAnswer":4,"imageWidth":44,"imageHeight":44},{"imageUrl":"avares://Quibee/Assets/Images/Pencil.png","count":2,"correctAnswer":2,"imageWidth":44,"imageHeight":44}]}',
    '{"answers":[4,2]}',
    'easy',
    10,
    7,
    TRUE,
    NOW(),
    NULL
),
(
    2,
    'resolvamos',
    'Relaciona cada grupo con su número.',
    'matching',
    'Patos',
    '{"layout":"paired_counts","numberImagePattern":"avares://Quibee/Assets/Images/{0}.png","numberImageWidth":44,"numberImageHeight":64,"rows":[{"imageUrl":"avares://Quibee/Assets/Images/Duck.png","count":6,"correctAnswer":6,"imageWidth":44,"imageHeight":44},{"imageUrl":"avares://Quibee/Assets/Images/Duck.png","count":2,"correctAnswer":2,"imageWidth":44,"imageHeight":44}]}',
    '{"answers":[6,2]}',
    'easy',
    10,
    10,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 2 AND section_type = 'resolvamos'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text, JSON_EXTRACT(config,'$.layout') AS layout_mode
FROM EXERCISE
WHERE id_lesson = 2 AND section_type = 'resolvamos'
ORDER BY order_index;
