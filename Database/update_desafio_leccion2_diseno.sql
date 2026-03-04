USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 2
  AND section_type = 'desafio';

DELETE FROM EXERCISE
WHERE id_lesson = 2
  AND section_type = 'desafio';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    2,
    'heading',
    '{"text":"Desafío","level":1,"color":"#FFFFFF"}',
    1,
    'desafio',
    TRUE
),
(
    2,
    'heading',
    '{"text":"Relacionemos números y objetos","level":3,"color":"#FFFFFF"}',
    2,
    'desafio',
    TRUE
),
(
    2,
    'text',
    '{"text":"¿Cuántos objetos puedes contar?","fontSize":16,"color":"#FFFFFF"}',
    3,
    'desafio',
    TRUE
),
(
    2,
    'text',
    '{"text":"Coloca los números en las casillas correspondientes:","fontSize":16,"color":"#FFFFFF"}',
    4,
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
    2,
    'desafio',
    'Relaciona cada grupo con su número correcto.',
    'matching',
    'Relacionemos números y objetos',
    '{"layout":"challenge_board","numberImagePattern":"avares://Quibee/Assets/Images/{0}.png","numberImageWidth":44,"numberImageHeight":64,"numbers":[4,5,3,6,7],"rows":[{"imageUrl":"avares://Quibee/Assets/Images/SmallStar.png","count":4,"correctAnswer":4,"imageWidth":32,"imageHeight":32,"rowColor":"#5B4A6E"},{"imageUrl":"avares://Quibee/Assets/Images/GalacticCat.png","count":5,"correctAnswer":5,"imageWidth":28,"imageHeight":28,"rowColor":"#5B4A6E"},{"imageUrl":"avares://Quibee/Assets/Images/PinkAlien.png","count":3,"correctAnswer":3,"imageWidth":30,"imageHeight":30,"rowColor":"#5B4A6E"},{"imageUrl":"avares://Quibee/Assets/Images/Telescope.png","count":6,"correctAnswer":6,"imageWidth":34,"imageHeight":34,"rowColor":"#5B4A6E"},{"imageUrl":"avares://Quibee/Assets/Images/SpaceDog.png","count":7,"correctAnswer":7,"imageWidth":26,"imageHeight":26,"rowColor":"#5B4A6E"}]}',
    '{"answers":[4,5,3,6,7]}',
    'medium',
    25,
    5,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 2 AND section_type = 'desafio'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text, JSON_EXTRACT(config,'$.layout') AS layout_mode
FROM EXERCISE
WHERE id_lesson = 2 AND section_type = 'desafio'
ORDER BY order_index;
