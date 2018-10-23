-- phpMyAdmin SQL Dump
-- version 4.8.2
-- https://www.phpmyadmin.net/
--
-- 主機: localhost
-- 產生時間： 2018-10-23 11:04:51
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
-- 資料表結構 `equipment`
--

CREATE TABLE `equipment` (
  `id` bigint(20) NOT NULL COMMENT 'ID',
  `jid` int(20) NOT NULL COMMENT '表格ID',
  `equipType` tinyint(4) NOT NULL DEFAULT '0' COMMENT '裝備類型(0:武器,1:防具,2:飾品)',
  `equipSlot` tinyint(4) NOT NULL DEFAULT '0' COMMENT '裝備插槽(0:沒裝備,1:武器,2:防具,3:飾品1,4:飾品2)',
  `lv` int(10) NOT NULL COMMENT '等級',
  `quality` int(10) NOT NULL COMMENT '品階',
  `ownUserID` bigint(20) NOT NULL COMMENT '擁有此裝備的玩家ID',
  `acquireTime` datetime NOT NULL COMMENT '獲得時間'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- 資料表的匯出資料 `equipment`
--

INSERT INTO `equipment` (`id`, `jid`, `equipType`, `equipSlot`, `lv`, `quality`, `ownUserID`, `acquireTime`) VALUES
(2, 2, 0, 1, 3, 4, 1, '2018-10-12 00:00:00'),
(7, 2, 1, 2, 2, 2, 1, '2018-10-12 00:00:00'),
(8, 2, 1, 0, 2, 1, 1, '2018-10-12 00:00:00'),
(10, 4, 2, 4, 3, 4, 1, '2018-10-12 00:00:00'),
(12, 2, 2, 3, 3, 2, 1, '2018-10-12 00:00:00'),
(13, 2, 0, 0, 3, 5, 1, '2018-10-12 00:00:00'),
(15, 2, 0, 0, 3, 2, 1, '2018-10-12 00:00:00'),
(16, 209, 0, 0, 3, 5, 0, '0000-00-00 00:00:00'),
(17, 42, 0, 0, 2, 1, 0, '0000-00-00 00:00:00'),
(18, 199, 0, 0, 3, 5, 0, '0000-00-00 00:00:00'),
(19, 48, 1, 0, 2, 1, 0, '0000-00-00 00:00:00'),
(20, 153, 2, 0, 3, 5, 0, '0000-00-00 00:00:00'),
(21, 105, 2, 0, 2, 1, 0, '0000-00-00 00:00:00'),
(22, 4, 1, 0, 3, 5, 0, '0000-00-00 00:00:00'),
(23, 232, 0, 0, 2, 1, 0, '0000-00-00 00:00:00'),
(29, 96, 2, 0, 1, 3, 0, '0000-00-00 00:00:00'),
(30, 60, 2, 0, 1, 1, 0, '0000-00-00 00:00:00'),
(31, 145, 2, 0, 2, 1, 0, '0000-00-00 00:00:00'),
(32, 157, 1, 0, 2, 3, 0, '0000-00-00 00:00:00'),
(33, 152, 1, 0, 2, 1, 0, '0000-00-00 00:00:00');

-- --------------------------------------------------------

--
-- 資料表結構 `playeraccount`
--

CREATE TABLE `playeraccount` (
  `id` bigint(20) NOT NULL COMMENT '玩家帳戶ID',
  `ac_K` varchar(20) NOT NULL COMMENT 'Kongregate_Name',
  `userID_K` int(20) NOT NULL COMMENT 'Kongregate UserID',
  `gold` int(10) NOT NULL COMMENT '金幣',
  `emerald` int(10) NOT NULL COMMENT '綠寶石',
  `maxFloor` int(20) NOT NULL,
  `signUpTime` datetime NOT NULL COMMENT '註冊時間',
  `signInTime` datetime NOT NULL COMMENT '最後登入時間'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- 資料表的匯出資料 `playeraccount`
--

INSERT INTO `playeraccount` (`id`, `ac_K`, `userID_K`, `gold`, `emerald`, `maxFloor`, `signUpTime`, `signInTime`) VALUES
(1, 'scozirge', 1, 2927, 500, 3, '2018-10-11 18:14:28', '2018-10-23 09:58:16');

-- --------------------------------------------------------

--
-- 資料表結構 `strengthen`
--

CREATE TABLE `strengthen` (
  `id` bigint(20) NOT NULL COMMENT 'ID',
  `jid` int(11) NOT NULL COMMENT '表格ID',
  `lv` int(11) NOT NULL DEFAULT '0' COMMENT '等級',
  `ownUserID` bigint(20) NOT NULL COMMENT '擁有此強化的玩家ID',
  `acquireTime` datetime NOT NULL COMMENT '更新時間'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- 資料表的匯出資料 `strengthen`
--

INSERT INTO `strengthen` (`id`, `jid`, `lv`, `ownUserID`, `acquireTime`) VALUES
(1, 1, 7, 1, '0000-00-00 00:00:00'),
(2, 2, 2, 1, '2018-10-13 11:45:52'),
(3, 3, 4, 1, '2018-10-18 10:01:13'),
(9, 4, 3, 1, '2018-10-18 10:06:33'),
(10, 5, 2, 1, '2018-10-18 10:08:23');

--
-- 已匯出資料表的索引
--

--
-- 資料表索引 `equipment`
--
ALTER TABLE `equipment`
  ADD PRIMARY KEY (`id`),
  ADD KEY `ownUserID` (`ownUserID`);

--
-- 資料表索引 `playeraccount`
--
ALTER TABLE `playeraccount`
  ADD PRIMARY KEY (`id`),
  ADD KEY `userID_K` (`userID_K`);

--
-- 資料表索引 `strengthen`
--
ALTER TABLE `strengthen`
  ADD PRIMARY KEY (`id`),
  ADD KEY `ownUserID` (`ownUserID`);

--
-- 在匯出的資料表使用 AUTO_INCREMENT
--

--
-- 使用資料表 AUTO_INCREMENT `equipment`
--
ALTER TABLE `equipment`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID', AUTO_INCREMENT=34;

--
-- 使用資料表 AUTO_INCREMENT `playeraccount`
--
ALTER TABLE `playeraccount`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '玩家帳戶ID', AUTO_INCREMENT=5;

--
-- 使用資料表 AUTO_INCREMENT `strengthen`
--
ALTER TABLE `strengthen`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT 'ID', AUTO_INCREMENT=11;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
