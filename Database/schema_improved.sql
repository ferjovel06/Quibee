-- Create the database with UTF8MB4 support for emojis and international characters
-- IF NOT EXISTS ensures this script is idempotent (can be run multiple times safely)
CREATE DATABASE IF NOT EXISTS learning_platform
    CHARACTER SET utf8mb4 
    COLLATE utf8mb4_unicode_ci;
USE learning_platform;

-- ========================================
-- CONTENT HIERARCHY TABLES
-- ========================================

-- Table: LEVEL
-- Stores learning levels (e.g., Beginner, Intermediate, Advanced)
-- Each level groups related topics and provides progression structure
CREATE TABLE IF NOT EXISTS LEVEL (
    id_level INT AUTO_INCREMENT PRIMARY KEY,          -- Unique identifier for each level
    level_number INT NOT NULL UNIQUE,                 -- Display order (1, 2, 3...), must be unique
    level_name VARCHAR(100) NOT NULL,                 -- Display name (e.g., "Explorer", "Scientist")
    description TEXT,                                 -- Detailed description of the level
    icon VARCHAR(255),                                -- Path to icon/image for UI display
    is_active BOOLEAN DEFAULT TRUE,                   -- MEJORA: Permite desactivar niveles sin eliminarlos
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- MEJORA: Auditoría de creación
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP  -- MEJORA: Última actualización
);

-- Table: TOPIC
-- Topics within a level, organizing lessons by subject area
-- Example: "Electricity", "Robotics", "Space" under a "Science" level
CREATE TABLE IF NOT EXISTS TOPIC (
    id_topic INT AUTO_INCREMENT PRIMARY KEY,          -- Unique identifier for each topic
    id_level INT NOT NULL,                            -- Foreign key to parent level
    topic_name VARCHAR(100) NOT NULL,                 -- Display name of the topic
    description TEXT,                                 -- Detailed description
    icon VARCHAR(255),                                -- MEJORA: Icono para cada tema
    order_index INT NOT NULL,                         -- Display order within the level
    is_active BOOLEAN DEFAULT TRUE,                   -- MEJORA: Control de visibilidad
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- Creation timestamp for auditing
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,  -- MEJORA: Última actualización
    FOREIGN KEY (id_level) REFERENCES LEVEL(id_level) ON DELETE CASCADE,
    INDEX idx_level_order (id_level, order_index)     -- MEJORA: Índice para consultas de ordenamiento
);

-- Table: LESSON
-- Individual lessons within a topic, containing educational content
-- Each lesson can have text, multimedia, and associated exercises
CREATE TABLE IF NOT EXISTS LESSON (
    id_lesson INT AUTO_INCREMENT PRIMARY KEY,         -- Unique identifier for each lesson
    id_topic INT NOT NULL,                            -- Foreign key to parent topic
    title VARCHAR(100) NOT NULL,                      -- Lesson title
    content TEXT,                                     -- Main educational content (text)
    multimedia VARCHAR(255),                          -- Path to video/image/audio resource
    multimedia_type ENUM('image', 'video', 'audio', 'interactive') DEFAULT 'image',  -- MEJORA: Tipo de multimedia
    estimated_duration_minutes INT DEFAULT 10,        -- MEJORA: Duración estimada para completar
    order_index INT NOT NULL,                         -- Display order within the topic
    is_active BOOLEAN DEFAULT TRUE,                   -- MEJORA: Control de visibilidad
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- Creation timestamp
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,  -- MEJORA: Última actualización
    FOREIGN KEY (id_topic) REFERENCES TOPIC(id_topic) ON DELETE CASCADE,
    INDEX idx_topic_order (id_topic, order_index)     -- MEJORA: Índice para consultas de ordenamiento
);

-- Table: EXERCISE
-- Interactive exercises attached to lessons for student practice
-- Supports multiple exercise types with flexible JSON configuration
CREATE TABLE IF NOT EXISTS EXERCISE (
    id_exercise INT AUTO_INCREMENT PRIMARY KEY,       -- Unique identifier for each exercise
    id_lesson INT NOT NULL,                           -- Foreign key to parent lesson
    title VARCHAR(100),                               -- Exercise title
    instructions TEXT,                                -- Instructions for completing the exercise
    exercise_type ENUM('drag_drop', 'ordering', 'matching', 'multiple_choice', 'fill_blank') NOT NULL,  -- Type of interactive exercise
    difficulty ENUM('easy', 'medium', 'hard') DEFAULT 'medium',  -- MEJORA: Nivel de dificultad
    config JSON,                                      -- Exercise configuration (questions, options, items to match, etc.)
    solution JSON,                                    -- Correct answer(s) for validation
    points INT DEFAULT 10,                            -- Points awarded for correct completion
    order_index INT NOT NULL,                         -- Display order within the lesson
    is_active BOOLEAN DEFAULT TRUE,                   -- MEJORA: Control de visibilidad
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- Creation timestamp
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,  -- MEJORA: Última actualización
    FOREIGN KEY (id_lesson) REFERENCES LESSON(id_lesson) ON DELETE CASCADE,
    INDEX idx_lesson_order (id_lesson, order_index)   -- MEJORA: Índice para consultas de ordenamiento
);

-- ========================================
-- USER AND AUTHENTICATION TABLES
-- ========================================

-- Table: STUDENT
-- Stores student account information and authentication credentials
-- Each student can have progress across multiple levels and lessons
CREATE TABLE IF NOT EXISTS STUDENT (
    id_student INT AUTO_INCREMENT PRIMARY KEY,        -- Unique identifier for each student
    username VARCHAR(50) NOT NULL UNIQUE,             -- Login username, must be unique across platform
    password_hash VARCHAR(255) NOT NULL,              -- Hashed password (bcrypt/Argon2), NEVER store plaintext
    first_name VARCHAR(50),                           -- MEJORA: Separar nombres de apellidos
    last_name VARCHAR(50),                            -- MEJORA: Apellidos
    date_of_birth DATE,                               -- MEJORA: Fecha de nacimiento (para estadísticas y personalización)
    grade_level INT,                                  -- MEJORA: Grado escolar (1, 2, 3)
    gender ENUM('male', 'female', 'other', 'prefer_not_to_say'),  -- Gender for personalization (optional)
    avatar VARCHAR(255),                              -- Path to avatar image
    email VARCHAR(100) UNIQUE,                        -- MEJORA: Email (opcional pero útil para recuperación)
    is_active BOOLEAN DEFAULT TRUE,                   -- MEJORA: Permite desactivar cuentas sin eliminarlas
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,   -- Account registration timestamp
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,  -- MEJORA: Última actualización de perfil
    last_login TIMESTAMP NULL,                        -- Last successful login timestamp (NULL if never logged in)
    INDEX idx_username (username),                    -- MEJORA: Índice para búsquedas por username
    INDEX idx_email (email)                           -- MEJORA: Índice para búsquedas por email
);

-- ========================================
-- PROGRESS TRACKING TABLES
-- ========================================

-- Table: STUDENT_LEVEL_PROGRESS
-- Tracks which levels each student has unlocked and completed
-- Implements gamification: levels unlock as students progress
CREATE TABLE IF NOT EXISTS STUDENT_LEVEL_PROGRESS (
    id_student_level INT AUTO_INCREMENT PRIMARY KEY,  -- Unique identifier for this progress record
    id_student INT NOT NULL,                          -- Foreign key to student
    id_level INT NOT NULL,                            -- Foreign key to level
    is_unlocked BOOLEAN DEFAULT FALSE,                -- Whether student has access to this level
    is_completed BOOLEAN DEFAULT FALSE,               -- Whether student finished all content in this level
    completion_percentage DECIMAL(5,2) DEFAULT 0.00,  -- MEJORA: Porcentaje de completitud (0.00-100.00)
    unlocked_at TIMESTAMP NULL,                       -- When the level was unlocked
    completed_at TIMESTAMP NULL,                      -- When the level was completed
    FOREIGN KEY (id_student) REFERENCES STUDENT(id_student) ON DELETE CASCADE,
    FOREIGN KEY (id_level) REFERENCES LEVEL(id_level) ON DELETE CASCADE,
    UNIQUE KEY unique_student_level (id_student, id_level),
    INDEX idx_student_progress (id_student, is_completed)  -- MEJORA: Índice para consultas de progreso
);

-- Table: STUDENT_LESSON_PROGRESS
-- Tracks which lessons each student has completed
-- Finer-grained progress tracking than level-based tracking
CREATE TABLE IF NOT EXISTS STUDENT_LESSON_PROGRESS (
    id_lesson_progress INT AUTO_INCREMENT PRIMARY KEY, -- Unique identifier for this progress record
    id_student INT NOT NULL,                           -- Foreign key to student
    id_lesson INT NOT NULL,                            -- Foreign key to lesson
    is_completed BOOLEAN DEFAULT FALSE,                -- Whether student finished this lesson
    time_spent_minutes INT DEFAULT 0,                  -- MEJORA: Tiempo real invertido
    completed_at TIMESTAMP NULL,                       -- When the lesson was completed
    last_accessed TIMESTAMP NULL,                      -- MEJORA: Última vez que accedió a la lección
    FOREIGN KEY (id_student) REFERENCES STUDENT(id_student) ON DELETE CASCADE,
    FOREIGN KEY (id_lesson) REFERENCES LESSON(id_lesson) ON DELETE CASCADE,
    UNIQUE KEY unique_student_lesson (id_student, id_lesson),
    INDEX idx_student_lesson_progress (id_student, is_completed)  -- MEJORA: Índice para consultas de progreso
);

-- ========================================
-- EXERCISE TRACKING TABLES
-- ========================================

-- Table: EXERCISE_ATTEMPT
-- Records every attempt a student makes on an exercise
-- Allows multiple attempts per exercise for learning and improvement
-- Stores student's answer as JSON for flexible exercise types
CREATE TABLE IF NOT EXISTS EXERCISE_ATTEMPT (
    id_attempt INT AUTO_INCREMENT PRIMARY KEY,        -- Unique identifier for each attempt
    id_student INT NOT NULL,                          -- Foreign key to student who made the attempt
    id_exercise INT NOT NULL,                         -- Foreign key to the exercise attempted
    attempt_number INT DEFAULT 1,                     -- MEJORA: Número de intento (1, 2, 3...)
    student_answer JSON,                              -- Student's answer (format depends on exercise_type)
    is_correct BOOLEAN,                               -- Whether the answer was correct (NULL if not yet graded)
    points_earned INT DEFAULT 0,                      -- Points awarded for this attempt
    time_taken_seconds INT,                           -- MEJORA: Tiempo que tomó completar
    attempted_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- When the attempt was made
    FOREIGN KEY (id_student) REFERENCES STUDENT(id_student) ON DELETE CASCADE,
    FOREIGN KEY (id_exercise) REFERENCES EXERCISE(id_exercise) ON DELETE CASCADE,
    INDEX idx_student_exercise (id_student, id_exercise),  -- MEJORA: Índice para consultas de intentos
    INDEX idx_attempted_at (attempted_at)             -- MEJORA: Índice para consultas temporales
);

-- ========================================
-- GAMIFICATION AND STATISTICS TABLES
-- ========================================

-- Table: STUDENT_STATS
-- Aggregated statistics and gamification metrics for each student
-- This denormalized table improves performance for leaderboards and dashboards
-- Values should be updated by application logic or triggers when progress changes
CREATE TABLE IF NOT EXISTS STUDENT_STATS (
    id_stat INT AUTO_INCREMENT PRIMARY KEY,           -- Unique identifier for this stats record
    id_student INT NOT NULL UNIQUE,                   -- Foreign key to student (one stats record per student)
    current_level INT DEFAULT 1,                      -- Current level the student is working on
    total_points INT DEFAULT 0,                       -- Total points accumulated across all exercises
    lessons_completed INT DEFAULT 0,                  -- Count of completed lessons
    exercises_completed INT DEFAULT 0,                -- Count of correctly completed exercises
    exercises_attempted INT DEFAULT 0,                -- MEJORA: Total de ejercicios intentados
    accuracy_percentage DECIMAL(5,2) DEFAULT 0.00,    -- MEJORA: Precisión general (% de respuestas correctas)
    total_time_spent_minutes INT DEFAULT 0,           -- MEJORA: Tiempo total invertido en la plataforma
    current_streak INT DEFAULT 0,                     -- Current consecutive days of activity
    best_streak INT DEFAULT 0,                        -- Highest streak ever achieved by this student
    last_activity_date DATE,                          -- MEJORA: Última fecha de actividad (para calcular streaks)
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,  -- MEJORA: Última actualización
    FOREIGN KEY (id_student) REFERENCES STUDENT(id_student) ON DELETE CASCADE,
    INDEX idx_total_points (total_points DESC),       -- MEJORA: Índice para leaderboards
    INDEX idx_current_streak (current_streak DESC)    -- MEJORA: Índice para rankings de streaks
);

-- ========================================
-- MEJORAS ADICIONALES: NUEVAS TABLAS
-- ========================================

-- Table: ACHIEVEMENT
-- Define achievements/badges that students can earn
CREATE TABLE IF NOT EXISTS ACHIEVEMENT (
    id_achievement INT AUTO_INCREMENT PRIMARY KEY,
    achievement_name VARCHAR(100) NOT NULL,
    description TEXT,
    icon VARCHAR(255),
    criteria JSON,                                     -- Criterios para obtener el logro (ej: {"lessons_completed": 10})
    points_reward INT DEFAULT 0,                       -- Puntos bonus por obtener el logro
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table: STUDENT_ACHIEVEMENT
-- Tracks which achievements each student has earned
CREATE TABLE IF NOT EXISTS STUDENT_ACHIEVEMENT (
    id_student_achievement INT AUTO_INCREMENT PRIMARY KEY,
    id_student INT NOT NULL,
    id_achievement INT NOT NULL,
    earned_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_student) REFERENCES STUDENT(id_student) ON DELETE CASCADE,
    FOREIGN KEY (id_achievement) REFERENCES ACHIEVEMENT(id_achievement) ON DELETE CASCADE,
    UNIQUE KEY unique_student_achievement (id_student, id_achievement)
);

-- Table: SESSION_LOG
-- Logs student sessions for analytics
CREATE TABLE IF NOT EXISTS SESSION_LOG (
    id_session INT AUTO_INCREMENT PRIMARY KEY,
    id_student INT NOT NULL,
    login_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    logout_time TIMESTAMP NULL,
    session_duration_minutes INT,
    FOREIGN KEY (id_student) REFERENCES STUDENT(id_student) ON DELETE CASCADE,
    INDEX idx_student_sessions (id_student, login_time)
);
