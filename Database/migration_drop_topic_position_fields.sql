-- Elimina columnas de posicionamiento/alineación de TOPIC
-- El mapa ahora usa un patrón automático por order_index.

USE quibee_db;

ALTER TABLE TOPIC
    DROP COLUMN position_x,
    DROP COLUMN position_y,
    DROP COLUMN text_on_left,
    DROP COLUMN text_on_right,
    DROP COLUMN rotation_angle;
