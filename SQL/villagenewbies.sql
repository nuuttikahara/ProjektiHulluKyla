-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema vn
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `vn` ;

-- -----------------------------------------------------
-- Schema vn
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `vn` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_bin ;
USE `vn` ;

-- -----------------------------------------------------
-- Table `vn`.`alue`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `vn`.`alue` ;

CREATE TABLE IF NOT EXISTS `vn`.`alue` (
  `alue_id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `nimi` VARCHAR(40) NULL DEFAULT NULL,
  PRIMARY KEY (`alue_id`),
  INDEX `alue_nimi_index` (`nimi` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


-- -----------------------------------------------------
-- Table `vn`.`posti`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `vn`.`posti` ;

CREATE TABLE IF NOT EXISTS `vn`.`posti` (
  `postinro` CHAR(5) NOT NULL,
  `toimipaikka` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`postinro`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


-- -----------------------------------------------------
-- Table `vn`.`asiakas`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `vn`.`asiakas` ;

CREATE TABLE IF NOT EXISTS `vn`.`asiakas` (
  `asiakas_id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `postinro` CHAR(5) NOT NULL,
  `etunimi` VARCHAR(20) NULL DEFAULT NULL,
  `sukunimi` VARCHAR(40) NULL DEFAULT NULL,
  `lahiosoite` VARCHAR(40) NULL DEFAULT NULL,
  `email` VARCHAR(50) NULL DEFAULT NULL,
  `puhelinnro` VARCHAR(15) NULL DEFAULT NULL,
  PRIMARY KEY (`asiakas_id`),
  INDEX `fk_as_posti1_idx` (`postinro` ASC) VISIBLE,
  INDEX `asiakas_snimi_idx` (`sukunimi` ASC) VISIBLE,
  INDEX `asiakas_enimi_idx` (`etunimi` ASC) VISIBLE,
  CONSTRAINT `fk_asiakas_posti`
    FOREIGN KEY (`postinro`)
    REFERENCES `vn`.`posti` (`postinro`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


-- -----------------------------------------------------
-- Table `vn`.`mokki`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `vn`.`mokki` ;

CREATE TABLE IF NOT EXISTS `vn`.`mokki` (
  `mokki_id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `alue_id` INT UNSIGNED NOT NULL,
  `postinro` CHAR(5) NOT NULL,
  `mokkinimi` VARCHAR(45) NULL DEFAULT NULL,
  `katuosoite` VARCHAR(45) NULL DEFAULT NULL,
  `hinta` DOUBLE(8,2) NOT NULL,
  `kuvaus` VARCHAR(150) NULL DEFAULT NULL,
  `henkilomaara` INT NULL DEFAULT NULL,
  `varustelu` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`mokki_id`),
  INDEX `fk_mokki_alue_idx` (`alue_id` ASC) VISIBLE,
  INDEX `fk_mokki_posti_idx` (`postinro` ASC) VISIBLE,
  CONSTRAINT `fk_mokki_alue`
    FOREIGN KEY (`alue_id`)
    REFERENCES `vn`.`alue` (`alue_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_mokki_posti`
    FOREIGN KEY (`postinro`)
    REFERENCES `vn`.`posti` (`postinro`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


-- -----------------------------------------------------
-- Table `vn`.`varaus`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `vn`.`varaus` ;

CREATE TABLE IF NOT EXISTS `vn`.`varaus` (
  `varaus_id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `asiakas_id` INT UNSIGNED NOT NULL,
  `mokki_id` INT UNSIGNED NOT NULL,
  `varattu_pvm` DATETIME NULL DEFAULT NULL,
  `vahvistus_pvm` DATETIME NULL DEFAULT NULL,
  `varattu_alkupvm` DATETIME NULL DEFAULT NULL,
  `varattu_loppupvm` DATETIME NULL DEFAULT NULL,
  PRIMARY KEY (`varaus_id`),
  INDEX `varaus_as_id_index` (`asiakas_id` ASC) VISIBLE,
  INDEX `fk_var_mok_idx` (`mokki_id` ASC) VISIBLE,
  CONSTRAINT `fk_varaus_mokki`
    FOREIGN KEY (`mokki_id`)
    REFERENCES `vn`.`mokki` (`mokki_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `varaus_ibfk`
    FOREIGN KEY (`asiakas_id`)
    REFERENCES `vn`.`asiakas` (`asiakas_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


-- -----------------------------------------------------
-- Table `vn`.`lasku`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `vn`.`lasku` ;

CREATE TABLE IF NOT EXISTS `vn`.`lasku` (
  `lasku_id` INT NOT NULL,
  `varaus_id` INT UNSIGNED NOT NULL,
  `summa` DOUBLE(8,2) NOT NULL,
  `alv` DOUBLE(8,2) NOT NULL,
  `maksettu` DOUBLE(8,2) NOT NULL DEFAULT 0,
  PRIMARY KEY (`lasku_id`),
  INDEX `lasku_ibfk_1` (`varaus_id` ASC) VISIBLE,
  CONSTRAINT `lasku_ibfk_1`
    FOREIGN KEY (`varaus_id`)
    REFERENCES `vn`.`varaus` (`varaus_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


-- -----------------------------------------------------
-- Table `vn`.`palvelu`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `vn`.`palvelu` ;

CREATE TABLE IF NOT EXISTS `vn`.`palvelu` (
  `palvelu_id` INT UNSIGNED NOT NULL,
  `alue_id` INT UNSIGNED NOT NULL,
  `nimi` VARCHAR(40) NULL DEFAULT NULL,
  `kuvaus` VARCHAR(255) NULL DEFAULT NULL,
  `hinta` DOUBLE(8,2) NOT NULL,
  `alv` DOUBLE(8,2) NOT NULL,
  PRIMARY KEY (`palvelu_id`),
  INDEX `Palvelu_nimi_index` (`nimi` ASC) VISIBLE,
  INDEX `palv_toimip_id_ind` (`alue_id` ASC) VISIBLE,
  CONSTRAINT `palvelu_ibfk_1`
    FOREIGN KEY (`alue_id`)
    REFERENCES `vn`.`alue` (`alue_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


-- -----------------------------------------------------
-- Table `vn`.`varauksen_palvelut`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `vn`.`varauksen_palvelut` ;

CREATE TABLE IF NOT EXISTS `vn`.`varauksen_palvelut` (
  `varaus_id` INT UNSIGNED NOT NULL,
  `palvelu_id` INT UNSIGNED NOT NULL,
  `lkm` INT NOT NULL,
  PRIMARY KEY (`palvelu_id`, `varaus_id`),
  INDEX `vp_varaus_id_index` (`varaus_id` ASC) VISIBLE,
  INDEX `vp_palvelu_id_index` (`palvelu_id` ASC) VISIBLE,
  CONSTRAINT `fk_palvelu`
    FOREIGN KEY (`palvelu_id`)
    REFERENCES `vn`.`palvelu` (`palvelu_id`),
  CONSTRAINT `fk_varaus`
    FOREIGN KEY (`varaus_id`)
    REFERENCES `vn`.`varaus` (`varaus_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_bin;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `vn`.`alue`
-- -----------------------------------------------------
START TRANSACTION;
USE `vn`;
INSERT INTO `vn`.`alue` (`alue_id`, `nimi`) VALUES (1, 'Levi');
INSERT INTO `vn`.`alue` (`alue_id`, `nimi`) VALUES (2, 'Ylläs');
INSERT INTO `vn`.`alue` (`alue_id`, `nimi`) VALUES (3, 'Ruka');

COMMIT;


-- -----------------------------------------------------
-- Data for table `vn`.`posti`
-- -----------------------------------------------------
START TRANSACTION;
USE `vn`;
INSERT INTO `vn`.`posti` (`postinro`, `toimipaikka`) VALUES ('70100', 'Kuopio');
INSERT INTO `vn`.`posti` (`postinro`, `toimipaikka`) VALUES ('45100', 'Kouvola');
INSERT INTO `vn`.`posti` (`postinro`, `toimipaikka`) VALUES ('00100', 'Helsinki');
INSERT INTO `vn`.`posti` (`postinro`, `toimipaikka`) VALUES ('99100', 'Levi');
INSERT INTO `vn`.`posti` (`postinro`, `toimipaikka`) VALUES ('93100', 'Ruka');
INSERT INTO `vn`.`posti` (`postinro`, `toimipaikka`) VALUES ('95100', 'Ylläs');

COMMIT;


-- -----------------------------------------------------
-- Data for table `vn`.`asiakas`
-- -----------------------------------------------------
START TRANSACTION;
USE `vn`;
INSERT INTO `vn`.`asiakas` (`asiakas_id`, `postinro`, `etunimi`, `sukunimi`, `lahiosoite`, `email`, `puhelinnro`) VALUES (1, '70100', 'Mikko', 'Mallikas', 'Torikatu 13', 'mikko.mallikas@gmail.com', '0454783293');
INSERT INTO `vn`.`asiakas` (`asiakas_id`, `postinro`, `etunimi`, `sukunimi`, `lahiosoite`, `email`, `puhelinnro`) VALUES (2, '45100', 'Jermu', 'Jermunen', 'Kauppalankatu 10', 'jermujee@hotmail.com', '0400778374');
INSERT INTO `vn`.`asiakas` (`asiakas_id`, `postinro`, `etunimi`, `sukunimi`, `lahiosoite`, `email`, `puhelinnro`) VALUES (3, '00100', 'Saara', 'Järvi', 'Mölykatu 4', 'saarajarvi@gmail.com', '0403498766');
INSERT INTO `vn`.`asiakas` (`asiakas_id`, `postinro`, `etunimi`, `sukunimi`, `lahiosoite`, `email`, `puhelinnro`) VALUES (4, '70100', 'Jenni', 'Mallikas', 'Torikatu 13', 'jennityo@outlook.com', '0447833456');

COMMIT;


-- -----------------------------------------------------
-- Data for table `vn`.`mokki`
-- -----------------------------------------------------
START TRANSACTION;
USE `vn`;
INSERT INTO `vn`.`mokki` (`mokki_id`, `alue_id`, `postinro`, `mokkinimi`, `katuosoite`, `hinta`, `kuvaus`, `henkilomaara`, `varustelu`) VALUES (110, 1, '99100', 'Mellu', 'Levintie 2', 1200, 'Kaunis ja tilava perhemökki', 7, 'Suihku, pesukone, astianpesukone, sauna, TV, poreallas');
INSERT INTO `vn`.`mokki` (`mokki_id`, `alue_id`, `postinro`, `mokkinimi`, `katuosoite`, `hinta`, `kuvaus`, `henkilomaara`, `varustelu`) VALUES (305, 3, '93100', 'Rutku', 'Rukantie 17', 890, 'Pieni ja valoisa mökki', 3, 'Suihku, sauna, astianpesukone');
INSERT INTO `vn`.`mokki` (`mokki_id`, `alue_id`, `postinro`, `mokkinimi`, `katuosoite`, `hinta`, `kuvaus`, `henkilomaara`, `varustelu`) VALUES (75, 2, '95100', 'Lumiparatiisi', 'Äkäslompolontie 4', 1599.99, 'Paratiisi mökissä', 9, 'Suihku, sauna, pesukone, astianpesukone, TV, poreallas, pelihuone');

COMMIT;


-- -----------------------------------------------------
-- Data for table `vn`.`varaus`
-- -----------------------------------------------------
START TRANSACTION;
USE `vn`;
INSERT INTO `vn`.`varaus` (`varaus_id`, `asiakas_id`, `mokki_id`, `varattu_pvm`, `vahvistus_pvm`, `varattu_alkupvm`, `varattu_loppupvm`) VALUES (1, 1, 305, '2024-11-09 12:31:24', '2024-11-09 12:33:05', '2025-02-27 12:00:00', '2025-03-03 12:00:00');
INSERT INTO `vn`.`varaus` (`varaus_id`, `asiakas_id`, `mokki_id`, `varattu_pvm`, `vahvistus_pvm`, `varattu_alkupvm`, `varattu_loppupvm`) VALUES (2, 2, 110, '2024-05-11 20:29:01', '2024-05-11 20:31:04', '2024-12-23 12:00:00', '2024-12-30 12:00:00');
INSERT INTO `vn`.`varaus` (`varaus_id`, `asiakas_id`, `mokki_id`, `varattu_pvm`, `vahvistus_pvm`, `varattu_alkupvm`, `varattu_loppupvm`) VALUES (3, 3, 75, '2024-08-05 09:24:22', '2024-08-05 09:24:57', '2024-12-30 14:00:00', '2025-01-02 12:00:00');

COMMIT;


-- -----------------------------------------------------
-- Data for table `vn`.`lasku`
-- -----------------------------------------------------
START TRANSACTION;
USE `vn`;
INSERT INTO `vn`.`lasku` (`lasku_id`, `varaus_id`, `summa`, `alv`, `maksettu`) VALUES (1, 1, 1340, 10, 890);
INSERT INTO `vn`.`lasku` (`lasku_id`, `varaus_id`, `summa`, `alv`, `maksettu`) VALUES (2, 2, 1480, 10, 0);
INSERT INTO `vn`.`lasku` (`lasku_id`, `varaus_id`, `summa`, `alv`, `maksettu`) VALUES (3, 3, 1959.99, 10, 1959.99);

COMMIT;


-- -----------------------------------------------------
-- Data for table `vn`.`palvelu`
-- -----------------------------------------------------
START TRANSACTION;
USE `vn`;
INSERT INTO `vn`.`palvelu` (`palvelu_id`, `alue_id`, `nimi`, `kuvaus`, `hinta`, `alv`) VALUES (1, 1, 'Siivouspalvelu', 'Talon siivous, lakanoiden vaihto', 40, 10.5);
INSERT INTO `vn`.`palvelu` (`palvelu_id`, `alue_id`, `nimi`, `kuvaus`, `hinta`, `alv`) VALUES (2, 1, 'Ruokatoimitus', 'Asiakkaan tilattu ruoka toimitetaan mökille', 25, 25);
INSERT INTO `vn`.`palvelu` (`palvelu_id`, `alue_id`, `nimi`, `kuvaus`, `hinta`, `alv`) VALUES (3, 2, 'Siivouspalvelu', 'Talon siivous, lakanoiden vaihto', 45, 10.5);
INSERT INTO `vn`.`palvelu` (`palvelu_id`, `alue_id`, `nimi`, `kuvaus`, `hinta`, `alv`) VALUES (4, 2, 'Tilataksi', 'Kuljetus mökiltä laskettelurinteelle', 60, 14);
INSERT INTO `vn`.`palvelu` (`palvelu_id`, `alue_id`, `nimi`, `kuvaus`, `hinta`, `alv`) VALUES (5, 3, 'Siivouspalvelu', 'Talon siivous, lakanoiden vaihto', 42, 10.5);
INSERT INTO `vn`.`palvelu` (`palvelu_id`, `alue_id`, `nimi`, `kuvaus`, `hinta`, `alv`) VALUES (6, 3, 'Tilataksi', 'Kuljetus mökiltä laskettelurinteelle', 80, 14);

COMMIT;


-- -----------------------------------------------------
-- Data for table `vn`.`varauksen_palvelut`
-- -----------------------------------------------------
START TRANSACTION;
USE `vn`;
INSERT INTO `vn`.`varauksen_palvelut` (`varaus_id`, `palvelu_id`, `lkm`) VALUES (1, 5, 5);
INSERT INTO `vn`.`varauksen_palvelut` (`varaus_id`, `palvelu_id`, `lkm`) VALUES (1, 6, 3);
INSERT INTO `vn`.`varauksen_palvelut` (`varaus_id`, `palvelu_id`, `lkm`) VALUES (2, 1, 7);
INSERT INTO `vn`.`varauksen_palvelut` (`varaus_id`, `palvelu_id`, `lkm`) VALUES (3, 3, 4);
INSERT INTO `vn`.`varauksen_palvelut` (`varaus_id`, `palvelu_id`, `lkm`) VALUES (3, 4, 3);

COMMIT;

