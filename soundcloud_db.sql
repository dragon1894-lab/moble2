-- 1. 스키마(데이터베이스) 생성 및 선택
CREATE DATABASE IF NOT EXISTS `soundcloud_db` 
DEFAULT CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE `soundcloud_db`;

-- 2. Songs 테이블 생성 (컬럼 구조 정의)
CREATE TABLE IF NOT EXISTS `Songs` (
    `SongId`      INT AUTO_INCREMENT PRIMARY KEY  COMMENT '곡 고유 ID',
    `Title`       VARCHAR(255) NOT NULL           COMMENT '곡 제목',
    `Artist`      VARCHAR(255) NOT NULL           COMMENT '아티스트명',
    `FilePath`    VARCHAR(500)                    COMMENT '음원 파일 경로',
    `ImagePath`   VARCHAR(500)                    COMMENT '커버 이미지 경로',
    `LikeCount`   INT DEFAULT 0                   COMMENT '좋아요 수',
    `ReleaseDate` VARCHAR(50)                     COMMENT '발매일'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 3. 실제 데이터 삽입
INSERT INTO Songs (Title, Artist, FilePath, ImagePath, LikeCount, ReleaseDate)
VALUES (
    '칠색조 배틀 BGM', 
    '포켓몬스터', 
    'C:\\Users\\모블\\Desktop\\music\\HoohBattle.mp3', 
    'C:\\Users\\모블\\Desktop\\music\\WaitBase.png', 
    999, 
    '2026-08-11'
);