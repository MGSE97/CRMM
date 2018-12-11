-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema crmm
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema crmm
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `crmm` DEFAULT CHARACTER SET utf8 ;
SHOW WARNINGS;
USE `crmm` ;

-- -----------------------------------------------------
-- Table `Role`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Role` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `Role` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE UNIQUE INDEX `Id_UNIQUE` ON `Role` (`Id` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `State`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `State` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `State` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Type` VARCHAR(50) NOT NULL,
  `Description` VARCHAR(1024) NULL,
  `CreatedOnUtc` DATETIME NOT NULL DEFAULT NOW(),
  `DeletedOnUtc` DATETIME NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE UNIQUE INDEX `Id_UNIQUE` ON `State` (`Id` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `Order`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Order` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `Order` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(100) NOT NULL,
  `Type` VARCHAR(50) NOT NULL,
  `Description` VARCHAR(1024) NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE UNIQUE INDEX `Id_UNIQUE` ON `Order` (`Id` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `Place`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Place` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `Place` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(100) NOT NULL,
  `Type` VARCHAR(50) NOT NULL,
  `Address` VARCHAR(300) NOT NULL,
  `X` DOUBLE NULL,
  `Y` DOUBLE NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE UNIQUE INDEX `Id_UNIQUE` ON `Place` (`Id` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `User`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `User` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `User` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(100) NOT NULL,
  `Email` VARCHAR(200) NOT NULL,
  `Password` VARCHAR(255) NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE UNIQUE INDEX `Id_UNIQUE` ON `User` (`Id` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `User_has_State`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `User_has_State` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `User_has_State` (
  `UserId` INT UNSIGNED NOT NULL,
  `StateId` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`UserId`, `StateId`),
  CONSTRAINT `fk_User_has_State_User`
    FOREIGN KEY (`UserId`)
    REFERENCES `User` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_User_has_State_State1`
    FOREIGN KEY (`StateId`)
    REFERENCES `State` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE INDEX `fk_User_has_State_State1_idx` ON `User_has_State` (`StateId` ASC);

SHOW WARNINGS;
CREATE INDEX `fk_User_has_State_User_idx` ON `User_has_State` (`UserId` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `OrderState`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `OrderState` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `OrderState` (
  `StateId` INT UNSIGNED NOT NULL,
  `OrderId` INT UNSIGNED NOT NULL,
  `PlaceId` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`StateId`, `OrderId`, `PlaceId`),
  CONSTRAINT `fk_State_has_Order_State1`
    FOREIGN KEY (`StateId`)
    REFERENCES `State` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_State_has_Order_Order1`
    FOREIGN KEY (`OrderId`)
    REFERENCES `Order` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_State_has_Order_Place1`
    FOREIGN KEY (`PlaceId`)
    REFERENCES `Place` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE INDEX `fk_State_has_Order_Order1_idx` ON `OrderState` (`OrderId` ASC);

SHOW WARNINGS;
CREATE INDEX `fk_State_has_Order_State1_idx` ON `OrderState` (`StateId` ASC);

SHOW WARNINGS;
CREATE INDEX `fk_State_has_Order_Place1_idx` ON `OrderState` (`PlaceId` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `User_has_Place`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `User_has_Place` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `User_has_Place` (
  `UserId` INT UNSIGNED NOT NULL,
  `PlaceId` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`UserId`, `PlaceId`),
  CONSTRAINT `fk_User_has_Place_User1`
    FOREIGN KEY (`UserId`)
    REFERENCES `User` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_User_has_Place_Place1`
    FOREIGN KEY (`PlaceId`)
    REFERENCES `Place` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE INDEX `fk_User_has_Place_Place1_idx` ON `User_has_Place` (`PlaceId` ASC);

SHOW WARNINGS;
CREATE INDEX `fk_User_has_Place_User1_idx` ON `User_has_Place` (`UserId` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `User_has_Role`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `User_has_Role` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `User_has_Role` (
  `UserId` INT UNSIGNED NOT NULL,
  `RoleId` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`UserId`, `RoleId`),
  CONSTRAINT `fk_User_has_Role_User1`
    FOREIGN KEY (`UserId`)
    REFERENCES `User` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_User_has_Role_Role1`
    FOREIGN KEY (`RoleId`)
    REFERENCES `Role` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE INDEX `fk_User_has_Role_Role1_idx` ON `User_has_Role` (`RoleId` ASC);

SHOW WARNINGS;
CREATE INDEX `fk_User_has_Role_User1_idx` ON `User_has_Role` (`UserId` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `User_has_Order`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `User_has_Order` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `User_has_Order` (
  `UserId` INT UNSIGNED NOT NULL,
  `OrderId` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`UserId`, `OrderId`),
  CONSTRAINT `fk_User_has_Order_User1`
    FOREIGN KEY (`UserId`)
    REFERENCES `User` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_User_has_Order_Order1`
    FOREIGN KEY (`OrderId`)
    REFERENCES `Order` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE INDEX `fk_User_has_Order_Order1_idx` ON `User_has_Order` (`OrderId` ASC);

SHOW WARNINGS;
CREATE INDEX `fk_User_has_Order_User1_idx` ON `User_has_Order` (`UserId` ASC);

SHOW WARNINGS;

-- -----------------------------------------------------
-- Table `Place_has_State`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Place_has_State` ;

SHOW WARNINGS;
CREATE TABLE IF NOT EXISTS `Place_has_State` (
  `PlaceId` INT UNSIGNED NOT NULL,
  `StateId` INT UNSIGNED NOT NULL,
  PRIMARY KEY (`PlaceId`, `StateId`),
  CONSTRAINT `fk_Place_has_State_Place1`
    FOREIGN KEY (`PlaceId`)
    REFERENCES `Place` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Place_has_State_State1`
    FOREIGN KEY (`StateId`)
    REFERENCES `State` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;
CREATE INDEX `fk_Place_has_State_State1_idx` ON `Place_has_State` (`StateId` ASC);

SHOW WARNINGS;
CREATE INDEX `fk_Place_has_State_Place1_idx` ON `Place_has_State` (`PlaceId` ASC);

SHOW WARNINGS;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
