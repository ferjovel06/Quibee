USE quibee_db;

START TRANSACTION;

DELETE FROM LESSON_CONTENT
WHERE id_lesson = 2
  AND section_type = 'analicemos'
  AND content_type = 'visual_example'
  AND content_data LIKE '%Assets/Images/Robot.png%';

COMMIT;

SELECT id_content, order_index, content_type
FROM LESSON_CONTENT
WHERE id_lesson = 2
  AND section_type = 'analicemos'
ORDER BY order_index;
