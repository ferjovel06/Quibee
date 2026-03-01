-- ============================================================
-- MIGRACIÓN: Separar contenido estático de ejercicios interactivos
-- LESSON_CONTENT → solo heading, text, image, visual_example sin respuesta
-- EXERCISE      → visual_count (visual_example con answerBox), matching
-- ============================================================

-- 1. Añadir columnas necesarias a EXERCISE y hacer question_text opcional
ALTER TABLE EXERCISE
    ADD COLUMN section_type VARCHAR(50) NULL AFTER id_lesson,
    ADD COLUMN instructions TEXT NULL AFTER section_type,
    MODIFY COLUMN question_text VARCHAR(500) NULL DEFAULT NULL;

-- 2. Migrar visual_example con correctAnswer (ejercicios interactivos) → EXERCISE
INSERT INTO EXERCISE (id_lesson, section_type, instructions, exercise_type, question_text, config, solution, difficulty, points, order_index, is_active, created_at)
SELECT
    lc.id_lesson,
    lc.section_type,
    '',   -- La instrucción está en la fila de texto adyacente en LESSON_CONTENT
    'visual_count',
    '',   -- question_text requerido por constraint, usar instructions en su lugar
    lc.content_data,
    JSON_OBJECT('answer', JSON_EXTRACT(lc.content_data, '$.correctAnswer')),
    'easy',
    10,
    lc.order_index,
    1,
    NOW()
FROM LESSON_CONTENT lc
WHERE lc.id_lesson = 2
  AND lc.content_type = 'visual_example'
  AND JSON_EXTRACT(lc.content_data, '$.correctAnswer') IS NOT NULL;

-- 3. Migrar matching_exercise → EXERCISE
INSERT INTO EXERCISE (id_lesson, section_type, instructions, exercise_type, question_text, config, solution, difficulty, points, order_index, is_active, created_at)
SELECT
    lc.id_lesson,
    lc.section_type,
    COALESCE(JSON_UNQUOTE(JSON_EXTRACT(lc.content_data, '$.instruction')), ''),
    'matching',
    COALESCE(JSON_UNQUOTE(JSON_EXTRACT(lc.content_data, '$.instruction')), ''),
    lc.content_data,
    '{}',
    'medium',
    20,
    lc.order_index,
    1,
    NOW()
FROM LESSON_CONTENT lc
WHERE lc.id_lesson = 2
  AND lc.content_type = 'matching_exercise';

-- 4. Eliminar filas interactivas de LESSON_CONTENT
DELETE FROM LESSON_CONTENT
WHERE id_lesson = 2
  AND (
    content_type = 'matching_exercise'
    OR (content_type = 'visual_example' AND JSON_EXTRACT(content_data, '$.correctAnswer') IS NOT NULL)
  );

-- 5. Limpiar lecciones duplicadas (ids 3 y 4 son duplicados del id 2)
--    Reasignar cualquier progreso a la lección canónica id=2
UPDATE STUDENT_LESSON_PROGRESS SET id_lesson = 2 WHERE id_lesson IN (3, 4);
DELETE FROM LESSON WHERE id_lesson IN (3, 4);

-- Verificación
SELECT 'LESSON_CONTENT restante:' AS info, content_type, section_type, COUNT(*) AS total
FROM LESSON_CONTENT WHERE id_lesson = 2
GROUP BY content_type, section_type
ORDER BY section_type, content_type;

SELECT 'EXERCISE migrado:' AS info, exercise_type, section_type, COUNT(*) AS total
FROM EXERCISE WHERE id_lesson = 2
GROUP BY exercise_type, section_type
ORDER BY section_type;

SELECT 'LESSON sin duplicados:' AS info, id_lesson, title FROM LESSON;
