-- MySQL dump 10.13  Distrib 8.4.8, for Linux (x86_64)
--
-- Host: localhost    Database: quibee_db
-- ------------------------------------------------------
-- Server version	11.4.9-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `ACHIEVEMENT`
--

DROP TABLE IF EXISTS `ACHIEVEMENT`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ACHIEVEMENT` (
  `id_achievement` int(11) NOT NULL AUTO_INCREMENT,
  `achievement_name` varchar(100) NOT NULL,
  `description` varchar(500) DEFAULT NULL,
  `icon` varchar(100) DEFAULT NULL,
  `criteria` varchar(1000) DEFAULT NULL,
  `points_reward` int(11) DEFAULT NULL,
  `created_at` datetime(6) NOT NULL,
  PRIMARY KEY (`id_achievement`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ACHIEVEMENT`
--

LOCK TABLES `ACHIEVEMENT` WRITE;
/*!40000 ALTER TABLE `ACHIEVEMENT` DISABLE KEYS */;
/*!40000 ALTER TABLE `ACHIEVEMENT` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `EXERCISE`
--

DROP TABLE IF EXISTS `EXERCISE`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `EXERCISE` (
  `id_exercise` int(11) NOT NULL AUTO_INCREMENT,
  `id_lesson` int(11) NOT NULL,
  `section_type` varchar(50) DEFAULT NULL,
  `instructions` text DEFAULT NULL,
  `exercise_type` varchar(30) NOT NULL,
  `question_text` varchar(500) DEFAULT NULL,
  `config` varchar(2000) DEFAULT NULL,
  `solution` varchar(1000) DEFAULT NULL,
  `difficulty` varchar(10) DEFAULT NULL,
  `points` int(11) NOT NULL,
  `order_index` int(11) NOT NULL,
  `is_active` tinyint(1) NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_exercise`),
  KEY `IX_EXERCISE_id_lesson` (`id_lesson`),
  CONSTRAINT `FK_EXERCISE_LESSON_id_lesson` FOREIGN KEY (`id_lesson`) REFERENCES `LESSON` (`id_lesson`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `EXERCISE`
--

LOCK TABLES `EXERCISE` WRITE;
/*!40000 ALTER TABLE `EXERCISE` DISABLE KEYS */;
INSERT INTO `EXERCISE` VALUES (1,2,'practiquemos','','visual_count','','{\"objects\":[{\"imageUrl\":\"avares://Quibee/Assets/Images/6.png\",\"width\":34,\"height\":52},{\"imageUrl\":\"avares://Quibee/Assets/Images/RedBalloon.png\",\"count\":6,\"width\":34,\"height\":50},{\"imageUrl\":\"avares://Quibee/Assets/Images/PlusSign.png\",\"width\":32,\"height\":32},{\"imageUrl\":\"avares://Quibee/Assets/Images/2.png\",\"width\":34,\"height\":52},{\"imageUrl\":\"avares://Quibee/Assets/Images/BlueBalloon.png\",\"count\":2,\"width\":34,\"height\":50},{\"imageUrl\":\"avares://Quibee/Assets/Images/EqualSign.png\",\"width\":32,\"height\":32},{\"answerBox\":true,\"width\":84,\"height\":68}],\"layout\":\"horizontal\",\"spacing\":12,\"centerAlign\":true,\"correctAnswer\":8}','{\"answer\": 8}','easy',10,3,1,'2026-02-23 00:07:10.000000',NULL),(2,2,'practiquemos','','visual_count','','{\"objects\":[{\"imageUrl\":\"avares://Quibee/Assets/Images/4.png\",\"width\":34,\"height\":52},{\"imageUrl\":\"avares://Quibee/Assets/Images/MathBook.png\",\"count\":4,\"width\":32,\"height\":38},{\"imageUrl\":\"avares://Quibee/Assets/Images/PlusSign.png\",\"width\":32,\"height\":32},{\"imageUrl\":\"avares://Quibee/Assets/Images/3.png\",\"width\":34,\"height\":52},{\"imageUrl\":\"avares://Quibee/Assets/Images/ScienceBook.png\",\"count\":3,\"width\":32,\"height\":38},{\"imageUrl\":\"avares://Quibee/Assets/Images/EqualSign.png\",\"width\":32,\"height\":32},{\"answerBox\":true,\"width\":84,\"height\":68}],\"layout\":\"horizontal\",\"spacing\":12,\"centerAlign\":true,\"correctAnswer\":7}','{\"answer\": 7}','easy',10,6,1,'2026-02-23 00:07:10.000000',NULL),(3,2,'resolvamos','','visual_count','','{\"objects\":[{\"number\":\"5\",\"color\":\"#4FD1C5\",\"fontSize\":62},{\"imageUrl\":\"avares://Quibee/Assets/Images/Butterfly.png\",\"count\":5,\"width\":42,\"height\":42},{\"symbol\":\"+\",\"color\":\"#66D3F2\",\"fontSize\":58},{\"number\":\"3\",\"color\":\"#B682D5\",\"fontSize\":62},{\"imageUrl\":\"avares://Quibee/Assets/Images/Bee.png\",\"count\":3,\"width\":42,\"height\":42},{\"symbol\":\"=\",\"color\":\"#B682D5\",\"fontSize\":58},{\"answerBox\":true,\"width\":80,\"height\":80}],\"layout\":\"horizontal\",\"spacing\":10,\"correctAnswer\":8}','{\"answer\": 8}','easy',10,3,1,'2026-02-23 00:07:10.000000',NULL),(4,2,'resolvamos','','visual_count','','{\"objects\":[{\"number\":\"4\",\"color\":\"#FF8C42\",\"fontSize\":62},{\"imageUrl\":\"avares://Quibee/Assets/Images/Pencil.png\",\"count\":4,\"width\":36,\"height\":36},{\"symbol\":\"+\",\"color\":\"#66D3F2\",\"fontSize\":58},{\"number\":\"2\",\"color\":\"#FF5A5F\",\"fontSize\":62},{\"imageUrl\":\"avares://Quibee/Assets/Images/Pencil.png\",\"count\":2,\"width\":36,\"height\":36},{\"symbol\":\"=\",\"color\":\"#B682D5\",\"fontSize\":58},{\"answerBox\":true,\"width\":80,\"height\":80}],\"layout\":\"horizontal\",\"spacing\":10,\"correctAnswer\":6}','{\"answer\": 6}','easy',10,6,1,'2026-02-23 00:07:10.000000',NULL),(5,2,'resolvamos','','visual_count','','{\"objects\":[{\"number\":\"6\",\"color\":\"#E8A0C8\",\"fontSize\":62},{\"imageUrl\":\"avares://Quibee/Assets/Images/Duck.png\",\"count\":6,\"width\":38,\"height\":38},{\"symbol\":\"+\",\"color\":\"#66D3F2\",\"fontSize\":58},{\"number\":\"2\",\"color\":\"#FF5A5F\",\"fontSize\":62},{\"imageUrl\":\"avares://Quibee/Assets/Images/Duck.png\",\"count\":2,\"width\":38,\"height\":38},{\"symbol\":\"=\",\"color\":\"#B682D5\",\"fontSize\":58},{\"answerBox\":true,\"width\":80,\"height\":80}],\"layout\":\"horizontal\",\"spacing\":10,\"correctAnswer\":8}','{\"answer\": 8}','easy',10,9,1,'2026-02-23 00:07:10.000000',NULL),(8,2,'desafio','Arrastra el número correcto a cada grupo','matching','Arrastra el número correcto a cada grupo','{\n        \"instruction\": \"Arrastra el número correcto a cada grupo\",\n        \"numberImagePattern\": \"avares://Quibee/Assets/Images/{0}.png\",\n        \"numberImageWidth\": 70,\n        \"numberImageHeight\": 70,\n        \"rows\": [\n            {\n                \"imageUrl\": \"avares://Quibee/Assets/Images/Star2.png\",\n                \"count\": 4,\n                \"correctAnswer\": 4,\n                \"imageWidth\": 60,\n                \"imageHeight\": 60,\n                \"rowColor\": \"#5B4A6E\"\n            },\n            {\n                \"imageUrl\": \"avares://Quibee/Assets/Images/GalacticCat.png\",\n                \"count\": 5,\n                \"correctAnswer\": 5,\n                \"imageWidth\": 60,\n                \"imageHeight\": 60,\n                \"rowColor\": \"#4A3D5E\"\n            },\n            {\n                \"imageUrl\": \"avares://Quibee/Assets/Images/PinkAlien.png\",\n                \"count\": 3,\n                \"correctAnswer\": 3,\n                \"imageWidth\": 60,\n                \"imageHeight\": 60,\n                \"rowColor\": \"#6B5A7E\"\n            },\n            {\n                \"imageUrl\": \"avares://Quibee/Assets/Images/Telescope.png\",\n                \"count\": 7,\n                \"correctAnswer\": 7,\n                \"imageWidth\": 60,\n                \"imageHeight\": 60,\n                \"rowColor\": \"#3E3450\"\n            }\n        ]\n    }','{}','medium',20,3,1,'2026-02-23 00:07:10.000000',NULL);
/*!40000 ALTER TABLE `EXERCISE` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `EXERCISE_ATTEMPT`
--

DROP TABLE IF EXISTS `EXERCISE_ATTEMPT`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `EXERCISE_ATTEMPT` (
  `id_exercise_attempt` int(11) NOT NULL AUTO_INCREMENT,
  `id_student` int(11) NOT NULL,
  `id_exercise` int(11) NOT NULL,
  `attempt_number` int(11) NOT NULL,
  `student_answer` varchar(1000) DEFAULT NULL,
  `is_correct` tinyint(1) NOT NULL,
  `points_earned` int(11) DEFAULT NULL,
  `time_taken_seconds` int(11) DEFAULT NULL,
  `attempt_date` datetime(6) NOT NULL,
  PRIMARY KEY (`id_exercise_attempt`),
  KEY `IX_EXERCISE_ATTEMPT_id_student` (`id_student`),
  CONSTRAINT `FK_EXERCISE_ATTEMPT_STUDENT_id_student` FOREIGN KEY (`id_student`) REFERENCES `STUDENT` (`id_student`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `EXERCISE_ATTEMPT`
--

LOCK TABLES `EXERCISE_ATTEMPT` WRITE;
/*!40000 ALTER TABLE `EXERCISE_ATTEMPT` DISABLE KEYS */;
/*!40000 ALTER TABLE `EXERCISE_ATTEMPT` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `LESSON`
--

DROP TABLE IF EXISTS `LESSON`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LESSON` (
  `id_lesson` int(11) NOT NULL AUTO_INCREMENT,
  `id_topic` int(11) NOT NULL,
  `title` varchar(200) NOT NULL,
  `content` varchar(2000) DEFAULT NULL,
  `multimedia_url` varchar(255) DEFAULT NULL,
  `multimedia_type` varchar(20) DEFAULT NULL,
  `order_index` int(11) NOT NULL,
  `estimated_duration_minutes` int(11) DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_lesson`),
  KEY `IX_LESSON_id_topic` (`id_topic`),
  CONSTRAINT `FK_LESSON_TOPIC_id_topic` FOREIGN KEY (`id_topic`) REFERENCES `TOPIC` (`id_topic`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `LESSON`
--

LOCK TABLES `LESSON` WRITE;
/*!40000 ALTER TABLE `LESSON` DISABLE KEYS */;
INSERT INTO `LESSON` VALUES (2,602,'Lección 1: Relacionemos números y objetos','Lección completa con todas las secciones',NULL,NULL,1,30,1,'2026-02-13 16:40:58.000000','2026-02-13 16:40:58.000000');
/*!40000 ALTER TABLE `LESSON` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `LESSON_CONTENT`
--

DROP TABLE IF EXISTS `LESSON_CONTENT`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LESSON_CONTENT` (
  `id_content` int(11) NOT NULL AUTO_INCREMENT,
  `id_lesson` int(11) NOT NULL,
  `content_type` varchar(50) NOT NULL,
  `content_data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL CHECK (json_valid(`content_data`)),
  `order_index` int(11) NOT NULL,
  `section_type` varchar(50) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_at` timestamp NULL DEFAULT current_timestamp(),
  `updated_at` timestamp NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id_content`),
  KEY `idx_lesson_section_order` (`id_lesson`,`section_type`,`order_index`),
  CONSTRAINT `LESSON_CONTENT_ibfk_1` FOREIGN KEY (`id_lesson`) REFERENCES `LESSON` (`id_lesson`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `LESSON_CONTENT`
--

LOCK TABLES `LESSON_CONTENT` WRITE;
/*!40000 ALTER TABLE `LESSON_CONTENT` DISABLE KEYS */;
INSERT INTO `LESSON_CONTENT` VALUES (11,2,'heading','{\"text\": \"Lección 1: Relacionemos números y objetos\", \"level\": 1, \"color\": \"#311B42\"}',1,'introduccion',1,'2026-02-13 13:45:32','2026-02-15 19:32:15'),(12,2,'text','{\"text\": \"Bienvenidos a esta lección de matemáticas donde aprenderemos a relacionar números con objetos. Este concepto es fundamental para comprender cómo los números representan cantidades en el mundo real. En esta lección, exploraremos cómo vincular números con objetos, veremos ejemplos prácticos y practicaremos con ejercicios propuestos.\", \"fontSize\": 16, \"color\": \"#FFFFFF\"}',2,'introduccion',1,'2026-02-13 13:45:32','2026-02-16 12:02:31'),(13,2,'heading','{\"text\": \"¿Qué significa relacionar números con objetos?\", \"level\": 3, \"color\": \"#FFFFFF\"}',3,'introduccion',1,'2026-02-13 13:45:32','2026-02-17 19:30:46'),(14,2,'text','{\"text\": \"Relacionar números con objetos es el proceso de asignar un valor numérico a un grupo de elementos reales, que se pueden ver y tocar. Esto nos ayuda a cuantificar y entender mejor nuestro entorno. Por ejemplo, cuando decimos que hay \\\"5 manzanas\\\", estamos utilizando el número 5 para describir una cantidad específica de manzanas.\", \"fontSize\": 16, \"color\": \"#FFFFFF\"}',4,'introduccion',1,'2026-02-13 13:45:32','2026-02-16 12:02:31'),(15,2,'text','{\"text\": \"Imagina que estás contando las galletas en un plato. Cada galleta representa un objeto, y al contarlas, obtienes un número total que describe cuántas galletas hay.\", \"fontSize\": 16, \"color\": \"#FFFFFF\"}',1,'analicemos',1,'2026-02-13 13:45:32','2026-02-16 12:06:59'),(16,2,'text','{\"text\": \"Galletas en el plato: 🍪🍪🍪🍪🍪 (5 galletas)\", \"fontSize\": 16, \"color\": \"#FFFFFF\", \"isBulletPoint\": true}',2,'analicemos',1,'2026-02-13 13:45:32','2026-02-16 12:06:59'),(17,2,'visual_example','{\"objects\":[{\"imageUrl\":\"avares://Quibee/Assets/Images/Cookie.png\",\"count\":5,\"width\":42,\"height\":42},{\"symbol\":\"→\",\"fontSize\":64,\"color\":\"#FFFFFF\"},{\"imageUrl\":\"avares://Quibee/Assets/Images/5.png\",\"width\":78,\"height\":78}],\"layout\":\"horizontal\",\"spacing\":18,\"centerAlign\":true}',4,'analicemos',1,'2026-02-13 13:45:32','2026-02-16 12:06:59'),(18,2,'heading','{\"text\":\"Ejemplo 1\",\"level\":3,\"color\":\"#FFFFFF\"}',1,'practiquemos',1,'2026-02-13 13:45:32','2026-02-17 19:18:22'),(19,2,'text','{\"text\":\"Un niño tiene 6 globos rojos y 2 globos azules. ¿Cuántos globos tiene en total?\",\"fontSize\":16,\"color\":\"#FFFFFF\"}',2,'practiquemos',1,'2026-02-13 13:45:32','2026-02-17 19:18:22'),(21,2,'heading','{\"text\":\"Ejemplo 2\",\"level\":3,\"color\":\"#FFFFFF\"}',4,'practiquemos',1,'2026-02-13 13:45:32','2026-02-17 19:18:22'),(22,2,'text','{\"text\":\"En la mesa hay 4 libros de matemáticas y 3 libros de ciencia. ¿Cuántos libros hay en total?\",\"fontSize\":16,\"color\":\"#FFFFFF\"}',5,'practiquemos',1,'2026-02-13 13:45:32','2026-02-17 19:18:22'),(34,2,'text','{\"text\":\"En este ejemplo, el número 5 nos dice cuántas galletas hay en total.\",\"fontSize\":16,\"color\":\"#FFFFFF\"}',3,'analicemos',1,'2026-02-16 12:06:59','2026-02-16 12:06:59'),(35,2,'text','{\"text\":\"Ejercicio 1\",\"fontSize\":22,\"color\":\"#FFFFFF\",\"alignment\":\"left\"}',1,'resolvamos',1,'2026-02-19 12:41:03','2026-02-19 12:41:03'),(36,2,'text','{\"text\":\"En el jardín hay 5 mariposas y 3 abejas. ¿Cuántos insectos hay en total?\",\"fontSize\":18,\"color\":\"#FFFFFF\",\"alignment\":\"left\"}',2,'resolvamos',1,'2026-02-19 12:41:03','2026-02-19 12:41:03'),(38,2,'text','{\"text\":\"Ejercicio 2\",\"fontSize\":22,\"color\":\"#FFFFFF\",\"alignment\":\"left\"}',4,'resolvamos',1,'2026-02-19 12:41:03','2026-02-19 12:41:03'),(39,2,'text','{\"text\":\"Tienes 4 lápices en tu estuche y encuentras 2 más en tu mochila. ¿Cuántos lápices tienes en total?\",\"fontSize\":18,\"color\":\"#FFFFFF\",\"alignment\":\"left\"}',5,'resolvamos',1,'2026-02-19 12:41:03','2026-02-19 12:41:03'),(41,2,'text','{\"text\":\"Ejercicio 3\",\"fontSize\":22,\"color\":\"#FFFFFF\",\"alignment\":\"left\"}',7,'resolvamos',1,'2026-02-19 12:41:03','2026-02-19 12:41:03'),(42,2,'text','{\"text\":\"En un estanque hay 6 patos nadando y 2 patos en la orilla. ¿Cuántos patos hay en total?\",\"fontSize\":18,\"color\":\"#FFFFFF\",\"alignment\":\"left\"}',8,'resolvamos',1,'2026-02-19 12:41:03','2026-02-19 12:41:03'),(47,2,'heading','{\"text\": \"¡Desafío!\", \"level\": 1}',1,'desafio',1,'2026-02-20 12:34:02','2026-02-20 12:34:02'),(48,2,'text','{\"text\": \"Observa cada grupo de objetos y arrastra el número que representa la cantidad correcta.\", \"fontSize\": 18, \"color\": \"#CCCCCC\"}',2,'desafio',1,'2026-02-20 12:34:02','2026-02-20 12:34:02');
/*!40000 ALTER TABLE `LESSON_CONTENT` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `LEVEL`
--

DROP TABLE IF EXISTS `LEVEL`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `LEVEL` (
  `id_level` int(11) NOT NULL AUTO_INCREMENT,
  `level_number` int(11) NOT NULL,
  `level_name` varchar(100) NOT NULL,
  `description` varchar(500) DEFAULT NULL,
  `icon` varchar(100) DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_level`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `LEVEL`
--

LOCK TABLES `LEVEL` WRITE;
/*!40000 ALTER TABLE `LEVEL` DISABLE KEYS */;
INSERT INTO `LEVEL` VALUES (1,1,'Primer Nivel','Tus primeros pasos con los números',NULL,1,'2026-01-28 22:38:48.000000','2026-01-28 22:38:48.000000'),(4,1,'Primer Nivel','Tus primeros pasos con los números','avares://Quibee/Assets/Images/Grade1Icon.png',1,'2026-01-23 17:00:08.411646',NULL),(5,2,'Segundo Nivel','Nivel intermedio de matemáticas para segundo nivel','avares://Quibee/Assets/Images/Grade2Icon.png',1,'2026-01-23 17:00:08.458285',NULL),(6,3,'Tercer Nivel','Nivel avanzado de matemáticas para tercer nivel','avares://Quibee/Assets/Images/Grade3Icon.png',1,'2026-01-23 17:00:08.458291',NULL);
/*!40000 ALTER TABLE `LEVEL` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SESSION_LOG`
--

DROP TABLE IF EXISTS `SESSION_LOG`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SESSION_LOG` (
  `id_session_log` int(11) NOT NULL AUTO_INCREMENT,
  `id_student` int(11) NOT NULL,
  `session_start` datetime(6) NOT NULL,
  `session_end` datetime(6) DEFAULT NULL,
  `duration_minutes` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_session_log`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SESSION_LOG`
--

LOCK TABLES `SESSION_LOG` WRITE;
/*!40000 ALTER TABLE `SESSION_LOG` DISABLE KEYS */;
/*!40000 ALTER TABLE `SESSION_LOG` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `STUDENT`
--

DROP TABLE IF EXISTS `STUDENT`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `STUDENT` (
  `id_student` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `date_of_birth` datetime(6) NOT NULL,
  `level_number` int(11) NOT NULL,
  `gender` varchar(20) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `access_code` varchar(4) NOT NULL,
  `is_active` tinyint(1) NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_student`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `STUDENT`
--

LOCK TABLES `STUDENT` WRITE;
/*!40000 ALTER TABLE `STUDENT` DISABLE KEYS */;
INSERT INTO `STUDENT` VALUES (1,'fprueba748','Fernando','Prueba','2015-05-10 00:00:00.000000',3,'male',NULL,'1234',1,'2026-01-13 10:05:24.178495',NULL),(2,'fjovel663','Fernanda','Jovel','2000-07-05 00:00:00.000000',3,'female',NULL,'1234',1,'2026-01-13 10:15:02.330062',NULL),(3,'fjovel727','Fernanda','Jovel','2000-07-05 00:00:00.000000',3,'female',NULL,'1234',1,'2026-01-13 10:15:02.749004',NULL),(4,'fjovel501','Fernanda','Jovel','1999-07-05 00:00:00.000000',3,'female',NULL,'1234',1,'2026-01-15 10:34:14.985309',NULL),(5,'jmartinez887','Jairo','Martinez','2026-01-17 00:00:00.000000',2,'male',NULL,'1234',1,'2026-01-17 13:50:06.153029',NULL),(6,'jdelgado869','Julio','Delgado','2026-01-22 16:10:54.541035',1,'male',NULL,'1234',1,'2026-01-22 16:11:01.382163',NULL),(7,'fcruz161','Fernando','Cruz','2026-02-17 00:00:00.000000',1,'male',NULL,'1234',1,'2026-02-17 21:00:45.607819','2026-02-17 21:00:49.300936');
/*!40000 ALTER TABLE `STUDENT` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `STUDENT_ACHIEVEMENT`
--

DROP TABLE IF EXISTS `STUDENT_ACHIEVEMENT`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `STUDENT_ACHIEVEMENT` (
  `id_student_achievement` int(11) NOT NULL AUTO_INCREMENT,
  `id_student` int(11) NOT NULL,
  `id_achievement` int(11) NOT NULL,
  `earned_date` datetime(6) NOT NULL,
  PRIMARY KEY (`id_student_achievement`),
  KEY `IX_STUDENT_ACHIEVEMENT_id_student` (`id_student`),
  CONSTRAINT `FK_STUDENT_ACHIEVEMENT_STUDENT_id_student` FOREIGN KEY (`id_student`) REFERENCES `STUDENT` (`id_student`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `STUDENT_ACHIEVEMENT`
--

LOCK TABLES `STUDENT_ACHIEVEMENT` WRITE;
/*!40000 ALTER TABLE `STUDENT_ACHIEVEMENT` DISABLE KEYS */;
/*!40000 ALTER TABLE `STUDENT_ACHIEVEMENT` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `STUDENT_LESSON_PROGRESS`
--

DROP TABLE IF EXISTS `STUDENT_LESSON_PROGRESS`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `STUDENT_LESSON_PROGRESS` (
  `id_student_lesson_progress` int(11) NOT NULL AUTO_INCREMENT,
  `id_student` int(11) NOT NULL,
  `id_lesson` int(11) NOT NULL,
  `is_completed` tinyint(1) NOT NULL,
  `sections_visited` text DEFAULT NULL,
  `total_sections` int(11) DEFAULT 0,
  `progress_percentage` int(11) DEFAULT 0,
  `time_spent_minutes` int(11) DEFAULT NULL,
  `last_accessed` datetime(6) DEFAULT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_student_lesson_progress`),
  KEY `IX_STUDENT_LESSON_PROGRESS_id_student` (`id_student`),
  CONSTRAINT `FK_STUDENT_LESSON_PROGRESS_STUDENT_id_student` FOREIGN KEY (`id_student`) REFERENCES `STUDENT` (`id_student`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `STUDENT_LESSON_PROGRESS`
--

LOCK TABLES `STUDENT_LESSON_PROGRESS` WRITE;
/*!40000 ALTER TABLE `STUDENT_LESSON_PROGRESS` DISABLE KEYS */;
INSERT INTO `STUDENT_LESSON_PROGRESS` VALUES (1,7,2,1,'introduccion,analicemos,practiquemos,resolvamos,desafio',5,100,NULL,'2026-02-23 00:44:36.195551','2026-02-20 13:52:12.101570','2026-02-23 00:44:36.195557');
/*!40000 ALTER TABLE `STUDENT_LESSON_PROGRESS` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `STUDENT_LEVEL_PROGRESS`
--

DROP TABLE IF EXISTS `STUDENT_LEVEL_PROGRESS`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `STUDENT_LEVEL_PROGRESS` (
  `id_student_level_progress` int(11) NOT NULL AUTO_INCREMENT,
  `id_student` int(11) NOT NULL,
  `id_level` int(11) NOT NULL,
  `is_unlocked` tinyint(1) NOT NULL,
  `is_completed` tinyint(1) NOT NULL,
  `completion_percentage` decimal(65,30) DEFAULT NULL,
  `stars_earned` int(11) DEFAULT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_student_level_progress`),
  KEY `IX_STUDENT_LEVEL_PROGRESS_id_student` (`id_student`),
  CONSTRAINT `FK_STUDENT_LEVEL_PROGRESS_STUDENT_id_student` FOREIGN KEY (`id_student`) REFERENCES `STUDENT` (`id_student`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `STUDENT_LEVEL_PROGRESS`
--

LOCK TABLES `STUDENT_LEVEL_PROGRESS` WRITE;
/*!40000 ALTER TABLE `STUDENT_LEVEL_PROGRESS` DISABLE KEYS */;
/*!40000 ALTER TABLE `STUDENT_LEVEL_PROGRESS` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `STUDENT_STATS`
--

DROP TABLE IF EXISTS `STUDENT_STATS`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `STUDENT_STATS` (
  `id_student_stats` int(11) NOT NULL AUTO_INCREMENT,
  `id_student` int(11) NOT NULL,
  `total_points` int(11) NOT NULL,
  `lessons_completed` int(11) NOT NULL,
  `exercises_completed` int(11) NOT NULL,
  `current_streak` int(11) NOT NULL,
  `longest_streak` int(11) NOT NULL,
  `accuracy_percentage` decimal(65,30) DEFAULT NULL,
  `total_time_spent_minutes` int(11) DEFAULT NULL,
  `last_activity_date` datetime(6) DEFAULT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_student_stats`),
  UNIQUE KEY `IX_STUDENT_STATS_id_student` (`id_student`),
  CONSTRAINT `FK_STUDENT_STATS_STUDENT_id_student` FOREIGN KEY (`id_student`) REFERENCES `STUDENT` (`id_student`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `STUDENT_STATS`
--

LOCK TABLES `STUDENT_STATS` WRITE;
/*!40000 ALTER TABLE `STUDENT_STATS` DISABLE KEYS */;
INSERT INTO `STUDENT_STATS` VALUES (1,1,0,0,0,0,0,NULL,NULL,'2026-01-13 10:05:24.732393','2026-01-13 10:05:24.731960',NULL),(2,3,0,0,0,0,0,NULL,NULL,'2026-01-13 10:15:02.832035','2026-01-13 10:15:02.831514',NULL),(3,2,0,0,0,0,0,NULL,NULL,'2026-01-13 10:15:02.970273','2026-01-13 10:15:02.970264',NULL),(4,4,0,0,0,0,0,NULL,NULL,'2026-01-15 10:34:15.599232','2026-01-15 10:34:15.594479',NULL),(5,5,0,0,0,0,0,NULL,NULL,'2026-01-17 13:50:06.645236','2026-01-17 13:50:06.644769',NULL),(6,6,0,0,0,0,0,NULL,NULL,'2026-01-22 16:11:01.757218','2026-01-22 16:11:01.756653',NULL),(7,7,50,0,7,0,0,NULL,NULL,'2026-02-20 13:53:57.320046','2026-02-17 21:00:46.110178','2026-02-20 13:53:57.320051');
/*!40000 ALTER TABLE `STUDENT_STATS` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `TOPIC`
--

DROP TABLE IF EXISTS `TOPIC`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TOPIC` (
  `id_topic` int(11) NOT NULL AUTO_INCREMENT,
  `id_level` int(11) NOT NULL,
  `topic_name` varchar(100) NOT NULL,
  `description` varchar(500) DEFAULT NULL,
  `icon` varchar(100) DEFAULT NULL,
  `order_index` int(11) NOT NULL,
  `icon_width` double DEFAULT 100,
  `icon_height` double DEFAULT 100,
  `is_active` tinyint(1) NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id_topic`),
  KEY `IX_TOPIC_id_level` (`id_level`),
  CONSTRAINT `FK_TOPIC_LEVEL_id_level` FOREIGN KEY (`id_level`) REFERENCES `LEVEL` (`id_level`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=620 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `TOPIC`
--

LOCK TABLES `TOPIC` WRITE;
/*!40000 ALTER TABLE `TOPIC` DISABLE KEYS */;
INSERT INTO `TOPIC` VALUES (21,5,'Sumas y restas','Tema 1: Sumas y restas','avares://Quibee/Assets/Images/Calculator.png',1,80,80,1,'2026-01-23 17:00:10.013600',NULL),(22,5,'Pictogramas','Tema 2: Pictogramas','avares://Quibee/Assets/Images/WhiteRocket.png',2,100,100,1,'2026-01-23 17:00:10.013601',NULL),(23,5,'Suma horizontal y vertical','Tema 3: Suma horizontal y vertical','avares://Quibee/Assets/Images/SkyBlueAlien.png',3,100,100,1,'2026-01-23 17:00:10.013602',NULL),(24,5,'Multiplicación','Tema 4: Multiplicación','avares://Quibee/Assets/Images/Star2.png',4,90,90,1,'2026-01-23 17:00:10.013602',NULL),(25,5,'Longitud','Tema 5: Longitud','avares://Quibee/Assets/Images/Neptune.png',5,70,70,1,'2026-01-23 17:00:10.013603',NULL),(26,6,'Cuerpos geométricos','Tema 1: Cuerpos geométricos','avares://Quibee/Assets/Images/LilacPlanet.png',1,80,80,1,'2026-01-23 17:00:10.013603',NULL),(27,6,'Fracciones','Tema 2: Fracciones','avares://Quibee/Assets/Images/Star2.png',2,80,80,1,'2026-01-23 17:00:10.013604',NULL),(28,6,'Números decimales','Tema 3: Números decimales','avares://Quibee/Assets/Images/SquirrelRocket.png',3,100,100,1,'2026-01-23 17:00:10.013605',NULL),(29,6,'Operaciones combinadas','Tema 4: Operaciones combinadas','avares://Quibee/Assets/Images/AlienFullBody.png',4,100,100,1,'2026-01-23 17:00:10.013605',NULL),(30,6,'Medidas de capacidad','Tema 5: Medidas de capacidad','avares://Quibee/Assets/Images/GreenPlanet.png',5,70,70,1,'2026-01-23 17:00:10.013606',NULL),(601,1,'Conozcamos los números del 0 al 50','Números del 0 al 50','avares://Quibee/Assets/Images/SmallStar.png',1,80,80,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(602,1,'Relacionemos números y objetos','Números y objetos','avares://Quibee/Assets/Images/LilacPlanet2.png',2,100,100,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(603,1,'Comparemos cantidades','Comparemos cantidades','avares://Quibee/Assets/Images/Meteor.png',3,90,90,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(604,1,'Suma','Suma','avares://Quibee/Assets/Images/Earth2.png',4,100,100,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(605,1,'Resta','Resta','avares://Quibee/Assets/Images/Saturn2.png',5,100,100,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(606,1,'Números cardinales del 1 al 10','Cardinales 1 al 10','avares://Quibee/Assets/Images/3.png',6,95,95,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(607,1,'Partes curvas y planas en objetos','Curvas y planas','avares://Quibee/Assets/Images/Robot.png',7,85,85,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(608,1,'Agrupación de objetos por formas','Agrupación por formas','avares://Quibee/Assets/Images/SmallStar.png',8,90,90,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(609,1,'Líneas abiertas y cerradas','Líneas abiertas/cerradas','avares://Quibee/Assets/Images/Star2.png',9,85,85,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(610,1,'La recta numérica','La recta numérica','avares://Quibee/Assets/Images/LilacPlanet2.png',10,90,90,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(611,1,'Números del 10 al 50','Números 10 al 50','avares://Quibee/Assets/Images/Meteor.png',11,95,95,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(612,1,'Números del 51 al 100','Números 51 al 100','avares://Quibee/Assets/Images/Earth2.png',12,85,85,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(613,1,'Unidades y decenas','Unidades y decenas','avares://Quibee/Assets/Images/Saturn2.png',13,90,90,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(614,1,'El número 100','El número 100','avares://Quibee/Assets/Images/3.png',14,95,95,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(615,1,'La recta numérica hasta 100','Recta hasta 100','avares://Quibee/Assets/Images/Robot.png',15,85,85,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(616,1,'Comparación de Números hasta el 100','Comparación hasta 100','avares://Quibee/Assets/Images/SmallStar.png',16,90,90,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(617,1,'Comparación de longitudes','Comparación longitudes','avares://Quibee/Assets/Images/Neptune.png',17,85,85,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(618,1,'Medimos usando nuestro cuerpo','Medimos con el cuerpo','avares://Quibee/Assets/Images/LilacPlanet2.png',18,90,90,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000'),(619,1,'Lección de repaso','Lección de repaso','avares://Quibee/Assets/Images/Meteor.png',19,100,100,1,'2026-01-30 16:34:10.000000','2026-01-30 16:34:10.000000');
/*!40000 ALTER TABLE `TOPIC` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-01 14:31:44
