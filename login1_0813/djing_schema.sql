CREATE DATABASE djing;
USE djing;

CREATE TABLE member(
	member_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id VARCHAR(50) NOT NULL UNIQUE,-- user_name이 id임
    password VARCHAR(255) NOT NULL,
    name VARCHAR(50) NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(100),
    join_date DATETIME DEFAULT CURRENT_TIMESTAMP
);

select * from member;
ALTER TABLE member ADD COLUMN profile_icon INT DEFAULT 1;
select user_id,profile_icon from member;