-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 07-10-2020 a las 18:06:41
-- Versión del servidor: 10.4.14-MariaDB
-- Versión de PHP: 7.4.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `unityacces`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `friends`
--

CREATE TABLE `friends` (
  `ID` int(11) NOT NULL,
  `userone` varchar(12) NOT NULL,
  `usertwo` varchar(12) NOT NULL,
  `idone` int(11) NOT NULL,
  `idtwo` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `friends`
--

INSERT INTO `friends` (`ID`, `userone`, `usertwo`, `idone`, `idtwo`) VALUES
(1, 'username1', 'username2', 1, 2),
(2, 'username1', 'username3', 1, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `skins`
--

CREATE TABLE `skins` (
  `skinID` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `price` int(11) NOT NULL,
  `image` varchar(300) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `skins`
--

INSERT INTO `skins` (`skinID`, `name`, `price`, `image`) VALUES
(1, 'Pepelaugh', 1000, 'https://i.guim.co.uk/img/media/327e46c3ab049358fad80575146be9e0e65686e7/0_56_1023_614/master/1023.jpg?width=1200&height=1200&quality=85&auto=format&fit=crop&s=094ddc96f89a1ea9a4f68d1b585d1f97');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(15) NOT NULL,
  `salt` varchar(50) NOT NULL,
  `hash` varchar(100) NOT NULL,
  `coins` int(11) NOT NULL DEFAULT 0,
  `level` int(11) NOT NULL DEFAULT 1,
  `currentxp` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `users`
--

INSERT INTO `users` (`id`, `username`, `salt`, `hash`, `coins`, `level`, `currentxp`) VALUES
(1, 'username1', '$5$rounds=5000$steamedhamsusername1$', '$5$rounds=5000$steamedhamsusern$8Xi5TuxG5l3TvyAjxKlRhFyZMi9bx7ZN3HUVX3IkXLD', 0, 1, 0),
(2, 'username2', '$5$rounds=5000$steamedhamsusername2$', '$5$rounds=5000$steamedhamsusern$bKNcDfC45XH23bt0Pd44P0rt4F46P5iom2wX5eh52P/', 0, 1, 0),
(3, 'username3', '$5$rounds=5000$steamedhamsusername3$', '$5$rounds=5000$steamedhamsusern$cztOQoktgCCanR2r/PIpGXFtHB13YV4yKUfKFAuZGkD', 0, 1, 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `userskins`
--

CREATE TABLE `userskins` (
  `userid` int(11) NOT NULL,
  `skinID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `userskins`
--

INSERT INTO `userskins` (`userid`, `skinID`) VALUES
(1, 1),
(1, 1),
(2, 1);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `friends`
--
ALTER TABLE `friends`
  ADD PRIMARY KEY (`ID`);

--
-- Indices de la tabla `skins`
--
ALTER TABLE `skins`
  ADD PRIMARY KEY (`skinID`);

--
-- Indices de la tabla `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `friends`
--
ALTER TABLE `friends`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `skins`
--
ALTER TABLE `skins`
  MODIFY `skinID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
