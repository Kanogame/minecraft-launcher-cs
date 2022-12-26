-- @block
CREATE TABLE Users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(255) NOT NULL UNIQUE,
    passwrd VARCHAR(255) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    mcusername VARCHAR(255) NOT NULL UNIQUE,
    loginkey VARCHAR(16) NOT NULL DEFAULT ''
)

-- @block 
INSERT INTO Users (username, passwrd, email, mcusername)
VALUES
    ("kanogames","1235", "wasdqert1222@gmail.com", "kanogames"),
    ("baaanchic","123asd5", "niggas@gmail.com", "banchic"),
    ("sashok","21", "setset@gmail.com", "wasda"),
    ("asdadad222","wddf3433vdvvvb565", "givememymoneybackyoudumbass@gmail.com", "kanogames12");

-- @block 
SELECT * FROM Users;

-- @block
DROP TABLE Users;

-- @block
CREATE TABLE Token (
    tokenid INT PRIMARY KEY AUTO_INCREMENT,
    token VARCHAR(255) NOT NULL,
    passhash VARCHAR(255) NOT NULL,
    id INT NOT NULL
)

-- @block
SELECT * FROM Token;

-- @block
DROP TABLE Token;