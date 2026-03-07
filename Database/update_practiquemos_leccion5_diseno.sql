USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 5
  AND section_type = 'practiquemos';

DELETE FROM EXERCISE
WHERE id_lesson = 5
  AND section_type = 'practiquemos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    5,
    'heading',
    '{"text":"Practiquemos","level":1,"color":"#FFFFFF"}',
    1,
    'practiquemos',
    TRUE
),
(
    5,
    'heading',
    '{"text":"Ejercicio 1","level":3,"color":"#FFFFFF"}',
    2,
    'practiquemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Si compras 5 naranjas y te regalan 4 más, ¿cuántas naranjas tienes en total?","fontSize":16,"color":"#FFFFFF"}',
    3,
    'practiquemos',
    TRUE
),
(
    5,
    'heading',
    '{"text":"Ejercicio 2","level":3,"color":"#FFFFFF"}',
    6,
    'practiquemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Hay 3 ranas dentro de un estanque y 2 afuera, ¿cuántas ranas puedes ver?","fontSize":16,"color":"#FFFFFF"}',
    7,
    'practiquemos',
    TRUE
),
(
    5,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Pond.png","width":360,"height":180}]}',
    8,
    'practiquemos',
    TRUE
),
(
    5,
    'heading',
    '{"text":"Ejercicio 3","level":3,"color":"#FFFFFF"}',
    10,
    'practiquemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Si sumas 3 + 5, ¿cuál es el resultado?","fontSize":16,"color":"#FFFFFF"}',
    11,
    'practiquemos',
    TRUE
),
(
    5,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Road.png","width":420,"height":130}]}',
    12,
    'practiquemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Selecciona la respuesta correcta.","fontSize":16,"color":"#FFFFFF"}',
    13,
    'practiquemos',
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
    5,
    'practiquemos',
    'Cuenta y escribe la cantidad total de naranjas.',
    'visual_count',
    'Naranjas: 5 + 4',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/Orange.png","count":5,"width":28,"height":28},{"symbol":"+","fontSize":52,"color":"#55C4E8"},{"imageUrl":"avares://Quibee/Assets/Images/4.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/Orange.png","count":4,"width":28,"height":28},{"symbol":"=","fontSize":52,"color":"#9B86D1"},{"answerBox":true,"width":98,"height":74}],"layout":"horizontal","spacing":10,"centerAlign":true,"correctAnswer":9}',
    '{"answer":9}',
    'easy',
    10,
    4,
    TRUE,
    NOW(),
    NULL
),
(
    5,
    'practiquemos',
    'Cuenta y escribe la cantidad total de ranas.',
    'visual_count',
    'Ranas: 3 + 2',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Frog.png","count":3,"width":34,"height":34},{"symbol":"+","fontSize":52,"color":"#55C4E8"},{"imageUrl":"avares://Quibee/Assets/Images/Frog.png","count":2,"width":34,"height":34},{"symbol":"=","fontSize":52,"color":"#9B86D1"},{"answerBox":true,"width":98,"height":74}],"layout":"horizontal","spacing":12,"centerAlign":true,"correctAnswer":5}',
    '{"answer":5}',
    'easy',
    10,
    9,
    TRUE,
    NOW(),
    NULL
),
(
    5,
    'practiquemos',
    'Arrastra la respuesta correcta al recuadro.',
    'matching',
    'Suma: 3 + 5',
    '{"layout":"order_tokens","showOrderPrompt":false,"symbols":["7","5","8","10"],"rows":[{"correctSymbolAnswer":"8"}]}',
    '{"answers":["8"]}',
    'easy',
    10,
    14,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 5 AND section_type = 'practiquemos'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text
FROM EXERCISE
WHERE id_lesson = 5 AND section_type = 'practiquemos'
ORDER BY order_index;
