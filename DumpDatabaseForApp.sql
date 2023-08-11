CREATE DATABASE  IF NOT EXISTS `mydb` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `mydb`;
-- MySQL dump 10.13  Distrib 8.0.32, for Win64 (x86_64)
--
-- Host: localhost    Database: mydb
-- ------------------------------------------------------
-- Server version	8.0.32

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
-- Table structure for table `chief_engineer`
--

DROP TABLE IF EXISTS `chief_engineer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `chief_engineer` (
  `emp_login` char(18) NOT NULL,
  `chief_eng_email` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`emp_login`),
  UNIQUE KEY `chief_eng_email_UNIQUE` (`chief_eng_email`),
  CONSTRAINT `R_79` FOREIGN KEY (`emp_login`) REFERENCES `employee` (`emp_login`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chief_engineer`
--

LOCK TABLES `chief_engineer` WRITE;
/*!40000 ALTER TABLE `chief_engineer` DISABLE KEYS */;
INSERT INTO `chief_engineer` VALUES ('ivanov_log',NULL);
/*!40000 ALTER TABLE `chief_engineer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `decommissioned_spare_parts`
--

DROP TABLE IF EXISTS `decommissioned_spare_parts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `decommissioned_spare_parts` (
  `log_ID` int unsigned NOT NULL,
  `decommissioned_sp_quantity` int unsigned DEFAULT NULL,
  `sp_inventory_number` varchar(20) NOT NULL,
  PRIMARY KEY (`log_ID`,`sp_inventory_number`),
  KEY `include_in_log` (`sp_inventory_number`),
  CONSTRAINT `contains_sp` FOREIGN KEY (`log_ID`) REFERENCES `maintenance_log_entry` (`log_ID`),
  CONSTRAINT `include_in_log` FOREIGN KEY (`sp_inventory_number`) REFERENCES `spare_part` (`sp_inventory_number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `decommissioned_spare_parts`
--

LOCK TABLES `decommissioned_spare_parts` WRITE;
/*!40000 ALTER TABLE `decommissioned_spare_parts` DISABLE KEYS */;
INSERT INTO `decommissioned_spare_parts` VALUES (40,1,'1242231'),(41,1,'1253334'),(41,20,'1253390'),(42,1,'124232'),(42,1,'21312'),(43,1,'1253543'),(43,100,'1253991'),(44,12,'12345212'),(44,100,'1253334');
/*!40000 ALTER TABLE `decommissioned_spare_parts` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `writeOffSP_trigger` AFTER INSERT ON `decommissioned_spare_parts` FOR EACH ROW BEGIN
		update mydb.spare_part set sp_stock_quantity = sp_stock_quantity - new.decommissioned_sp_quantity where  sp_inventory_number = new.sp_inventory_number;
  END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS `employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employee` (
  `emp_name` varchar(18) NOT NULL,
  `emp_surname` varchar(18) NOT NULL,
  `emp_patronymic` varchar(18) DEFAULT NULL,
  `emp_login` varchar(18) NOT NULL,
  `emp_password` char(18) NOT NULL,
  `position_ID` int NOT NULL,
  PRIMARY KEY (`emp_login`),
  UNIQUE KEY `emp_login_UNIQUE` (`emp_login`),
  KEY `R_22` (`position_ID`),
  CONSTRAINT `R_22` FOREIGN KEY (`position_ID`) REFERENCES `position` (`position_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee`
--

LOCK TABLES `employee` WRITE;
/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
INSERT INTO `employee` VALUES ('Баскаков','Павел','Вячеславович','baskakov_log','baskakov_pass',3),('Горюнов','Николай','Васильевич','gorunov_log','gorunov_pass',1),('Иванов','Иван','Иванович','ivanov_log','ivanov_pass',2),('Кузнецов','Сергей','Константинович','kuznecoc_log','kuznecoc_pass',1),('Петров','Петр','Петрович','petrov_log','petrov_pass',3),('Сидоров','Иван','Петрович','sidorov_log','sidorov_pass',1);
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `add_empl_trigger` AFTER INSERT ON `employee` FOR EACH ROW BEGIN
		if new.position_ID = 1 then insert into technician values (new.emp_login);
		elseif new.position_ID = 2 then insert into chief_engineer (emp_login) values (new.emp_login);
		elseif new.position_ID = 3 then insert into spare_part_specialist (emp_login) values (new.emp_login);
		end if;
  END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `equipment`
--

DROP TABLE IF EXISTS `equipment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `equipment` (
  `equip_name` varchar(50) NOT NULL,
  `equip_brand_type` varchar(18) DEFAULT NULL COMMENT 'Наименование бренда/производителя оборудования',
  `equip_serial_number` varchar(18) NOT NULL,
  `equip_installation_location` char(18) NOT NULL,
  `emp_login` char(18) NOT NULL,
  `status_ID` int NOT NULL,
  `equip_inventory_number` int NOT NULL,
  PRIMARY KEY (`equip_inventory_number`),
  UNIQUE KEY `unique_serialNumber_and_Brand` (`equip_brand_type`,`equip_serial_number`),
  KEY `R_21` (`status_ID`),
  KEY `R_52` (`emp_login`),
  CONSTRAINT `R_21` FOREIGN KEY (`status_ID`) REFERENCES `status` (`status_ID`),
  CONSTRAINT `R_52` FOREIGN KEY (`emp_login`) REFERENCES `technician` (`emp_login`),
  CONSTRAINT `CHK_equip_invent_numb` CHECK ((`equip_inventory_number` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `equipment`
--

LOCK TABLES `equipment` WRITE;
/*!40000 ALTER TABLE `equipment` DISABLE KEYS */;
INSERT INTO `equipment` VALUES ('Металлодетектор №1','Mesutronic','1C43A45','29','sidorov_log',2,123124),('Котёл пищеварочный','Abat','1234','123127','gorunov_log',1,123127),('Трейсилер Proseal №1','Proseal','11-234-F3','123140','sidorov_log',1,123140),('Трейсилер Mondini','G. Mondini','27-T3-78-00','27','gorunov_log',2,123150),('Палетообмотчик ROTOPLAT 708 PVS','ROBOPAC','123-123','10','gorunov_log',3,123160),('Волчок электричекий Zeydelman','Zeydelmann','1E3212','432342','sidorov_log',1,432342),('Камера шоковой заморозки IRINOX SD-10','IRINOX','1274-1','1234223','kuznecoc_log',1,1234223),('Пароконвектомат Rational SCC WE','Rational','123-32-12C','23','kuznecoc_log',2,12343321),('Соусный дозатор Пастпак-Р','Патспак','123321-1212-С1212','34','sidorov_log',2,123432313);
/*!40000 ALTER TABLE `equipment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `list_decommiss_sp`
--

DROP TABLE IF EXISTS `list_decommiss_sp`;
/*!50001 DROP VIEW IF EXISTS `list_decommiss_sp`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `list_decommiss_sp` AS SELECT 
 1 AS `Взятое количество`,
 1 AS `Наименование запчасти`,
 1 AS `Инвентарный номер`,
 1 AS `Взявший сотрудник`,
 1 AS `Дата изъятия`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `list_maintenance`
--

DROP TABLE IF EXISTS `list_maintenance`;
/*!50001 DROP VIEW IF EXISTS `list_maintenance`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `list_maintenance` AS SELECT 
 1 AS `Начало проведения работ`,
 1 AS `Конец проведения работ`,
 1 AS `Тип обслуживания`,
 1 AS `Описание работ`,
 1 AS `Исполнитель работ`,
 1 AS `Оборудования`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `list_of_equipment`
--

DROP TABLE IF EXISTS `list_of_equipment`;
/*!50001 DROP VIEW IF EXISTS `list_of_equipment`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `list_of_equipment` AS SELECT 
 1 AS `Название оборудования`,
 1 AS `Производитель оборудования`,
 1 AS `Серийный номер`,
 1 AS `Инвентарный номер`,
 1 AS `Номер помещения`,
 1 AS `Статус`,
 1 AS `Ответственный`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `list_sp`
--

DROP TABLE IF EXISTS `list_sp`;
/*!50001 DROP VIEW IF EXISTS `list_sp`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `list_sp` AS SELECT 
 1 AS `Наименование`,
 1 AS `Артикульный номер`,
 1 AS `Инвентарный номер`,
 1 AS `Кол-во на складе`,
 1 AS `Заказано`,
 1 AS `Мин. кол-во`,
 1 AS `Важность по ABC`,
 1 AS `Оборудование`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `maintenance_log_entry`
--

DROP TABLE IF EXISTS `maintenance_log_entry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `maintenance_log_entry` (
  `log_ID` int unsigned NOT NULL AUTO_INCREMENT,
  `log_start_repair_time` datetime NOT NULL,
  `log_stop_repair_time` datetime NOT NULL,
  `log_type_of_maintenance` varchar(50) NOT NULL,
  `log_job_description` text NOT NULL,
  `log_contractor` char(18) DEFAULT NULL,
  `equip_inventory_number` int DEFAULT NULL,
  PRIMARY KEY (`log_ID`),
  KEY `R_15` (`log_contractor`),
  KEY `R_58` (`equip_inventory_number`),
  CONSTRAINT `R_15` FOREIGN KEY (`log_contractor`) REFERENCES `technician` (`emp_login`) ON DELETE SET NULL,
  CONSTRAINT `R_58` FOREIGN KEY (`equip_inventory_number`) REFERENCES `equipment` (`equip_inventory_number`),
  CONSTRAINT `CHK_maintenance_time` CHECK ((`log_start_repair_time` < `log_stop_repair_time`))
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `maintenance_log_entry`
--

LOCK TABLES `maintenance_log_entry` WRITE;
/*!40000 ALTER TABLE `maintenance_log_entry` DISABLE KEYS */;
INSERT INTO `maintenance_log_entry` VALUES (34,'2023-04-14 00:18:00','2023-04-14 01:18:00','техническое обслуживание','Чистка пароводяной рубашки.','gorunov_log',123127),(35,'2023-03-30 14:32:00','2023-03-31 12:00:00','техническое обслуживание','Проверка редуктора, смазка цепи','kuznecoc_log',432342),(36,'2023-03-09 12:05:00','2023-03-09 14:05:00','техническое обслуживание','Чистка матрицы во время остановки оборудования, замена винтов смазка направляющих','kuznecoc_log',123140),(37,'2023-02-10 01:06:00','2023-02-10 02:06:00','текущий ремонт','Замена дисплея вследствие повреждения его сотрудниками цеха во время транспортировки оборудования','kuznecoc_log',123124),(38,'2023-04-05 16:07:00','2023-04-05 17:08:00','техническое обслуживание','Чистка пароводяной рубашки.','kuznecoc_log',123127),(39,'2023-02-01 14:40:00','2023-02-01 19:41:00','техническое обслуживание','Тарировка датчиков температуры в камере, восстановление заезда в камеру','gorunov_log',1234223),(40,'2023-01-07 18:41:00','2023-01-07 19:20:00','техническое обслуживание','Произведена замена верхнего вентилятора, отрегулирован частотный регулятор','sidorov_log',1234223),(41,'2022-12-15 18:45:00','2022-12-15 20:00:00','текущий ремонт','Заменён индуктивный датчик положения штампа из-за износа провода, заменено уплотнение матрицы ','sidorov_log',123150),(42,'2023-01-04 18:00:00','2023-01-04 18:10:00','техническое обслуживание','Заменены режущие решётки в комплекте по причине износа','gorunov_log',432342),(43,'2023-02-10 21:08:00','2023-02-18 21:08:00','текущий ремонт','wrgwefwdvwrgerg','gorunov_log',123140),(44,'2023-04-12 21:42:00','2023-04-12 23:42:00','техническое обслуживание','erfgergergwg','gorunov_log',123150);
/*!40000 ALTER TABLE `maintenance_log_entry` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `position`
--

DROP TABLE IF EXISTS `position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `position` (
  `position_ID` int NOT NULL AUTO_INCREMENT,
  `position_name` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`position_ID`),
  UNIQUE KEY `position_ID_UNIQUE` (`position_ID`),
  UNIQUE KEY `position_name_UNIQUE` (`position_name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `position`
--

LOCK TABLES `position` WRITE;
/*!40000 ALTER TABLE `position` DISABLE KEYS */;
INSERT INTO `position` VALUES (2,'главный инженер'),(3,'специалист по запчастям'),(1,'техник');
/*!40000 ALTER TABLE `position` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `report_for_order_sp`
--

DROP TABLE IF EXISTS `report_for_order_sp`;
/*!50001 DROP VIEW IF EXISTS `report_for_order_sp`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `report_for_order_sp` AS SELECT 
 1 AS `Название`,
 1 AS `Для оборудования`,
 1 AS `Артикульный номер`,
 1 AS `Кол-во к заказу`,
 1 AS `Остаток на складе`,
 1 AS `Заказано`,
 1 AS `Определённый минимум`,
 1 AS `Важность по abc`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `spare_part`
--

DROP TABLE IF EXISTS `spare_part`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `spare_part` (
  `sp_name` varchar(40) NOT NULL,
  `sp_article_number` varchar(20) DEFAULT NULL,
  `sp_inventory_number` varchar(20) NOT NULL,
  `sp_stock_quantity` int NOT NULL,
  `sp_ordered_quantity` int NOT NULL,
  `sp_min_quantity` int NOT NULL,
  `sp_abc_importance` char(1) NOT NULL,
  `sp_emp_added_login` char(18) DEFAULT NULL,
  `equip_inventory_number` int DEFAULT NULL,
  PRIMARY KEY (`sp_inventory_number`),
  KEY `R_32` (`equip_inventory_number`),
  KEY `R_33` (`sp_emp_added_login`),
  CONSTRAINT `R_32` FOREIGN KEY (`equip_inventory_number`) REFERENCES `equipment` (`equip_inventory_number`),
  CONSTRAINT `R_33` FOREIGN KEY (`sp_emp_added_login`) REFERENCES `spare_part_specialist` (`emp_login`) ON DELETE SET NULL,
  CONSTRAINT `CHK_min_quant` CHECK ((`sp_min_quantity` > 0)),
  CONSTRAINT `CHK_sp_invent_numb` CHECK ((`sp_inventory_number` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `spare_part`
--

LOCK TABLES `spare_part` WRITE;
/*!40000 ALTER TABLE `spare_part` DISABLE KEYS */;
INSERT INTO `spare_part` VALUES ('Шестерня подающего транспортёра','123-123-12442','12345212',15,12,12,'B',NULL,123150),('Плата управления насосами','123-1213-212','12412',2,2,3,'A','baskakov_log',12343321),('Верхний вентилятор','23-41-234','1242231',1,0,1,'B','baskakov_log',1234223),('Решетка д5','3213-11','124232',3,2,5,'C','baskakov_log',432342),('Датчик индуктивный','1232458','1253334',97,0,2,'B','petrov_log',123150),('Шестерня','123445','1253355',198,0,3,'A','petrov_log',123150),('Уплотнение Т-образное','1232323','1253390',178,0,2,'C','petrov_log',123150),('Емкостной датчик крышки','1235','1253543',4,5,5,'B','petrov_log',123140),('Плата дисплея','1232460','1253589',195,5,1,'А','petrov_log',123124),('Дисплей','1232450','1253749',199,0,1,'А','petrov_log',123124),('Винт М5 матрицы','123','1253991',98,0,20,'C','petrov_log',123140),('Датчик температуры ','128232','126434',5,0,1,'B','baskakov_log',123127),('Главная плата управления ','12131-211','142311',2,1,3,'A','baskakov_log',12343321),('Решётка приемная','321134-121','21312',0,2,4,'C','baskakov_log',432342),('Геркон пневмоцилиндра','324421','432244',8,2,15,'C','baskakov_log',123432313),('Паяльные губки','12113','44223',3,10,15,'B','baskakov_log',123432313);
/*!40000 ALTER TABLE `spare_part` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `spare_part_specialist`
--

DROP TABLE IF EXISTS `spare_part_specialist`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `spare_part_specialist` (
  `emp_login` char(18) NOT NULL,
  `sp_spec_email` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`emp_login`),
  UNIQUE KEY `sp_spec_email_UNIQUE` (`sp_spec_email`),
  CONSTRAINT `R_101` FOREIGN KEY (`emp_login`) REFERENCES `employee` (`emp_login`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `spare_part_specialist`
--

LOCK TABLES `spare_part_specialist` WRITE;
/*!40000 ALTER TABLE `spare_part_specialist` DISABLE KEYS */;
INSERT INTO `spare_part_specialist` VALUES ('baskakov_log',NULL),('petrov_log',NULL);
/*!40000 ALTER TABLE `spare_part_specialist` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `status`
--

DROP TABLE IF EXISTS `status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `status` (
  `status_ID` int NOT NULL AUTO_INCREMENT,
  `status_description` varchar(20) NOT NULL,
  PRIMARY KEY (`status_ID`),
  UNIQUE KEY `status_ID_UNIQUE` (`status_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `status`
--

LOCK TABLES `status` WRITE;
/*!40000 ALTER TABLE `status` DISABLE KEYS */;
INSERT INTO `status` VALUES (1,'В эксплуатации'),(2,'В ремонте'),(3,'В резерве');
/*!40000 ALTER TABLE `status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `technician`
--

DROP TABLE IF EXISTS `technician`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `technician` (
  `emp_login` char(18) NOT NULL,
  PRIMARY KEY (`emp_login`),
  CONSTRAINT `R_99` FOREIGN KEY (`emp_login`) REFERENCES `employee` (`emp_login`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `technician`
--

LOCK TABLES `technician` WRITE;
/*!40000 ALTER TABLE `technician` DISABLE KEYS */;
INSERT INTO `technician` VALUES ('gorunov_log'),('kuznecoc_log'),('sidorov_log');
/*!40000 ALTER TABLE `technician` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'mydb'
--

--
-- Dumping routines for database 'mydb'
--
/*!50003 DROP PROCEDURE IF EXISTS `check_person` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `check_person`(
  IN login VARCHAR(50),
  IN pass VARCHAR(50),
  OUT position VARCHAR(50))
    COMMENT 'check if exist user in database'
BEGIN
	SELECT position_ID into position FROM employee WHERE emp_login = login AND emp_password = pass ;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `list_sp_for_equipment` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `list_sp_for_equipment`(IN nameOfEquip varchar(50))
begin

select sp_name, sp_article_number, sp_stock_quantity, equipment.equip_name 
from spare_part 
join equipment 
on spare_part.equip_inventory_number = equipment.equip_inventory_number 
where equip_name = nameOfEquip;
end ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `maintenance_time_report_for_unit` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `maintenance_time_report_for_unit`(
IN invent_numb int, 
IN start_time datetime, 
IN stop_time datetime)
BEGIN
			select
		date_format(log_start_repair_time, '%d %b %Y %h:%m') as 'Начало проведения работ', 
		date_format(log_stop_repair_time, '%d %b %Y %h:%m') as 'Конец проведения работ',
		maintenance_log_entry.log_type_of_maintenance as 'Тип обслуживания', 
		maintenance_log_entry.log_job_description as 'Описание работ', 
		employee.emp_name as 'Исполнитель работ',
		equipment.equip_name as 'Оборудования'
		from maintenance_log_entry 
		join equipment 
		on equipment.equip_inventory_number = maintenance_log_entry.equip_inventory_number
		join employee 
		on employee.emp_login = maintenance_log_entry.log_contractor
		where (equipment.equip_inventory_number = invent_numb
			   and maintenance_log_entry.log_start_repair_time > start_time
			   and maintenance_log_entry.log_stop_repair_time <  stop_time);
    	END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_equip_info` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_equip_info`(
	IN invent_numb int, IN install_location int, IN respons_emp char(18), IN equip_status int)
BEGIN
		update equipment 
        SET equipment.equip_installation_location = install_location,
			equipment.emp_login = respons_emp,
			equipment.status_ID = equip_status
		where equip_inventory_number = invent_numb;
    END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `update_sp_info` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_sp_info`(
	IN invent_numb VARCHAR(20), IN curr_stock int, IN curr_order int, IN curr_min int)
BEGIN
		update spare_part 
        SET spare_part.sp_stock_quantity = curr_stock,
			spare_part.sp_ordered_quantity = curr_order,
            spare_part.sp_min_quantity = curr_min
		where sp_inventory_number = invent_numb;
    END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `list_decommiss_sp`
--

/*!50001 DROP VIEW IF EXISTS `list_decommiss_sp`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `list_decommiss_sp` AS select `decommissioned_spare_parts`.`decommissioned_sp_quantity` AS `Взятое количество`,`spare_part`.`sp_name` AS `Наименование запчасти`,`spare_part`.`sp_inventory_number` AS `Инвентарный номер`,`employee`.`emp_name` AS `Взявший сотрудник`,date_format(`maintenance_log_entry`.`log_stop_repair_time`,'%d %b %Y') AS `Дата изъятия` from (((`decommissioned_spare_parts` join `maintenance_log_entry` on((`decommissioned_spare_parts`.`log_ID` = `maintenance_log_entry`.`log_ID`))) join `spare_part` on((`decommissioned_spare_parts`.`sp_inventory_number` = `spare_part`.`sp_inventory_number`))) join `employee` on((`maintenance_log_entry`.`log_contractor` = `employee`.`emp_login`))) order by `maintenance_log_entry`.`log_stop_repair_time` desc limit 50 */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `list_maintenance`
--

/*!50001 DROP VIEW IF EXISTS `list_maintenance`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `list_maintenance` AS select date_format(`maintenance_log_entry`.`log_start_repair_time`,'%d %b %Y %h:%m') AS `Начало проведения работ`,date_format(`maintenance_log_entry`.`log_stop_repair_time`,'%d %b %Y %h:%m') AS `Конец проведения работ`,`maintenance_log_entry`.`log_type_of_maintenance` AS `Тип обслуживания`,`maintenance_log_entry`.`log_job_description` AS `Описание работ`,`employee`.`emp_name` AS `Исполнитель работ`,`equipment`.`equip_name` AS `Оборудования` from ((`maintenance_log_entry` join `equipment` on((`equipment`.`equip_inventory_number` = `maintenance_log_entry`.`equip_inventory_number`))) join `employee` on((`employee`.`emp_login` = `maintenance_log_entry`.`log_contractor`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `list_of_equipment`
--

/*!50001 DROP VIEW IF EXISTS `list_of_equipment`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `list_of_equipment` AS select `equipment`.`equip_name` AS `Название оборудования`,`equipment`.`equip_brand_type` AS `Производитель оборудования`,`equipment`.`equip_serial_number` AS `Серийный номер`,`equipment`.`equip_inventory_number` AS `Инвентарный номер`,`equipment`.`equip_installation_location` AS `Номер помещения`,`status`.`status_description` AS `Статус`,`employee`.`emp_name` AS `Ответственный` from ((`equipment` join `status` on((`equipment`.`status_ID` = `status`.`status_ID`))) join `employee` on((`equipment`.`emp_login` = `employee`.`emp_login`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `list_sp`
--

/*!50001 DROP VIEW IF EXISTS `list_sp`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `list_sp` AS select `spare_part`.`sp_name` AS `Наименование`,`spare_part`.`sp_article_number` AS `Артикульный номер`,`spare_part`.`sp_inventory_number` AS `Инвентарный номер`,`spare_part`.`sp_stock_quantity` AS `Кол-во на складе`,`spare_part`.`sp_ordered_quantity` AS `Заказано`,`spare_part`.`sp_min_quantity` AS `Мин. кол-во`,`spare_part`.`sp_abc_importance` AS `Важность по ABC`,`equipment`.`equip_name` AS `Оборудование` from (`spare_part` join `equipment` on((`spare_part`.`equip_inventory_number` = `equipment`.`equip_inventory_number`))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `report_for_order_sp`
--

/*!50001 DROP VIEW IF EXISTS `report_for_order_sp`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `report_for_order_sp` AS select `spare_part`.`sp_name` AS `Название`,`equipment`.`equip_name` AS `Для оборудования`,`spare_part`.`sp_article_number` AS `Артикульный номер`,(`spare_part`.`sp_min_quantity` - (`spare_part`.`sp_stock_quantity` + `spare_part`.`sp_ordered_quantity`)) AS `Кол-во к заказу`,`spare_part`.`sp_stock_quantity` AS `Остаток на складе`,`spare_part`.`sp_ordered_quantity` AS `Заказано`,`spare_part`.`sp_min_quantity` AS `Определённый минимум`,`spare_part`.`sp_abc_importance` AS `Важность по abc` from (`spare_part` join `equipment` on((`equipment`.`equip_inventory_number` = `spare_part`.`equip_inventory_number`))) where (`spare_part`.`sp_min_quantity` > (`spare_part`.`sp_stock_quantity` + `spare_part`.`sp_ordered_quantity`)) order by `spare_part`.`sp_abc_importance` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-08-11 22:10:18
