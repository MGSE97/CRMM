CREATE TABLE `crmm`.`RequestLog` (
  `Id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `Url` VARCHAR(2048) NULL,
  `CreatedOnUtc` DATETIME NOT NULL DEFAULT NOW(),
  `UserId` INT(10) UNSIGNED NULL,
  PRIMARY KEY (`Id`),
  INDEX `RequestUser_idx` (`UserId` ASC),
  CONSTRAINT `RequestUser`
    FOREIGN KEY (`UserId`)
    REFERENCES `crmm`.`User` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);