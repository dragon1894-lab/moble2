USE djing;

-- 곡 정보: 한 회원이 여러 곡을 올릴 수 있으므로 user_id에는 UNIQUE를 사용하지 않습니다.
CREATE TABLE IF NOT EXISTS songs (
    SongId INT NOT NULL AUTO_INCREMENT,
    user_id VARCHAR(50) NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Artist VARCHAR(255) NOT NULL,
    FilePath VARCHAR(500) DEFAULT NULL,
    ImagePath VARCHAR(500) DEFAULT NULL,
    LikeCount INT NOT NULL DEFAULT 0,
    ReleaseDate VARCHAR(50) DEFAULT NULL,
    PRIMARY KEY (SongId),
    INDEX idx_songs_user_id (user_id),
    CONSTRAINT fk_songs_member
        FOREIGN KEY (user_id) REFERENCES member(user_id)
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 사용자가 좋아요를 누른 곡
CREATE TABLE IF NOT EXISTS song_likes (
    user_id VARCHAR(50) NOT NULL,
    SongId INT NOT NULL,
    liked_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (user_id, SongId),
    INDEX idx_song_likes_song_id (SongId),
    CONSTRAINT fk_song_likes_member
        FOREIGN KEY (user_id) REFERENCES member(user_id)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT fk_song_likes_song
        FOREIGN KEY (SongId) REFERENCES songs(SongId)
        ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 사용자가 재생목록에 저장한 곡
CREATE TABLE IF NOT EXISTS playlist_songs (
    user_id VARCHAR(50) NOT NULL,
    SongId INT NOT NULL,
    added_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (user_id, SongId),
    INDEX idx_playlist_songs_song_id (SongId),
    CONSTRAINT fk_playlist_songs_member
        FOREIGN KEY (user_id) REFERENCES member(user_id)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT fk_playlist_songs_song
        FOREIGN KEY (SongId) REFERENCES songs(SongId)
        ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 곡에 작성한 댓글
CREATE TABLE IF NOT EXISTS comment (
    comment_id INT NOT NULL AUTO_INCREMENT,
    SongId INT NOT NULL,
    user_id VARCHAR(50) NOT NULL,
    content TEXT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (comment_id),
    INDEX idx_comment_song_id (SongId),
    INDEX idx_comment_user_id (user_id),
    CONSTRAINT fk_comment_song
        FOREIGN KEY (SongId) REFERENCES songs(SongId)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT fk_comment_member
        FOREIGN KEY (user_id) REFERENCES member(user_id)
        ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 댓글 좋아요
CREATE TABLE IF NOT EXISTS comment_like (
    comment_id INT NOT NULL,
    user_id VARCHAR(50) NOT NULL,
    PRIMARY KEY (comment_id, user_id),
    INDEX idx_comment_like_user_id (user_id),
    CONSTRAINT fk_comment_like_comment
        FOREIGN KEY (comment_id) REFERENCES comment(comment_id)
        ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT fk_comment_like_member
        FOREIGN KEY (user_id) REFERENCES member(user_id)
        ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

