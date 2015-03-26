-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.6.15-log - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL Version:             9.1.0.4867
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for function bjs.GetGameType
DROP FUNCTION IF EXISTS `GetGameType`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` FUNCTION `GetGameType`(`cClassName` CHAR(1) CHARSET utf8, `yYear` YEAR(4)) RETURNS enum('Competition','Traditional') CHARSET utf8
    READS SQL DATA
BEGIN

RETURN (select GameType from DisciplineMeta inner join Class on DisciplineMeta.ClassPKey = Class.PKey where ClassName = cClassName COLLATE utf8_unicode_ci AND Year = yYear);

END//
DELIMITER ;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;