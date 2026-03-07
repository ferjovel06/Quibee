USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 5
  AND section_type = 'resolvamos';

DELETE FROM EXERCISE
WHERE id_lesson = 5
  AND section_type = 'resolvamos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    5,
    'heading',
    '{"text":"Resolvamos","level":1,"color":"#FFFFFF"}',
    1,
    'resolvamos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Ahora que hemos visto algunos ejemplos, es tu turno de practicar. Intenta resolver los siguientes ejercicios:","fontSize":16,"color":"#FFFFFF"}',
    2,
    'resolvamos',
    TRUE
),
(
    5,
    'heading',
    '{"text":"Ejercicio 1","level":3,"color":"#FFFFFF"}',
    3,
    'resolvamos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Ayer compraste 7 galletas y hoy compraste 5, ¿cuántas galletas compraste en total?","fontSize":16,"color":"#FFFFFF"}',
    4,
    'resolvamos',
    TRUE
),
(
    5,
    'heading',
    '{"text":"Ejercicio 2","level":3,"color":"#FFFFFF"}',
    7,
    'resolvamos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Si tu mamá te dio 5 pesos y tu abuela te dio 10, ¿cuántos pesos tienes?","fontSize":16,"color":"#FFFFFF"}',
    8,
    'resolvamos',
    TRUE
),
(
    5,
    'heading',
    '{"text":"Ejercicio 3","level":3,"color":"#FFFFFF"}',
    11,
    'resolvamos',
    TRUE
),
(
    5,
    'text',
    '{"text":"En la estación de transporte hay 4 autobuses y luego llegan 7 más, ¿cuántos autobuses hay ahora en la estación?","fontSize":16,"color":"#FFFFFF"}',
    12,
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
    5,
    'resolvamos',
    'Cuenta y escribe la suma total.',
    'visual_count',
    'Galletas: 7 + 5',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/7.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/Cookie.png","count":7,"width":32,"height":32},{"symbol":"+","fontSize":52,"color":"#55C4E8"},{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/Cookie.png","count":5,"width":32,"height":32},{"symbol":"=","fontSize":52,"color":"#9B86D1"},{"answerBox":true,"width":98,"height":74}],"layout":"horizontal","spacing":10,"centerAlign":true,"correctAnswer":12}',
    '{"answer":12}',
    'easy',
    10,
    5,
    TRUE,
    NOW(),
    NULL
),
(
    5,
    'resolvamos',
    'Cuenta y escribe la suma total.',
    'visual_count',
    'Pesos: 5 + 10',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/Coin.png","count":5,"width":34,"height":34},{"symbol":"+","fontSize":52,"color":"#55C4E8"},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/0.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/Coin.png","count":10,"width":34,"height":34},{"symbol":"=","fontSize":52,"color":"#9B86D1"},{"answerBox":true,"width":98,"height":74}],"layout":"horizontal","spacing":10,"centerAlign":true,"correctAnswer":15}',
    '{"answer":15}',
    'easy',
    10,
    9,
    TRUE,
    NOW(),
    NULL
),
(
    5,
    'resolvamos',
    'Cuenta y escribe la suma total.',
    'visual_count',
    'Autobuses: 4 + 7',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/4.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/Bus.png","count":4,"width":48,"height":36},{"symbol":"+","fontSize":52,"color":"#55C4E8"},{"imageUrl":"avares://Quibee/Assets/Images/7.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/Bus.png","count":7,"width":48,"height":36},{"symbol":"=","fontSize":52,"color":"#9B86D1"},{"answerBox":true,"width":98,"height":74}],"layout":"horizontal","spacing":10,"centerAlign":true,"correctAnswer":11}',
    '{"answer":11}',
    'easy',
    10,
    13,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 5 AND section_type = 'resolvamos'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text
FROM EXERCISE
WHERE id_lesson = 5 AND section_type = 'resolvamos'
ORDER BY order_index;
