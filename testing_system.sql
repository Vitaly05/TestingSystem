-- phpMyAdmin SQL Dump
-- version 5.1.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Oct 16, 2022 at 05:05 PM
-- Server version: 5.7.24
-- PHP Version: 8.0.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `testing_system`
--

-- --------------------------------------------------------

--
-- Table structure for table `questions`
--

CREATE TABLE `questions` (
  `id` int(10) UNSIGNED NOT NULL,
  `test_id` int(10) UNSIGNED NOT NULL,
  `data` varchar(2000) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `questions`
--

INSERT INTO `questions` (`id`, `test_id`, `data`) VALUES
(4, 5, '{\"Questions\":[\"1\",\"2\",\"1\",\"2\"],\"Answers\":[\"1\",\"2\",\"1\",\"2\"],\"QuestionTypes\":[\"0\",\"1\",\"1\",\"1\"],\"IncorrectAnswers\":[[null,null,null],[\"1\",null,null],[\"2\",null,null],[\"1\",null,null]],\"TimerOn\":true,\"Minutes\":1,\"Seconds\":5}'),
(12, 13, '{\"Questions\":[\"1\",\"2\"],\"Answers\":[\"1\",\"2\"],\"QuestionTypes\":[\"0\",\"1\"],\"IncorrectAnswers\":[[null,null,null],[\"1\",null,null]],\"TimerOn\":false,\"Minutes\":0,\"Seconds\":0}');

-- --------------------------------------------------------

--
-- Table structure for table `tests`
--

CREATE TABLE `tests` (
  `id` int(11) UNSIGNED NOT NULL,
  `teacher_id` int(11) UNSIGNED NOT NULL,
  `name` varchar(45) NOT NULL,
  `description` varchar(200) DEFAULT NULL,
  `max_mark` int(11) NOT NULL,
  `create_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `tests`
--

INSERT INTO `tests` (`id`, `teacher_id`, `name`, `description`, `max_mark`, `create_date`) VALUES
(5, 43, '2', 'With timer', 2, '2022-10-15'),
(13, 43, '1', NULL, 1, '2022-10-16');

-- --------------------------------------------------------

--
-- Table structure for table `tests_results`
--

CREATE TABLE `tests_results` (
  `id` int(10) UNSIGNED NOT NULL,
  `test_id` int(10) UNSIGNED NOT NULL,
  `user_id` int(10) UNSIGNED NOT NULL,
  `mark` double UNSIGNED DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `tests_results`
--

INSERT INTO `tests_results` (`id`, `test_id`, `user_id`, `mark`) VALUES
(8, 5, 44, 0);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(10) UNSIGNED NOT NULL,
  `login` varchar(20) NOT NULL,
  `password` varchar(20) NOT NULL,
  `type` varchar(7) NOT NULL,
  `name` varchar(15) NOT NULL,
  `surname` varchar(15) NOT NULL,
  `patronymic` varchar(20) NOT NULL,
  `group` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `login`, `password`, `type`, `name`, `surname`, `patronymic`, `group`) VALUES
(1, 'admin', 'admin', 'Admin', 'admin', 'admin', 'admin', NULL),
(2, 'teacher', 'teacher', 'Teacher', 'teacher', 'teacher', 'teacher', NULL),
(42, '1', '1', 'Admin', '1', '1', '1', NULL),
(43, '2', '2', 'Teacher', '2', '2', '2', NULL),
(44, '3', '3', 'Student', '2', '1', '3', '0K9392'),
(45, 'loz', 'loz', 'Student', 'Виталий', 'Лозюк', 'Александрович', '0K9392'),
(48, 'student', 'student', 'Student', 'student', 'student', 'student', '0K9392'),
(49, '1111', '1111', 'Teacher', '1111', '11111', '1111', NULL),
(51, '31231', '23123123', 'Student', '32131', '12312', '2343234', '1'),
(53, 'апрап', 'раврав', 'Student', '456456', '456ркарапрапв', '345456456', '1'),
(54, 'апрвеырцы', 'рпрвору', 'Student', 'уекрпаору5вн', 'кре5етепр', 'раро56еркор', '1'),
(55, 'рвапрв', 'патпаркпрвкркп', 'Student', 'рапрке', 'рыпкркап', 'ркерпрпр', '1');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `questions`
--
ALTER TABLE `questions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `test_id` (`test_id`);

--
-- Indexes for table `tests`
--
ALTER TABLE `tests`
  ADD PRIMARY KEY (`id`),
  ADD KEY `teacher_id` (`teacher_id`);

--
-- Indexes for table `tests_results`
--
ALTER TABLE `tests_results`
  ADD PRIMARY KEY (`id`),
  ADD KEY `test_id` (`test_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id_UNIQUE` (`id`),
  ADD UNIQUE KEY `login_UNIQUE` (`login`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `questions`
--
ALTER TABLE `questions`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `tests`
--
ALTER TABLE `tests`
  MODIFY `id` int(11) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `tests_results`
--
ALTER TABLE `tests_results`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=56;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `questions`
--
ALTER TABLE `questions`
  ADD CONSTRAINT `questions_ibfk_1` FOREIGN KEY (`test_id`) REFERENCES `tests` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tests`
--
ALTER TABLE `tests`
  ADD CONSTRAINT `tests_ibfk_1` FOREIGN KEY (`teacher_id`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tests_results`
--
ALTER TABLE `tests_results`
  ADD CONSTRAINT `tests_results_ibfk_1` FOREIGN KEY (`test_id`) REFERENCES `tests` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tests_results_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
