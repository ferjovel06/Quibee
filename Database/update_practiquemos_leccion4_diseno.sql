USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 4
  AND section_type = 'practiquemos';

DELETE FROM EXERCISE
WHERE id_lesson = 4
  AND section_type = 'practiquemos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    4,
    'heading',
    '{"text":"Practiquemos","level":1,"color":"#FFFFFF"}',
    1,
    'practiquemos',
    TRUE
),
(
    4,
    'text',
    '{"text":"Ahora es tu turno de practicar con la ayuda de tu docente. Intenta resolver el siguiente ejercicio:","fontSize":16,"color":"#FFFFFF"}',
    2,
    'practiquemos',
    TRUE
),
(
    4,
    'text',
    '{"text":"Para cada grupo de objetos o cosas, cuéntalos y escribe la cantidad que representa cada grupo.","fontSize":16,"color":"#FFFFFF"}',
    3,
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
    4,
    'practiquemos',
    'Cuenta cada grupo y completa las tres casillas. Luego presiona Comprobar una sola vez.',
    'visual_count',
    'Cuenta balones, ranas y cupcakes',
    '{"objects":[{"symbol":"Escribe la cantidad de balones:","fontSize":20,"color":"#FFFFFF"},{"emoji":"🏀","count":12,"fontSize":40},{"answerBox":true,"width":98,"height":74},{"symbol":"Escribe la cantidad de ranas:","fontSize":20,"color":"#FFFFFF"},{"emoji":"🐸","count":18,"fontSize":34},{"answerBox":true,"width":98,"height":74},{"symbol":"Escribe la cantidad de cupcakes:","fontSize":20,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/Cupcake.png","count":14,"width":34,"height":42},{"answerBox":true,"width":98,"height":74}],"layout":"vertical","spacing":10,"centerAlign":false,"correctAnswers":[12,18,14]}',
    '{"answers":[12,18,14]}',
    'easy',
    30,
    4,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 4 AND section_type = 'practiquemos'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text
FROM EXERCISE
WHERE id_lesson = 4 AND section_type = 'practiquemos'
ORDER BY order_index;
