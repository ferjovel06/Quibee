USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 4
  AND section_type = 'resolvamos';

DELETE FROM EXERCISE
WHERE id_lesson = 4
  AND section_type = 'resolvamos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    4,
    'heading',
    '{"text":"Resolvamos","level":1,"color":"#FFFFFF"}',
    1,
    'resolvamos',
    TRUE
),
(
    4,
    'text',
    '{"text":"Ahora resuelve estos problemas por ti mismo:","fontSize":16,"color":"#FFFFFF"}',
    2,
    'resolvamos',
    TRUE
  ),
  (
    4,
    'text',
    '{"text":"Ordena los números de menor a mayor arrastrando cada número completo (por ejemplo: 19, 17).","fontSize":16,"color":"#FFFFFF"}',
    4,
    'resolvamos',
    TRUE
  ),
  (
    4,
    'text',
    '{"text":"Escribe la cantidad de animales que puedes contar:","fontSize":18,"color":"#FFFFFF"}',
    7,
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
    4,
    'resolvamos',
  'Ordena de menor a mayor arrastrando fichas.',
  'matching',
  'Ordena: 17, 14, 19, 15',
  '{"layout":"order_tokens","symbols":["17","14","19","15"],"rows":[{"correctSymbolAnswer":"14"},{"correctSymbolAnswer":"15"},{"correctSymbolAnswer":"17"},{"correctSymbolAnswer":"19"}]}',
  '{"answers":["14","15","17","19"]}',
  'easy',
  15,
  3,
  TRUE,
  NOW(),
  NULL
),
(
  4,
  'resolvamos',
  'Cuenta los animales y completa las casillas.',
  'visual_count',
  'Conteo de animales',
  '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Cow.png","count":13,"width":34,"height":34},{"symbol":"Total de vacas:","fontSize":18,"color":"#FFFFFF"},{"answerBox":true,"width":98,"height":74},{"imageUrl":"avares://Quibee/Assets/Images/Bee.png","count":16,"width":32,"height":32},{"symbol":"Total de abejas:","fontSize":18,"color":"#FFFFFF"},{"answerBox":true,"width":98,"height":74},{"imageUrl":"avares://Quibee/Assets/Images/Duck.png","count":17,"width":34,"height":34},{"symbol":"Total de patos:","fontSize":18,"color":"#FFFFFF"},{"answerBox":true,"width":98,"height":74}],"layout":"vertical","spacing":10,"centerAlign":false,"correctAnswers":[13,16,17]}',
  '{"answers":[13,16,17]}',
    'medium',
  15,
  8,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 4 AND section_type = 'resolvamos'
ORDER BY order_index;

SELECT id_exercise, order_index, exercise_type, question_text
FROM EXERCISE
WHERE id_lesson = 4 AND section_type = 'resolvamos'
ORDER BY order_index;
