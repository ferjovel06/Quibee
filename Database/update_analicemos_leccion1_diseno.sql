-- ========================================================
-- Actualiza la sección "analicemos" de la Lección 1 (id_lesson=1)
-- con el diseño de números del 0 al 9 + robot guía.
-- ========================================================

USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

-- Reemplazar contenido actual de la sección analicemos
DELETE FROM LESSON_CONTENT
WHERE id_lesson = 1
    AND section_type LIKE 'analicemos%';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    1,
    'heading',
    '{"text":"Los números del 0 al 10","level":2,"color":"#FFFFFF"}',
    1,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Para comenzar, aquí tienes los números del 0 al 10 con sus nombres:","fontSize":16,"color":"#FFFFFF"}',
    2,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/0.png","width":38,"height":58},{"symbol":"Cero","fontSize":30,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":38,"height":58},{"symbol":"Cinco","fontSize":30,"color":"#FFFFFF"}],"layout":"horizontal","spacing":16,"centerAlign":true}',
    3,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"symbol":"Uno","fontSize":30,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/6.png","width":38,"height":58},{"symbol":"Seis","fontSize":30,"color":"#FFFFFF"}],"layout":"horizontal","spacing":16,"centerAlign":true}',
    4,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/2.png","width":38,"height":58},{"symbol":"Dos","fontSize":30,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/7.png","width":38,"height":58},{"symbol":"Siete","fontSize":30,"color":"#FFFFFF"}],"layout":"horizontal","spacing":16,"centerAlign":true}',
    5,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/3.png","width":38,"height":58},{"symbol":"Tres","fontSize":30,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/8.png","width":38,"height":58},{"symbol":"Ocho","fontSize":30,"color":"#FFFFFF"}],"layout":"horizontal","spacing":16,"centerAlign":true}',
    6,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/4.png","width":38,"height":58},{"symbol":"Cuatro","fontSize":30,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/9.png","width":38,"height":58},{"symbol":"Nueve","fontSize":30,"color":"#FFFFFF"}],"layout":"horizontal","spacing":16,"centerAlign":true}',
    7,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/0.png","width":58,"height":58},{"symbol":"Diez","fontSize":30,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":true}',
    8,
    'analicemos',
    TRUE
),
(
    1,
    'heading',
    '{"text":"Cantidades que representa cada número","level":3,"color":"#FFFFFF"}',
    10,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"A continuación puedes ver las cantidades que representan cada uno de estos números:","fontSize":16,"color":"#FFFFFF"}',
    11,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":42,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/Pencil.png","width":42,"height":42},{"symbol":"→","fontSize":48,"color":"#F06292"},{"symbol":"El número uno indica una sola unidad: solo hay un objeto.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    12,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/2.png","width":42,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"emoji":"🍎","count":2,"fontSize":36},{"symbol":"→","fontSize":48,"color":"#F06292"},{"symbol":"El número dos es el primer número par: se puede dividir en dos partes iguales.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    13,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/3.png","width":42,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/Duck.png","count":3,"width":34,"height":34},{"symbol":"→","fontSize":48,"color":"#F06292"},{"symbol":"El número tres es impar: al agrupar, siempre queda un objeto sin pareja.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    14,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/4.png","width":42,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/Cookie.png","count":4,"width":34,"height":34},{"symbol":"→","fontSize":48,"color":"#F06292"},{"symbol":"El número cuatro es par y también es 2 + 2: dos parejas de objetos.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    15,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":42,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"emoji":"🍬","count":5,"fontSize":34},{"symbol":"→","fontSize":48,"color":"#F06292"},{"symbol":"El número cinco puede verse como 4 + 1: cuatro objetos y uno más.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    16,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/6.png","width":42,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"emoji":"🪙","count":6,"fontSize":34},{"symbol":"→","fontSize":48,"color":"#F06292"},{"symbol":"El número seis es par y se puede repartir en dos grupos iguales de tres.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    17,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/7.png","width":42,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"emoji":"📒","count":7,"fontSize":30},{"symbol":"→","fontSize":48,"color":"#F06292"},{"symbol":"El número siete es impar; al formar parejas queda uno sin pareja.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    18,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/8.png","width":42,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/Bee.png","count":8,"width":28,"height":28},{"symbol":"→","fontSize":48,"color":"#F06292"},{"symbol":"El número ocho es par y equivale a 4 + 4: dos grupos del mismo tamaño.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    19,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/9.png","width":42,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/RedBalloon.png","count":9,"width":28,"height":36},{"symbol":"→","fontSize":48,"color":"#F06292"},{"symbol":"El número nueve puede descomponerse como 5 + 4 para contar más fácil.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    20,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":42,"height":62},{"imageUrl":"avares://Quibee/Assets/Images/0.png","width":62,"height":62},{"symbol":":","fontSize":42,"color":"#FFFFFF"},{"imageUrl":"avares://Quibee/Assets/Images/Star2.png","count":5,"width":30,"height":30},{"imageUrl":"avares://Quibee/Assets/Images/Star2.png","count":5,"width":30,"height":30},{"symbol":"→","fontSize":52,"color":"#F06292"},{"symbol":"El diez forma una decena completa: diez unidades juntas.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    21,
    'analicemos',
    TRUE
),
(
    1,
    'heading',
    '{"text":"Aplicaciones en la vida diaria","level":3,"color":"#FFFFFF"}',
    22,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Veamos cómo los números del 0 al 9 se aplican en situaciones de la vida diaria a través de algunos ejemplos:","fontSize":16,"color":"#FFFFFF"}',
    23,
    'analicemos',
    TRUE
),
(
    1,
    'heading',
    '{"text":"Ejemplo 1: Contando Manzanas","level":3,"color":"#FFFFFF"}',
    24,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Imagina que estás en una tienda y ves diferentes cantidades de manzanas en varias cestas.","fontSize":15,"color":"#FFFFFF"}',
    25,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Cesta A: 1 manzana","fontSize":15,"color":"#FFFFFF","isBulletPoint":true}',
    26,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":1,"width":34,"height":34},{"symbol":"→","fontSize":52,"color":"#F06292"},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":42,"height":62}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    27,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Cesta B: 3 manzanas","fontSize":15,"color":"#FFFFFF","isBulletPoint":true}',
    28,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":3,"width":34,"height":34},{"symbol":"→","fontSize":52,"color":"#F06292"},{"imageUrl":"avares://Quibee/Assets/Images/3.png","width":42,"height":62}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    29,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Cesta C: 8 manzanas","fontSize":15,"color":"#FFFFFF","isBulletPoint":true}',
    30,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":8,"width":34,"height":34},{"symbol":"→","fontSize":52,"color":"#F06292"},{"imageUrl":"avares://Quibee/Assets/Images/8.png","width":42,"height":62}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    31,
    'analicemos',
    TRUE
),
(
    1,
    'heading',
    '{"text":"Ejemplo 2: Juguetes en un estante","level":3,"color":"#FFFFFF"}',
    32,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"En un estante de juguetes, hay diferentes cantidades de pelotas.","fontSize":15,"color":"#FFFFFF"}',
    33,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Estante A: 5 pelotas","fontSize":15,"color":"#FFFFFF","isBulletPoint":true}',
    34,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Ball.png","count":5,"width":32,"height":32},{"symbol":"→","fontSize":52,"color":"#F06292"},{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":42,"height":62}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    35,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Estante B: 2 pelotas","fontSize":15,"color":"#FFFFFF","isBulletPoint":true}',
    36,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Ball.png","count":2,"width":32,"height":32},{"symbol":"→","fontSize":52,"color":"#F06292"},{"imageUrl":"avares://Quibee/Assets/Images/2.png","width":42,"height":62}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    37,
    'analicemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Estante C: 9 pelotas","fontSize":15,"color":"#FFFFFF","isBulletPoint":true}',
    38,
    'analicemos',
    TRUE
),
(
    1,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Ball.png","count":9,"width":32,"height":32},{"symbol":"→","fontSize":52,"color":"#F06292"},{"imageUrl":"avares://Quibee/Assets/Images/9.png","width":42,"height":62}],"layout":"horizontal","spacing":14,"centerAlign":false}',
    39,
    'analicemos',
    TRUE
);

-- Reemplazar contenido y ejercicios de la sección practiquemos (Lección 1)
DELETE FROM LESSON_CONTENT
WHERE id_lesson = 1
  AND section_type = 'practiquemos';

DELETE FROM EXERCISE
WHERE id_lesson = 1
  AND section_type = 'practiquemos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    1,
    'heading',
    '{"text":"Practiquemos","level":1,"color":"#FFFFFF"}',
    1,
    'practiquemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Intenta resolver los siguientes ejercicios con la ayuda de tu docente:","fontSize":16,"color":"#FFFFFF"}',
    2,
    'practiquemos',
    TRUE
),
(
    1,
    'heading',
    '{"text":"Ejercicio 1","level":3,"color":"#FFFFFF"}',
    3,
    'practiquemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Cuenta las estrellas: Observa la imagen con estrellas y escribe cuántas ves.","fontSize":16,"color":"#FFFFFF"}',
    4,
    'practiquemos',
    TRUE
),
(
    1,
    'heading',
    '{"text":"Ejercicio 2","level":3,"color":"#FFFFFF"}',
    5,
    'practiquemos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Números en frutas: Imagina que tienes un tazón con frutas. Si hay 7 manzanas, ¿cómo escribirías ese número?","fontSize":16,"color":"#FFFFFF"}',
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
    1,
    'practiquemos',
    'Cuenta las estrellas y escribe la cantidad en el cuadro.',
    'visual_count',
    'Cuenta las estrellas',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Star2.png","count":8,"width":30,"height":30},{"symbol":"→","fontSize":52,"color":"#F06292"},{"answerBox":true,"width":84,"height":68},{"symbol":"Escribe tu respuesta en el cuadro.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":12,"centerAlign":true,"correctAnswer":8}',
    '{"answer":8}',
    'easy',
    10,
    4,
    TRUE,
    NOW(),
    NULL
),
(
    1,
    'practiquemos',
    'Cuenta las manzanas y escribe el número correcto en el cuadro.',
    'visual_count',
    'Números en frutas',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":7,"width":32,"height":32},{"symbol":"→","fontSize":52,"color":"#F06292"},{"answerBox":true,"width":84,"height":68},{"symbol":"Escribe tu respuesta en el cuadro.","fontSize":18,"color":"#FFFFFF"}],"layout":"horizontal","spacing":12,"centerAlign":true,"correctAnswer":7}',
    '{"answer":7}',
    'easy',
    10,
    7,
    TRUE,
    NOW(),
    NULL
);

-- Reemplazar contenido y ejercicios de la sección resolvamos (Lección 1)
DELETE FROM LESSON_CONTENT
WHERE id_lesson = 1
  AND section_type = 'resolvamos';

DELETE FROM EXERCISE
WHERE id_lesson = 1
  AND section_type = 'resolvamos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    1,
    'heading',
    '{"text":"Resolvamos","level":1,"color":"#FFFFFF"}',
    1,
    'resolvamos',
    TRUE
),
(
    1,
    'heading',
    '{"text":"Ejercicio 1: Identificación de Números","level":3,"color":"#FFFFFF"}',
    2,
    'resolvamos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Observa los siguientes dibujos y escribe el número de objetos que ves en el cuadro debajo de cada grupo:","fontSize":16,"color":"#FFFFFF"}',
    3,
    'resolvamos',
    TRUE
),
(
    1,
    'heading',
    '{"text":"Ejercicio 2: Contando","level":3,"color":"#FFFFFF"}',
    10,
    'resolvamos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Arrastra el número correspondiente a la cantidad de objetos.","fontSize":16,"color":"#FFFFFF"}',
    11,
    'resolvamos',
    TRUE
),
(
    1,
    'heading',
    '{"text":"Ejercicio 3: Números en secuencia","level":3,"color":"#FFFFFF"}',
    13,
    'resolvamos',
    TRUE
),
(
    1,
    'text',
    '{"text":"Completa la secuencia escribiendo los números que faltan.","fontSize":16,"color":"#FFFFFF"}',
    14,
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
    1,
    'resolvamos',
    'Cuenta cada grupo y completa las tres respuestas.',
    'visual_count',
    'Ejercicio 1: identificación de números',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/Cookie.png","count":5,"width":34,"height":34},{"answerBox":true,"width":72,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/Pencil.png","count":4,"width":36,"height":36},{"answerBox":true,"width":72,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/RedBalloon.png","count":3,"width":34,"height":42},{"answerBox":true,"width":72,"height":64}],"layout":"horizontal","spacing":14,"centerAlign":true,"correctAnswers":[5,4,3]}',
    '{"answers":[5,4,3]}',
    'easy',
    20,
    4,
    TRUE,
    NOW(),
    NULL
),
(
    1,
    'resolvamos',
    'Relaciona cada grupo con su número correcto.',
    'matching',
    'Ejercicio de arrastre',
    '{"instruction":"Arrastra el número correspondiente a cada grupo","numberImagePattern":"avares://Quibee/Assets/Images/{0}.png","numberImageWidth":70,"numberImageHeight":70,"rows":[{"imageUrl":"avares://Quibee/Assets/Images/Ball.png","count":6,"correctAnswer":6,"imageWidth":36,"imageHeight":36,"rowColor":"#5B4A6E"},{"imageUrl":"avares://Quibee/Assets/Images/Apple.png","count":2,"correctAnswer":2,"imageWidth":36,"imageHeight":36,"rowColor":"#4A3D5E"},{"imageUrl":"avares://Quibee/Assets/Images/Star2.png","count":8,"correctAnswer":8,"imageWidth":34,"imageHeight":34,"rowColor":"#6B5A7E"}]}',
    '{}',
    'medium',
    20,
    12,
    TRUE,
    NOW(),
    NULL
),
(
    1,
    'resolvamos',
    'Completa la secuencia numérica escribiendo los números que faltan.',
    'visual_count',
    'Números en secuencia',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/0.png","width":36,"height":52},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":36,"height":52},{"answerBox":true,"width":60,"height":56},{"imageUrl":"avares://Quibee/Assets/Images/3.png","width":36,"height":52},{"answerBox":true,"width":60,"height":56},{"answerBox":true,"width":60,"height":56},{"imageUrl":"avares://Quibee/Assets/Images/6.png","width":36,"height":52},{"answerBox":true,"width":60,"height":56},{"imageUrl":"avares://Quibee/Assets/Images/8.png","width":36,"height":52},{"imageUrl":"avares://Quibee/Assets/Images/9.png","width":36,"height":52}],"layout":"horizontal","spacing":10,"centerAlign":true,"correctAnswers":[2,4,5,7]}',
    '{"answers":[2,4,5,7]}',
    'easy',
    15,
    15,
    TRUE,
    NOW(),
    NULL
);

-- Reemplazar contenido y ejercicios de la sección desafio (Lección 1)
DELETE FROM LESSON_CONTENT
WHERE id_lesson = 1
  AND section_type = 'desafio';

DELETE FROM EXERCISE
WHERE id_lesson = 1
  AND section_type = 'desafio';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    1,
    'heading',
    '{"text":"Desafío","level":1,"color":"#FFFFFF"}',
    1,
    'desafio',
    TRUE
),
(
    1,
    'text',
    '{"text":"Para cerrar esta lección, te propongo un nuevo ejercicio que te ayudará a reforzar tu comprensión de los números del 0 al 9.","fontSize":16,"color":"#FFFFFF"}',
    2,
    'desafio',
    TRUE
),
(
    1,
    'text',
    '{"text":"A continuación, verás una serie de números en desorden. Tu tarea es organizarlos en el orden correcto.","fontSize":16,"color":"#FFFFFF"}',
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
    1,
    'desafio',
    'Arrastra y suelta los números para ordenarlos de 0 a 9. Tendrás un solo botón para comprobar todo el desafío.',
    'matching',
    'Ordena los números del 0 al 9',
    '{"layout":"sequence_grid","numbers":[0,3,1,4,9,2,5,8,7,6],"numberImagePattern":"avares://Quibee/Assets/Images/{0}.png","numberImageWidth":32,"numberImageHeight":48,"rows":[{"count":1,"correctAnswer":0},{"count":1,"correctAnswer":1},{"count":1,"correctAnswer":2},{"count":1,"correctAnswer":3},{"count":1,"correctAnswer":4},{"count":1,"correctAnswer":5},{"count":1,"correctAnswer":6},{"count":1,"correctAnswer":7},{"count":1,"correctAnswer":8},{"count":1,"correctAnswer":9}]}',
    '{"answers":[0,1,2,3,4,5,6,7,8,9]}',
    'medium',
    30,
    4,
    TRUE,
    NOW(),
    NULL
);

COMMIT;

-- Verificación rápida
SELECT id_content, content_type, order_index, section_type
FROM LESSON_CONTENT
WHERE id_lesson = 1 AND section_type = 'analicemos'
ORDER BY order_index;
