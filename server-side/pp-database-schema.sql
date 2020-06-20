-- Adminer 4.7.7 MySQL dump

SET NAMES utf8;
SET time_zone = '+00:00';
SET foreign_key_checks = 0;
SET sql_mode = 'NO_AUTO_VALUE_ON_ZERO';

SET NAMES utf8mb4;

DROP DATABASE IF EXISTS `pp`;
CREATE DATABASE `pp` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `pp`;

DROP TABLE IF EXISTS `inventories`;
CREATE TABLE `inventories` (
  `inv_id` int unsigned NOT NULL,
  `name` tinytext NOT NULL,
  `max_slots` int NOT NULL,
  PRIMARY KEY (`inv_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DROP TABLE IF EXISTS `inventory_map`;
CREATE TABLE `inventory_map` (
  `inv_id` int unsigned NOT NULL,
  `item_id` int unsigned NOT NULL,
  `quantity` int NOT NULL,
  KEY `inv_id_item_id` (`inv_id`,`item_id`),
  KEY `item_id` (`item_id`),
  CONSTRAINT `inventory_map_ibfk_1` FOREIGN KEY (`inv_id`) REFERENCES `inventories` (`inv_id`) ON DELETE CASCADE,
  CONSTRAINT `inventory_map_ibfk_3` FOREIGN KEY (`item_id`) REFERENCES `items` (`item_id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DROP TABLE IF EXISTS `items`;
CREATE TABLE `items` (
  `item_id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` tinytext NOT NULL,
  `description` text NOT NULL,
  `picture_location` tinytext,
  `owner_id` int unsigned NOT NULL,
  PRIMARY KEY (`item_id`),
  KEY `owner_id` (`owner_id`),
  CONSTRAINT `items_ibfk_1` FOREIGN KEY (`owner_id`) REFERENCES `players` (`player_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


DROP TABLE IF EXISTS `players`;
CREATE TABLE `players` (
  `player_id` int unsigned NOT NULL AUTO_INCREMENT,
  `username` tinytext NOT NULL,
  `last_login` timestamp NULL DEFAULT NULL,
  `password_hash` text,
  `inv_id` int unsigned DEFAULT NULL,
  PRIMARY KEY (`player_id`),
  KEY `inv_id` (`inv_id`),
  CONSTRAINT `players_ibfk_1` FOREIGN KEY (`inv_id`) REFERENCES `inventories` (`inv_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


-- 2020-06-20 15:25:11
