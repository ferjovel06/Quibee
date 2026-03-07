USE quibee_db;

SET NAMES utf8mb4;
SET character_set_client = utf8mb4;
SET character_set_connection = utf8mb4;
SET character_set_results = utf8mb4;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 4
  AND section_type = 'analicemos';

DELETE FROM EXERCISE
WHERE id_lesson = 4
  AND section_type = 'analicemos';

INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES
(
    4,
    'heading',
    '{"text":"Analicemos","level":1,"color":"#FFFFFF"}',
    1,
    'analicemos',
    TRUE
),
(
    4,
    'text',
    '{"text":"Aprendamos cómo leer los números del 11 al 20.","fontSize":16,"color":"#FFFFFF"}',
    2,
    'analicemos',
    TRUE
),
(
    4,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"symbol":"Once","fontSize":28,"color":"#FFFFFF"},{"symbol":"     "},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/5.png","width":38,"height":58},{"symbol":"Quince","fontSize":28,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    3,
    'analicemos',
    TRUE
),
(
    4,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/2.png","width":38,"height":58},{"symbol":"Doce","fontSize":28,"color":"#FFFFFF"},{"symbol":"     "},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/6.png","width":38,"height":58},{"symbol":"Dieciséis","fontSize":28,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    4,
    'analicemos',
    TRUE
),
(
    4,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/3.png","width":38,"height":58},{"symbol":"Trece","fontSize":28,"color":"#FFFFFF"},{"symbol":"     "},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/7.png","width":38,"height":58},{"symbol":"Diecisiete","fontSize":28,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    5,
    'analicemos',
    TRUE
),
(
    4,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/4.png","width":38,"height":58},{"symbol":"Catorce","fontSize":28,"color":"#FFFFFF"},{"symbol":"     "},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/8.png","width":38,"height":58},{"symbol":"Dieciocho","fontSize":28,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    6,
    'analicemos',
    TRUE
),
(
    4,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/9.png","width":38,"height":58},{"symbol":"Diecinueve","fontSize":28,"color":"#FFFFFF"},{"symbol":"     "},{"imageUrl":"avares://Quibee/Assets/Images/2.png","width":38,"height":58},{"imageUrl":"avares://Quibee/Assets/Images/0.png","width":38,"height":58},{"symbol":"Veinte","fontSize":28,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    7,
    'analicemos',
    TRUE
),
(
    4,
    'text',
    '{"text":"Veamos algunos ejemplos que nos ayudarán a entender cómo aplicar los números del 11 al 20 en situaciones reales:","fontSize":16,"color":"#FFFFFF"}',
    8,
    'analicemos',
    TRUE
),
(
    4,
    'heading',
    '{"text":"Ejemplo 1: Contando Juguetes","level":3,"color":"#FFFFFF"}',
    9,
    'analicemos',
    TRUE
),
(
    4,
    'text',
    '{"text":"Imagina que estás organizando tus juguetes y quieres saber cuántos tienes en total. Contemos juntos:","fontSize":16,"color":"#FFFFFF"}',
    10,
    'analicemos',
    TRUE
),
(
    4,
    'text',
    '{"text":"Peluches: 11 peluches","fontSize":16,"color":"#FFFFFF","isBulletPoint":true}',
    11,
    'analicemos',
    TRUE
),
(
    4,
    'text',
    '{"text":"Autos de juguete: 13 autos","fontSize":16,"color":"#FFFFFF","isBulletPoint":true}',
    12,
    'analicemos',
    TRUE
),
(
    4,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/TeddyBear.png","count":11,"width":34,"height":34},{"symbol":"→","fontSize":52,"color":"#F06292"},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":44,"height":64},{"symbol":"peluches","fontSize":24,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    13,
    'analicemos',
    TRUE
),
(
    4,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/RedCar.png","count":13,"width":36,"height":24},{"symbol":"→","fontSize":52,"color":"#F06292"},{"imageUrl":"avares://Quibee/Assets/Images/1.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/3.png","width":44,"height":64},{"symbol":"autos de juguete","fontSize":24,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    14,
    'analicemos',
    TRUE
),
(
    4,
    'heading',
    '{"text":"Ejemplo 2: Libros en una Biblioteca","level":3,"color":"#FFFFFF"}',
    15,
    'analicemos',
    TRUE
),
(
    4,
    'text',
    '{"text":"En una pequeña biblioteca, contamos el número de libros en un estante. Contemos juntos:","fontSize":16,"color":"#FFFFFF"}',
    16,
    'analicemos',
    TRUE
),
(
    4,
    'text',
    '{"text":"Libros: 20 libros","fontSize":16,"color":"#FFFFFF","isBulletPoint":true}',
    17,
    'analicemos',
    TRUE
),
(
    4,
    'visual_example',
    '{"objects":[{"imageUrl":"avares://Quibee/Assets/Images/MathBook.png","count":10,"width":30,"height":40},{"imageUrl":"avares://Quibee/Assets/Images/ScienceBook.png","count":10,"width":30,"height":40},{"symbol":"→","fontSize":52,"color":"#F06292"},{"imageUrl":"avares://Quibee/Assets/Images/2.png","width":44,"height":64},{"imageUrl":"avares://Quibee/Assets/Images/0.png","width":44,"height":64},{"symbol":"libros en el estante","fontSize":24,"color":"#FFFFFF"}],"layout":"horizontal","spacing":10,"centerAlign":true}',
    18,
    'analicemos',
    TRUE
);

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 4 AND section_type = 'analicemos'
ORDER BY order_index;
