-- ========================================================
-- Insertar contenido de la sección "desafio" para la lección 2
-- Ejercicio de emparejamiento: arrastrar números a grupos de objetos
-- ========================================================

USE quibee_db;

-- Primero verificar qué lecciones existen
-- SELECT id_lesson, title FROM LESSON;

-- Heading del desafío
INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES (
    2,
    'heading',
    '{"text": "¡Desafío!", "level": 1}',
    1,
    'desafio',
    TRUE
);

-- Instrucción de texto
INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES (
    2,
    'text',
    '{"text": "Observa cada grupo de objetos y arrastra el número que representa la cantidad correcta.", "fontSize": 18, "color": "#CCCCCC"}',
    2,
    'desafio',
    TRUE
);

-- Ejercicio de emparejamiento (matching_exercise)
INSERT INTO LESSON_CONTENT (id_lesson, content_type, content_data, order_index, section_type, is_active)
VALUES (
    2,
    'matching_exercise',
    '{
        "instruction": "Arrastra el número correcto a cada grupo",
        "numberImagePattern": "avares://Quibee/Assets/Images/{0}.png",
        "numberImageWidth": 70,
        "numberImageHeight": 70,
        "rows": [
            {
                "imageUrl": "avares://Quibee/Assets/Images/Star2.png",
                "count": 4,
                "correctAnswer": 4,
                "imageWidth": 60,
                "imageHeight": 60,
                "rowColor": "#5B4A6E"
            },
            {
                "imageUrl": "avares://Quibee/Assets/Images/GalacticCat.png",
                "count": 5,
                "correctAnswer": 5,
                "imageWidth": 60,
                "imageHeight": 60,
                "rowColor": "#4A3D5E"
            },
            {
                "imageUrl": "avares://Quibee/Assets/Images/PinkAlien.png",
                "count": 3,
                "correctAnswer": 3,
                "imageWidth": 60,
                "imageHeight": 60,
                "rowColor": "#6B5A7E"
            },
            {
                "imageUrl": "avares://Quibee/Assets/Images/Telescope.png",
                "count": 7,
                "correctAnswer": 7,
                "imageWidth": 60,
                "imageHeight": 60,
                "rowColor": "#3E3450"
            }
        ]
    }',
    3,
    'desafio',
    TRUE
);
