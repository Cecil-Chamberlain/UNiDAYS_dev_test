CREATE DATABASE UNiDAYS;
USE UNiDAYS;
CREATE TABLE Users ( UserID int NOT NULL IDENTITY(1,1) PRIMARY KEY, UserEmail varchar(255) NOT NULL UNIQUE, UserPassword varchar(255) NOT NULL, UserSalt varchar(255) NOT NULL);