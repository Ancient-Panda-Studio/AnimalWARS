-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 26, 2021 at 04:36 AM
-- Server version: 10.4.17-MariaDB
-- PHP Version: 8.0.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `unityacces`
--

-- --------------------------------------------------------

--
-- Table structure for table `friends`
--

CREATE TABLE `friends` (
  `ID` int(11) NOT NULL,
  `userone` varchar(12) NOT NULL,
  `usertwo` varchar(12) NOT NULL,
  `idone` int(11) NOT NULL,
  `idtwo` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `friends`
--

INSERT INTO `friends` (`ID`, `userone`, `usertwo`, `idone`, `idtwo`) VALUES
(1, 'username1', 'username2', 1, 2),
(2, 'username1', 'username3', 1, 3);

-- --------------------------------------------------------

--
-- Table structure for table `skins`
--

CREATE TABLE `skins` (
  `skinID` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `price` int(11) NOT NULL,
  `image` varchar(300) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `skins`
--

INSERT INTO `skins` (`skinID`, `name`, `price`, `image`) VALUES
(1, 'Pepelaugh', 1000, 'https://i.guim.co.uk/img/media/327e46c3ab049358fad80575146be9e0e65686e7/0_56_1023_614/master/1023.jpg?width=1200&height=1200&quality=85&auto=format&fit=crop&s=094ddc96f89a1ea9a4f68d1b585d1f97');

-- --------------------------------------------------------

--
-- Table structure for table `userdata`
--

CREATE TABLE `userdata` (
  `username` varchar(15) NOT NULL,
  `freeCurrency` int(10) NOT NULL DEFAULT 100,
  `paidCurrency` int(10) NOT NULL DEFAULT 10,
  `accountLevel` int(3) NOT NULL DEFAULT 1,
  `currentXP` int(5) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `userdata`
--

INSERT INTO `userdata` (`username`, `freeCurrency`, `paidCurrency`, `accountLevel`, `currentXP`) VALUES
('username1', 100, 10, 1, 1),
('username2', 100, 10, 1, 1),
('username3', 100, 10, 1, 1),
('username4', 100, 10, 1, 1),
('username5', 100, 10, 1, 1),
('username6', 100, 10, 1, 1),
('davidmata', 100, 10, 1, 1),
('davidmkasa', 100, 10, 1, 1),
('rubenventura', 100, 10, 1, 1),
('rubenventurga', 100, 10, 1, 1),
('rubenvengansa', 100, 10, 1, 1),
('rubenvergaso', 100, 10, 1, 1),
('rubenverhentaio', 100, 10, 1, 1),
('rubenventosa', 100, 10, 1, 1),
('', 100, 10, 1, 1),
('oriolpujolkekw', 100, 10, 1, 1),
('oriolpujolkekw2', 100, 10, 1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(15) NOT NULL,
  `salt` varchar(50) NOT NULL,
  `hash` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `online` int(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `username`, `salt`, `hash`, `email`, `online`) VALUES
(1, 'username1', '$5$rounds=5000$steamedhamsusername1$', '$5$rounds=5000$steamedhamsusern$8Xi5TuxG5l3TvyAjxKlRhFyZMi9bx7ZN3HUVX3IkXLD', 'username1@username1.com', 1),
(2, 'username2', '$5$rounds=5000$steamedhamsusername2$', '$5$rounds=5000$steamedhamsusern$bKNcDfC45XH23bt0Pd44P0rt4F46P5iom2wX5eh52P/', 'username1@username1.com', 0),
(3, 'username3', '$5$rounds=5000$steamedhamsusername3$', '$5$rounds=5000$steamedhamsusern$cztOQoktgCCanR2r/PIpGXFtHB13YV4yKUfKFAuZGkD', 'username1@username1.com', 1),
(4, 'username4', '$5$rounds=5000$steamedhamsusername4$', '$5$rounds=5000$steamedhamsusern$vK8awwUUGp/X1NFr1NQO2FcWoEYzYcyzX6Dgsyrwt66', 'username1@username1.com', 1),
(5, 'username5', '$5$rounds=5000$steamedhamsusername5$', '$5$rounds=5000$steamedhamsusern$YGlr7980.VmR9x7KR5fHiZen5n4KV22DeA0RIDhLu85', 'username1@username1.com', 1),
(6, 'username6', '$5$rounds=5000$steamedhamsusername6$', '$5$rounds=5000$steamedhamsusern$SEoeLRn01Tlgrri6UC09NqPsHVQRkugyEGFqdmf0Ur6', 'username1@username1.com', 0),
(7, 'davidmata', '$5$rounds=5000$steamedhamsdavidmata$', '$5$rounds=5000$steamedhamsdavid$MUj3VJHq5MUJUBfAAqlcuQiHtiCGLmBFWp2qF2LIQF/', 'davidmata@davidmata.com', 1),
(8, 'davidmkasa', '$5$rounds=5000$steamedhamsdavidmkasa$', '$5$rounds=5000$steamedhamsdavid$MUj3VJHq5MUJUBfAAqlcuQiHtiCGLmBFWp2qF2LIQF/', 'davidmata@davidmata.com', 1),
(9, 'rubenventura', '$5$rounds=5000$steamedhamsrubenventura$', '$5$rounds=5000$steamedhamsruben$w5X53LrmhjXJc3Efnvlop5CVHs..foNRKQYbgjt/Cp.', 'davidmata@davidmata.com', 1),
(10, 'rubenventurga', '$5$rounds=5000$steamedhamsrubenventurga$', '$5$rounds=5000$steamedhamsruben$w5X53LrmhjXJc3Efnvlop5CVHs..foNRKQYbgjt/Cp.', 'davidmata@davidmata.com', 1),
(11, 'rubenvengansa', '$5$rounds=5000$steamedhamsrubenvengansa$', '$5$rounds=5000$steamedhamsruben$L5yHTdzg97Jwb2Ekd94wDSTrUFGx.wejYA36EI3zJb0', 'rubenvengansa@rubenvengansa', 1),
(12, 'rubenvergaso', '$5$rounds=5000$steamedhamsrubenvergaso$', '$5$rounds=5000$steamedhamsruben$L5yHTdzg97Jwb2Ekd94wDSTrUFGx.wejYA36EI3zJb0', 'rubenvengansa@rubenvengansa', 1),
(13, 'rubenverhentaio', '$5$rounds=5000$steamedhamsrubenverhentaionline$', '$5$rounds=5000$steamedhamsruben$L5yHTdzg97Jwb2Ekd94wDSTrUFGx.wejYA36EI3zJb0', 'rubenvengansa@rubenvengansa', 1),
(14, 'rubenventosa', '$5$rounds=5000$steamedhamsrubenventosa$', '$5$rounds=5000$steamedhamsruben$L5yHTdzg97Jwb2Ekd94wDSTrUFGx.wejYA36EI3zJb0', 'rubenvengansa@rubenvengansa', 1),
(15, '', '$5$rounds=5000$steamedhams$', '$5$rounds=5000$steamedhams$KRaxREhYX4K9X8sbjvEHOJNSd9Wp/o6ds0La0x8egc0', '', 1),
(16, 'oriolpujolkekw', '$5$rounds=5000$steamedhamsoriolpujolkekw$', '$5$rounds=5000$steamedhamsoriol$Tew1T4jnlSDAd9qZlEQgLtlzFYreJeiAupFy0ykWtE.', 'oriolpujolkekw@gmail.com', 1),
(17, 'oriolpujolkekw2', '$5$rounds=5000$steamedhamsoriolpujolkekw2$', '$5$rounds=5000$steamedhamsoriol$Tew1T4jnlSDAd9qZlEQgLtlzFYreJeiAupFy0ykWtE.', 'oriolpujolkekw@gmail.com', 1);

-- --------------------------------------------------------

--
-- Table structure for table `userskins`
--

CREATE TABLE `userskins` (
  `userid` int(11) NOT NULL,
  `skinID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `userskins`
--

INSERT INTO `userskins` (`userid`, `skinID`) VALUES
(1, 1),
(1, 1),
(2, 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `friends`
--
ALTER TABLE `friends`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `skins`
--
ALTER TABLE `skins`
  ADD PRIMARY KEY (`skinID`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `friends`
--
ALTER TABLE `friends`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `skins`
--
ALTER TABLE `skins`
  MODIFY `skinID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
