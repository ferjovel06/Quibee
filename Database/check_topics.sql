-- Verificar temas del nivel 1
USE quibee_db;

-- Contar total de temas
SELECT COUNT(*) as 'Total Temas Nivel 1' FROM TOPIC WHERE id_level = 1;

-- Mostrar todos los temas
SELECT id_topic, order_index, topic_name, position_x, position_y 
FROM TOPIC 
WHERE id_level = 1 
ORDER BY order_index;

-- Buscar duplicados por order_index
SELECT order_index, COUNT(*) as cantidad 
FROM TOPIC 
WHERE id_level = 1 
GROUP BY order_index 
HAVING COUNT(*) > 1;
