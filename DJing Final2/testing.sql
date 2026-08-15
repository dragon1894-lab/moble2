-- CREATE TABLE member(
-- 	member_id INT AUTO_INCREMENT PRIMARY KEY,
--     user_id VARCHAR(50) NOT NULL UNIQUE,-- user_name이 id임
--     password VARCHAR(255) NOT NULL,
--     name VARCHAR(50) NOT NULL,
--     phone VARCHAR(20),
--     icon VARCHAR(50) DEFAULT NULL,
--     join_date DATETIME DEFAULT CURRENT_TIMESTAMP
-- );

-- select * from member;
-- ALTER TABLE member ADD COLUMN profile_icon INT DEFAULT 1;
-- select user_id,profile_icon from member;

-- CREATE TABLE `songs` (
--     `SongId` INT NOT NULL AUTO_INCREMENT,
--     `user_id` VARCHAR(50) NOT NULL UNIQUE,
--     `Title` VARCHAR(255) NOT NULL,
--     `Artist` VARCHAR(255) NOT NULL,
--     `FilePath` VARCHAR(500) DEFAULT NULL,
--     `ImagePath` VARCHAR(500) DEFAULT NULL,
--     `LikeCount` INT DEFAULT 0,
--     `ReleaseDate` VARCHAR(50) DEFAULT NULL,
--     PRIMARY KEY (`SongId`)
-- ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- CREATE TABLE song_likes
-- (
--     user_id VARCHAR(50) NOT NULL,
--     SongId INT NOT NULL,
--     liked_at DATETIME DEFAULT CURRENT_TIMESTAMP,

--     PRIMARY KEY (user_id, SongId),

--     FOREIGN KEY (user_id) REFERENCES member(user_id),
--     FOREIGN KEY (SongId) REFERENCES songs(SongId)
-- );

-- CREATE TABLE playlist_songs
-- (
--     user_id VARCHAR(50) NOT NULL,
--     SongId INT NOT NULL,
--     added_at DATETIME DEFAULT CURRENT_TIMESTAMP,

--     PRIMARY KEY (user_id, SongId),

--     FOREIGN KEY (user_id) REFERENCES member(user_id),
--     FOREIGN KEY (SongId) REFERENCES songs(SongId)
-- );

CREATE TABLE comment (
    comment_id INT AUTO_INCREMENT PRIMARY KEY,
    SongId INT NOT NULL,
    user_id VARCHAR(50) NOT NULL,
    content TEXT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,

    FOREIGN KEY (SongId) REFERENCES songs(SongId),
    FOREIGN KEY (user_id) REFERENCES member(user_id)
);

CREATE TABLE comment_like (
    comment_id INT NOT NULL,
    user_id VARCHAR(50) NOT NULL,

    PRIMARY KEY (comment_id, user_id),

    FOREIGN KEY (comment_id)
        REFERENCES comment(comment_id)
        ON DELETE CASCADE,

    FOREIGN KEY (user_id)
        REFERENCES member(user_id)
);