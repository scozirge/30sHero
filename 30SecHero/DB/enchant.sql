-- phpMyAdmin SQL Dump
-- version 4.8.2
-- https://www.phpmyadmin.net/
--
-- 主機: localhost
-- 產生時間： 2018-11-14 14:09:49
-- 伺服器版本: 10.1.10-MariaDB
-- PHP 版本： 5.6.19

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 資料庫： `30sechero`
--

-- --------------------------------------------------------

--
-- 資料表結構 `enchant`
--

CREATE TABLE `enchant` (
  `id` bigint(20) NOT NULL COMMENT 'ID',
  `jid` int(20) NOT NULL COMMENT '表格ID',
  `lv` int(20) NOT NULL COMMENT '等級',
  `ownUserID` bigint(20) NOT NULL COMMENT '擁有此附魔的玩家ID',
  `acquireTime` datetime NOT NULL COMMENT '更新時間'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- 資料表的匯出資料 `enchant`
--

INSERT INTO `enchant` (`id`, `jid`, `lv`, `ownUserID`, `acquireTime`) VALUES
(1, 1, 1, 1, '2018-11-14 14:03:10'),
(2, 2, 1, 1, '2018-11-14 14:04:28');

--
-- 已匯出資料表的索引
--

--
-- 資料表索引 `enchant`
--
ALTER TABLE `enchant`
  ADD PRIMARY KEY (`id`);

--
-- 在匯出的資料表使用 AUTO_INCREMENT
--

--
-- 使用資料表 AUTO_INCREMENT `enchant`
--
ALTER TABLE `enchant`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID', AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
