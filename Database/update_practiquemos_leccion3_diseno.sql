USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 3
  AND section_type = 'practiquemos';

DELETE FROM EXERCISE
WHERE id_lesson = 3
  AND section_type = 'practiquemos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    3,
    'heading',
    '{"text":"Practiquemos","level":1,"color":"#FFFFFF"}',
    1,
    'practiquemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Ahora es tu turno de practicar con la ayuda de tu docente. Intenta resolver los siguientes ejercicios:","fontSize":16,"color":"#FFFFFF"}',
    2,
    'practiquemos',
    TRUE
),
(
    3,
    'heading',
    '{"text":"Ejercicio 1:","level":3,"color":"#FFFFFF"}',
    3,
    'practiquemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"En una caja hay 6 lápices y en otra hay 9. ¿Cuál caja tiene más lápices? Coloca el operador de comparación correspondiente.","fontSize":16,"color":"#FFFFFF"}',
    4,
    'practiquemos',
    TRUE
),
(
    3,
    'heading',
    '{"text":"Ejercicio 2:","level":3,"color":"#FFFFFF"}',
    6,
    'practiquemos',
    TRUE
),
(
    3,
    'text',
    '{"text":"Tienes 4 coches de juguete y tu hermano tiene 5. ¿Quién tiene más coches?","fontSize":16,"color":"#FFFFFF"}',
    7,
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
    3,
    'practiquemos',
    'Arrastra el operador correcto sobre la línea.',
    'matching',
    '6 lápices comparado con 9 lápices',
    '{"instruction":"Arrastra el operador correcto sobre la línea.","layout":"operator_compare","symbols":["<",">"],"rows":[{"imageUrl":"avares://Quibee/Assets/Images/Pencil.png","count":6,"imageWidth":34,"imageHeight":34,"rightImageUrl":"avares://Quibee/Assets/Images/Pencil.png","rightCount":9,"rightImageWidth":34,"rightImageHeight":34,"correctSymbolAnswer":"<"}]}',
    '{"answer":"<"}',
    'easy',
    15,
    5,
    TRUE,
    NOW(),
    NULL
),
(
    3,
    'practiquemos',
    'Arrastra el operador correcto sobre la línea.',
    'matching',
    '4 coches comparado con 5 coches',
    '{"instruction":"Arrastra el operador correcto sobre la línea.","layout":"operator_compare","symbols":["<",">"],"rows":[{"imageUrl":"avares://Quibee/Assets/Images/RedCar.png","count":4,"imageWidth":42,"imageHeight":42,"rightImageUrl":"avares://Quibee/Assets/Images/BlueCar.png","rightCount":5,"rightImageWidth":42,"rightImageHeight":42,"correctSymbolAnswer":"<"}]}',
    '{"answer":"<"}',
    'easy',
    15,
    9,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 3 AND section_type = 'practiquemos'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text
FROM EXERCISE
WHERE id_lesson = 3 AND section_type = 'practiquemos'
ORDER BY order_index;