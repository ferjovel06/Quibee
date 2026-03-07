USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 5
  AND section_type = 'analicemos';

DELETE FROM EXERCISE
WHERE id_lesson = 5
  AND section_type = 'analicemos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    5,
    'heading',
    '{"text":"Analicemos","level":1,"color":"#FFFFFF"}',
    1,
    'analicemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Veamos algunos ejemplos que nos ayudarán a entender cómo aplicar la suma.","fontSize":16,"color":"#FFFFFF"}',
    2,
    'analicemos',
    TRUE
),
(
    5,
    'heading',
    '{"text":"Ejemplo 1: Manzanas","level":3,"color":"#FFFFFF"}',
    3,
    'analicemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Imagina que tienes un grupo de manzanas. Tienes 3 manzanas en una mano y 2 manzanas en la otra. Si juntas todas las manzanas, ¿cuántas tienes en total?","fontSize":16,"color":"#FFFFFF"}',
    4,
    'analicemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Manzanas en la mano izquierda: 3 manzanas","fontSize":16,"color":"#FFFFFF","isBulletPoint":true}',
    5,
    'analicemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Manzanas en la mano derecha: 2 manzanas","fontSize":16,"color":"#FFFFFF","isBulletPoint":true}',
    6,
    'analicemos',
    TRUE
),
(
    5,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":3,"width":38,"height":38},{"symbol":"+","fontSize":54,"color":"#55C4E8"},{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":2,"width":38,"height":38},{"symbol":"=","fontSize":54,"color":"#9B86D1"},{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":5,"width":38,"height":38}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    7,
    'analicemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Suma total: 3 + 2 = 5 (5 manzanas)","fontSize":16,"color":"#FFFFFF"}',
    8,
    'analicemos',
    TRUE
),
(
    5,
    'heading',
    '{"text":"Ejemplo 2: Cupcakes en la pastelería","level":3,"color":"#FFFFFF"}',
    9,
    'analicemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Vas a la pastelería, tú compras 6 cupcakes y tu amigo compra 4 cupcakes más. ¿Cuántos cupcakes tienen tú amigo y tú en total?","fontSize":16,"color":"#FFFFFF"}',
    10,
    'analicemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Tus cupcakes: 6 cupcakes","fontSize":16,"color":"#FFFFFF","isBulletPoint":true}',
    11,
    'analicemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Cupcakes de tu amigo: 4 cupcakes","fontSize":16,"color":"#FFFFFF","isBulletPoint":true}',
    12,
    'analicemos',
    TRUE
),
(
    5,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Cupcake.png","count":6,"width":38,"height":38},{"symbol":"+","fontSize":54,"color":"#55C4E8"},{"imageUrl":"avares://Quibee/Assets/Images/Cupcake.png","count":4,"width":38,"height":38},{"symbol":"=","fontSize":54,"color":"#9B86D1"},{"imageUrl":"avares://Quibee/Assets/Images/Cupcake.png","count":10,"width":38,"height":38}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    13,
    'analicemos',
    TRUE
),
(
    5,
    'text',
    '{"text":"Suma total: 6 + 4 = 10 (10 cupcakes)","fontSize":16,"color":"#FFFFFF"}',
    14,
    'analicemos',
    TRUE
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 5 AND section_type = 'analicemos'
ORDER BY order_index;
