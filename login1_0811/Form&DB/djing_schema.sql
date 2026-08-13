CREATE DATABASE djing;
USE djing;

-- 회원 테이블
CREATE TABLE member(
	member_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id VARCHAR(50) NOT NULL UNIQUE,-- user_name이 id임
    password VARCHAR(255) NOT NULL,
    name VARCHAR(50) NOT NULL,
    phone VARCHAR(20),
    join_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- 믹스(음원 게시물) 테이블
CREATE TABLE mix(
	mix_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id VARCHAR(50) NOT NULL,
    title VARCHAR(200) NOT NULL,
    description TEXT,
    genre VARCHAR(50),
    cover_image_path VARCHAR(255),
    file_path VARCHAR(255) NOT NULL,
    play_count INT DEFAULT 0,
    like_count INT DEFAULT 0,
    uploaded_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES member(user_id)
);
CREATE TABLE comment (
	comment_id INT AUTO_INCREMENT PRIMARY KEY,
    mix_id INT NOT NULL,
    user_id VARCHAR(50) NOT NULL,
    content TEXT NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (mix_id) REFERENCES mix(mix_id),
    FOREIGN KEY (user_id) REFERENCES member(user_id)
);

CREATE TABLE likes(
	like_id INT AUTO_INCREMENT PRIMARY KEY,
    mix_id INT NOT NULL,
    user_id VARCHAR(50) NOT NULL,
    UNIQUE KEY unique_like (mix_id,user_id),
    FOREIGN KEY (mix_id) REFERENCES mix(mix_id),
    FOREIGN KEY (user_id) REFERENCES member(user_id)
);

--   테스트 유저들임(없애도 되고 남겨도 되는 코드들) --
INSERT INTO member (user_id, password, name) VALUES ('test01', '1234', '테스트유저1');
INSERT INTO member (user_id, password, name) VALUES ('test02', '1234', '테스트유저2');

INSERT INTO mix (user_id, title, file_path) VALUES ('test01', '테스트 믹스1', 'test1.mp3');
INSERT INTO mix (user_id, title, file_path) VALUES ('test02', '테스트 믹스2', 'test2.mp3');


select * from member;
select * from comment;
select * from likes;
ALTER TABLE member ADD COLUMN profile_icon INT DEFAULT 1;
select user_id,profile_icon from member;