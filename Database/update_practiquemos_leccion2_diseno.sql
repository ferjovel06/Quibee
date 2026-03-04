USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 2
  AND section_type = 'practiquemos';

DELETE FROM EXERCISE
WHERE id_lesson = 2
  AND section_type = 'practiquemos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    2,
    'heading',
    '{"text":"Practiquemos","level":1,"color":"#FFFFFF"}',
    1,
    'practiquemos',
    TRUE
),
(
    2,
    'heading',
    '{"text":"Ejemplo 1","level":3,"color":"#FFFFFF"}',
    2,
    'practiquemos',
    TRUE
),
(
    2,
    'text',
    '{"text":"Un niño tiene 6 globos rojos y 2 globos azules. Escribe el número de globos rojos y de globos azules.","fontSize":16,"color":"#FFFFFF"}',
    3,
    'practiquemos',
    TRUE
),
(
    2,
    'heading',
    '{"text":"Ejemplo 2","level":3,"color":"#FFFFFF"}',
    5,
    'practiquemos',
    TRUE
),
(
    2,
    'text',
    '{"text":"En la mesa hay 4 libros de matemáticas y 3 libros de ciencia. Escribe el número de cada libro.","fontSize":16,"color":"#FFFFFF"}',
    6,
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
    2,
    'practiquemos',
    'Escribe cuántos globos rojos y cuántos globos azules hay.',
    'visual_count',
    'Globos rojos y azules',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/RedBalloon.png","count":6,"width":34,"height":46},{"answerBox":true,"width":74,"height":66},{"imageUrl":"avares://Quibee/Assets/Images/BlueBalloon.png","count":2,"width":34,"height":46},{"answerBox":true,"width":74,"height":66}],"layout":"horizontal","spacing":14,"centerAlign":true,"correctAnswers":[6,2]}',
    '{"answers":[6,2]}',
    'easy',
    15,
    4,
    TRUE,
    NOW(),
    NULL
),
(
    2,
    'practiquemos',
    'Escribe cuántos libros de matemáticas y cuántos libros de ciencia hay.',
    'visual_count',
    'Libros de matemáticas y ciencia',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/MathBook.png","count":4,"width":32,"height":38},{"answerBox":true,"width":74,"height":66},{"imageUrl":"avares://Quibee/Assets/Images/ScienceBook.png","count":3,"width":32,"height":38},{"answerBox":true,"width":74,"height":66}],"layout":"horizontal","spacing":14,"centerAlign":true,"correctAnswers":[4,3]}',
    '{"answers":[4,3]}',
    'easy',
    15,
    7,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 2 AND section_type = 'practiquemos'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text
FROM EXERCISE
WHERE id_lesson = 2 AND section_type = 'practiquemos'
ORDER BY order_index;
