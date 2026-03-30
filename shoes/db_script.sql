CREATE DATABASE  IF NOT EXISTS `db_shoes` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `db_shoes`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: db_shoes
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `category_id` int NOT NULL AUTO_INCREMENT,
  `category_name` varchar(100) NOT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Женская обувь'),(2,'Мужская обувь');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manufactures`
--

DROP TABLE IF EXISTS `manufactures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufactures` (
  `manufacturer_id` int NOT NULL AUTO_INCREMENT,
  `manufacturer_name` varchar(100) NOT NULL,
  PRIMARY KEY (`manufacturer_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufactures`
--

LOCK TABLES `manufactures` WRITE;
/*!40000 ALTER TABLE `manufactures` DISABLE KEYS */;
INSERT INTO `manufactures` VALUES (1,'Alessio Nesca'),(2,'CROSBY'),(3,'Kari'),(4,'Marco Tozzi'),(5,'Rieker'),(6,'Рос');
/*!40000 ALTER TABLE `manufactures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `order_items`
--

DROP TABLE IF EXISTS `order_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `order_items` (
  `order_item_id` int NOT NULL AUTO_INCREMENT,
  `order_id` int NOT NULL,
  `tovar_article` varchar(6) NOT NULL,
  `tovar_quantity` int NOT NULL,
  PRIMARY KEY (`order_item_id`),
  KEY `tovar_article_fk1_idx` (`tovar_article`),
  KEY `order_id_fk1_idx` (`order_id`),
  CONSTRAINT `order_id_fk1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`order_id`),
  CONSTRAINT `tovar_article_fk1` FOREIGN KEY (`tovar_article`) REFERENCES `tovars` (`tovar_article`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `order_items`
--

LOCK TABLES `order_items` WRITE;
/*!40000 ALTER TABLE `order_items` DISABLE KEYS */;
INSERT INTO `order_items` VALUES (1,1,'А112Т4',2),(2,1,'F635R4',2),(3,2,'H782T5',1),(4,2,'G783F5',1),(5,3,'J384T6',10),(6,3,'D572U8',10),(7,4,'F572H7',5),(8,4,'D329H3',4),(9,5,'А112Т4',2),(10,5,'F635R4',2),(11,6,'H782T5',1),(12,6,'G783F5',1),(13,7,'J384T6',10),(14,7,'D572U8',10),(15,8,'F572H7',5),(16,8,'D329H3',4),(17,9,'B320R5',5),(18,9,'G432E4',1),(19,10,'S213E3',5),(20,10,'E482R4',5);
/*!40000 ALTER TABLE `order_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `orders` (
  `order_id` int NOT NULL AUTO_INCREMENT,
  `order_date` date NOT NULL,
  `delivery_date` date NOT NULL,
  `pickup_point_id` int NOT NULL,
  `client_id` int NOT NULL,
  `code_to_receive` varchar(10) NOT NULL,
  `order_status` enum('Новый','Завершен') NOT NULL DEFAULT 'Новый',
  PRIMARY KEY (`order_id`),
  KEY `client_id_fk_idx` (`client_id`),
  KEY `pickup_point_id_fk_idx` (`pickup_point_id`),
  CONSTRAINT `client_id_fk` FOREIGN KEY (`client_id`) REFERENCES `users` (`user_id`),
  CONSTRAINT `pickup_point_id_fk` FOREIGN KEY (`pickup_point_id`) REFERENCES `pickup_points` (`pickup_point_id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,'2025-02-27','2025-04-20',1,10,'901','Завершен'),(2,'2022-09-28','2025-04-21',11,5,'902','Завершен'),(3,'2025-03-21','2025-04-22',2,7,'903','Завершен'),(4,'2025-02-20','2025-04-23',11,6,'904','Завершен'),(5,'2025-03-17','2025-04-24',2,10,'905','Завершен'),(6,'2025-03-01','2025-04-25',15,5,'906','Завершен'),(7,'2025-03-30','2025-04-26',3,7,'907','Завершен'),(8,'2025-03-31','2025-04-27',28,6,'908','Новый'),(9,'2025-04-02','2025-04-28',5,10,'909','Новый'),(10,'2025-04-03','2025-04-29',19,10,'910','Новый'),(11,'2026-03-11','2026-03-13',10,5,'243','Завершен'),(12,'2026-03-23','2026-04-02',5,5,'717','Завершен'),(13,'2026-03-27','2026-03-28',6,5,'005','Новый'),(14,'2026-03-27','2026-04-02',5,5,'468','Новый');
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pickup_points`
--

DROP TABLE IF EXISTS `pickup_points`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pickup_points` (
  `pickup_point_id` int NOT NULL AUTO_INCREMENT,
  `pickup_point_index` varchar(6) NOT NULL,
  `pickup_point_city` varchar(45) NOT NULL,
  `pickup_point_street` varchar(45) NOT NULL,
  `pickup_point_house` varchar(45) NOT NULL,
  PRIMARY KEY (`pickup_point_id`)
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pickup_points`
--

LOCK TABLES `pickup_points` WRITE;
/*!40000 ALTER TABLE `pickup_points` DISABLE KEYS */;
INSERT INTO `pickup_points` VALUES (1,'420151','Лесной','Вишневая','32'),(2,'125061','Лесной','Подгорная','8'),(3,'630370','Лесной','Шоссейная','24'),(4,'400562','Лесной','Зеленая','32'),(5,'614510','Лесной','Маяковского','47'),(6,'410542','Лесной','Светлая','46'),(7,'620839','Лесной','Цветочная','8'),(8,'443890','Лесной','Коммунистическая','1'),(9,'603379','Лесной','Спортивная','46'),(10,'603721','Лесной','Гоголя','41'),(11,'410172','Лесной','Северная','13'),(12,'614611','Лесной','Молодежная','50'),(13,'454311','Лесной','Новая','19'),(14,'660007','Лесной','Октябрьская','19'),(15,'603036','Лесной','Садовая','4'),(16,'394060','Лесной','Фрунзе','43'),(17,'410661','Лесной','Школьная','50'),(18,'625590','Лесной','Коммунистическая','20'),(19,'625683','Лесной','8 Марта','5'),(20,'450983','Лесной','Комсомольская','26'),(21,'394782','Лесной','Чехова','3'),(22,'603002','Лесной','Дзержинского','28'),(23,'450558','Лесной','Набережная','30'),(24,'344288','Лесной','Чехова','1'),(25,'614164','Лесной',' Степная','30'),(26,'394242','Лесной','Коммунистическая','43'),(27,'660540','Лесной','Солнечная','25'),(28,'125837','Лесной','Шоссейная','40'),(29,'125703','Лесной','Партизанская','49'),(30,'625283','Лесной','Победы','46'),(31,'614753','Лесной','Полевая','35'),(32,'426030','Лесной','Маяковского','44'),(33,'450375','Лесной','Клубная','44'),(34,'625560','Лесной','Некрасова','12'),(35,'630201','Лесной','Комсомольская','17'),(36,'190949','Лесной','Мичурина','26');
/*!40000 ALTER TABLE `pickup_points` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `suppliers`
--

DROP TABLE IF EXISTS `suppliers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `suppliers` (
  `supplier_id` int NOT NULL AUTO_INCREMENT,
  `supplier_name` varchar(100) NOT NULL,
  PRIMARY KEY (`supplier_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `suppliers`
--

LOCK TABLES `suppliers` WRITE;
/*!40000 ALTER TABLE `suppliers` DISABLE KEYS */;
INSERT INTO `suppliers` VALUES (1,'Kari'),(2,'Обувь для вас');
/*!40000 ALTER TABLE `suppliers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tovars`
--

DROP TABLE IF EXISTS `tovars`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tovars` (
  `tovar_article` varchar(6) NOT NULL,
  `tovar_name` varchar(255) NOT NULL,
  `tovar_price` decimal(8,2) NOT NULL,
  `unit_of_measurement` varchar(45) DEFAULT NULL,
  `supplier_id` int NOT NULL,
  `manufacturer_id` int NOT NULL,
  `category_id` int NOT NULL,
  `current_discount` decimal(8,2) NOT NULL,
  `quantity_in_stock` int NOT NULL,
  `tovar_description` text NOT NULL,
  `photo` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`tovar_article`),
  KEY `category_id_fk2_idx` (`category_id`),
  KEY `manufacturer_id_fk2_idx` (`manufacturer_id`),
  KEY `supplier_id_fk2_idx` (`supplier_id`),
  CONSTRAINT `category_id_fk2` FOREIGN KEY (`category_id`) REFERENCES `categories` (`category_id`),
  CONSTRAINT `manufacturer_id_fk2` FOREIGN KEY (`manufacturer_id`) REFERENCES `manufactures` (`manufacturer_id`),
  CONSTRAINT `supplier_id_fk2` FOREIGN KEY (`supplier_id`) REFERENCES `suppliers` (`supplier_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tovars`
--

LOCK TABLES `tovars` WRITE;
/*!40000 ALTER TABLE `tovars` DISABLE KEYS */;
INSERT INTO `tovars` VALUES ('B320R5','Туфли',4300.00,'шт.',1,5,1,2.00,6,'Туфли Rieker женские демисезонные, размер 41, цвет коричневый','9.jpg'),('B431R5','Ботинки',2700.00,'шт.',2,5,2,2.00,5,'Мужские кожаные ботинки/мужские ботинки',''),('C436G5','Ботинки',10200.00,'шт.',1,1,1,15.00,9,'Ботинки женские, ARGO, размер 40',''),('D268G5','Туфли',4399.00,'шт.',2,5,1,3.00,12,'Туфли Rieker женские демисезонные, размер 36, цвет коричневый','edited_photo.png'),('D329H3','Полуботинки',1890.00,'шт.',1,4,1,8.00,4,'Полуботинки Alessio Nesca женские 3-30797-47, размер 37, цвет: бордовый','8.jpg'),('D364R4','Туфли',12400.00,'шт.',1,3,1,16.00,5,'Туфли Luiza Belly женские Kate-lazo черные из натуральной замши',''),('D572U8','Кроссовки',4100.00,'шт.',2,6,2,3.00,6,'129615-4 Кроссовки мужские','6.jpg'),('E482R4','Полуботинки',1800.00,'шт.',1,3,1,2.00,14,'Полуботинки kari женские MYZ20S-149, размер 41, цвет: черный',''),('F427R5','Ботинки',11800.00,'шт.',2,5,1,15.00,11,'Ботинки на молнии с декоративной пряжкой FRAU','edited3.jpg'),('F572H7','Туфли',2700.00,'шт.',1,4,1,2.00,14,'Туфли Marco Tozzi женские летние, размер 39, цвет черный','7.jpg'),('F635R4','Ботинки',3244.00,'шт.',2,4,1,2.00,13,'Ботинки Marco Tozzi женские демисезонные, размер 39, цвет бежевый','2.jpg'),('G432E4','Туфли',2800.00,'шт.',1,3,1,3.00,15,'Туфли kari женские TR-YR-413017, размер 37, цвет: черный','10.jpg'),('G531F4','Ботинки',6600.00,'шт.',1,3,1,12.00,9,'Ботинки женские зимние ROMER арт. 893167-01 Черный',''),('G783F5','Ботинки',5900.00,'шт.',1,6,2,2.00,8,'Мужские ботинки Рос-Обувь кожаные с натуральным мехом','4.jpg'),('H535R5','Ботинки',2300.00,'шт.',2,5,1,2.00,7,'Женские Ботинки демисезонные',''),('H782T5','Туфли',4499.00,'шт.',1,3,2,4.00,5,'Туфли kari мужские классика MYZ21AW-450A, размер 43, цвет: черный','3.jpg'),('J384T6','Ботинки',3800.00,'шт.',2,5,2,2.00,16,'B3430/14 Полуботинки мужские Rieker','5.jpg'),('J542F5','Тапочки',500.00,'шт.',1,3,2,13.00,0,'Тапочки мужские Арт.70701-55-67син р.41',''),('K345R4','Полуботинки',2100.00,'шт.',2,2,2,2.00,3,'407700/01-02 Полуботинки мужские CROSBY',''),('K358H6','Тапочки',599.00,'шт.',1,5,2,20.00,2,'Тапочки мужские син р.41',''),('L754R4','Полуботинки',1700.00,'шт.',1,3,1,2.00,7,'Полуботинки kari женские WB2020SS-26, размер 38, цвет: черный',''),('M542T5','Кроссовки',2800.00,'шт.',2,5,2,18.00,3,'Кроссовки мужские TOFA',''),('N457T5','Полуботинки',4600.00,'шт.',1,2,1,3.00,13,'Полуботинки Ботинки черные зимние, мех',''),('NS85TR','ваиви',1.00,'шт.',2,3,2,99.00,1,'вамамиа',NULL),('O754F4','Туфли',5400.00,'шт.',2,5,1,4.00,18,'Туфли женские демисезонные Rieker артикул 55073-68/37',''),('P764G4','Туфли',6800.00,'шт.',1,2,1,15.00,15,'Туфли женские, ARGO, размер 38',''),('S213E3','Полуботинки',2156.00,'шт.',2,2,2,3.00,6,'407700/01-01 Полуботинки мужские CROSBY',''),('S326R5','Тапочки',9900.00,'шт.',2,2,2,17.00,15,'Мужские кожаные тапочки \"Профиль С.Дали\" ',''),('S634B5','Кеды',5500.00,'шт.',2,2,2,3.00,0,'Кеды Caprice мужские демисезонные, размер 42, цвет черный',''),('T324F5','Сапоги',4699.00,'шт.',1,2,1,2.00,5,'Сапоги замша Цвет: синий',''),('U8YZDK','новый товар',1500.00,'шт.',1,2,1,12.00,5,'описание нового товара',NULL),('А112Т4','Ботинки',4990.00,'шт.',1,3,1,3.00,6,'Женские Ботинки демисезонные kari','1.jpg');
/*!40000 ALTER TABLE `tovars` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `user_role` enum('Авторизированный клиент','Администратор','Менеджер') NOT NULL DEFAULT 'Авторизированный клиент',
  `user_surname` varchar(100) NOT NULL,
  `user_firstname` varchar(100) NOT NULL,
  `user_lastname` varchar(100) NOT NULL,
  `user_email` varchar(255) NOT NULL,
  `user_password` varchar(255) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Авторизированный клиент','Ворсин','Петр','Евгеньевич','1qz4kw@mail.com','gynQMT'),(2,'Авторизированный клиент','Михайлюк','Анна','Вячеславовна','5d4zbu@tutanota.com','rwVDh9'),(3,'Авторизированный клиент','Ситдикова','Елена','Анатольевна','ptec8ym@yahoo.com','LdNyos'),(4,'Авторизированный клиент','Старикова','Елена','Павловна','4np6se@mail.com','AtnDjr'),(5,'Администратор','Никифорова','Весения','Николаевна','94d5ous@gmail.com','uzWC67'),(6,'Администратор','Одинцов','Серафим','Артёмович','yzls62@outlook.com','JlFRCZ'),(7,'Администратор','Сазонов','Руслан','Германович','uth4iz@mail.com','2L6KZG'),(8,'Менеджер','Ворсин','Петр','Евгеньевич','tjde7c@yahoo.com','YOyhfR'),(9,'Менеджер','Старикова','Елена','Павловна','wpmrc3do@tutanota.com','RSbvHv'),(10,'Менеджер','Степанов','Михаил','Артёмович','1diph5e@tutanota.com','8ntwUp');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-30 19:04:31
