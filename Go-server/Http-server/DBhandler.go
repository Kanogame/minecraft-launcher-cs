package Httpserver

import (
	"database/sql"
	"fmt"
	utils "main/Utils"

	_ "github.com/go-sql-driver/mysql"
)

func errorHandler(err error) {
	if err != nil {
		panic(err)
	}
}

func FindDB(user string, password string) *sql.DB {
	db, err := sql.Open("mysql", user+":"+password+"@tcp(localhost:3306)/userdata")
	errorHandler(err)

	fmt.Println("подключено")
	return db
}

func RegNewUser(db *sql.DB, data UserRegData) bool {
	res, err := db.Exec(fmt.Sprintf("INSERT INTO Users (username, passwrd, email, mcusername) VALUES('%v', '%v', '%v', '%v')", data.Name, data.Password, data.Email, data.Mcusername))
	if err != nil {
		return false
	}
	fmt.Println(res)
	return true
}

func CheckPasswd(db *sql.DB, logData UserLogData) bool {
	res, err := db.Query("SELECT username, passwrd FROM Users \nWHERE username = ?", logData.Name)
	errorHandler(err)

	var user UserLogData

	for res.Next() {

		err := res.Scan(&user.Name, &user.Password)
		if err != nil {
			return false
		}
	}

	if logData.Password == user.Password {
		return true
	} else {
		return false
	}
}

func KeyExist(db *sql.DB, name string) bool {
	var dbkey = GetKey(db, name)
	return dbkey != ""
}

func AddKey(db *sql.DB, name string, key string) {
	res, err := db.Exec("UPDATE Users SET loginkey = ? WHERE username = ?", key, name)
	errorHandler(err)
	fmt.Println(res)
}

func GetKey(db *sql.DB, name string) string {
	res, err := db.Query("SELECT loginkey FROM Users \nWHERE username = ?", name)
	errorHandler(err)
	var dbkey string

	for res.Next() {
		err := res.Scan(&dbkey)
		errorHandler(err)
	}
	fmt.Println("key is ", dbkey)
	return dbkey
}

func GetId(db *sql.DB, name string) int {
	res, err := db.Query("SELECT id FROM Users \nWHERE username = ?", name)
	errorHandler(err)
	var dbid int

	for res.Next() {
		err := res.Scan(&dbid)
		errorHandler(err)
	}
	fmt.Println("id is ", dbid)
	return dbid
}

func GetNameByID(db *sql.DB, id int) string {
	res, err := db.Query("SELECT username FROM Users \nWHERE id = ?", id)
	errorHandler(err)
	var name string

	for res.Next() {
		err := res.Scan(&name)
		errorHandler(err)
	}
	return name
}

func GetIdByToken(db *sql.DB, id int) string {
	res, err := db.Query("SELECT token FROM Token \nWHERE id = ?", id)
	errorHandler(err)

	var token string
	for res.Next() {
		err := res.Scan(&token)
		errorHandler(err)
	}
	return token
}

func GetTokenById(db *sql.DB, token string) int {
	res, err := db.Query("SELECT id FROM Token \nWHERE token = ?", token)
	errorHandler(err)

	var id int
	for res.Next() {
		err := res.Scan(&id)
		errorHandler(err)
	}
	return id
}

func GetMCnickByID(db *sql.DB, id int) string {
	res, err := db.Query("SELECT mcusername FROM Users \nWHERE id = ?", id)
	errorHandler(err)
	var mcname string

	for res.Next() {
		err := res.Scan(&mcname)
		errorHandler(err)
	}
	return mcname
}

func AddToken(db *sql.DB, id int, data utils.TokenData) {
	fmt.Println(data, id)
	res, err := db.Exec(fmt.Sprintf("INSERT INTO Token (token, passhash, id) VALUES('%v', '%v', '%v')", data.Token, data.Passhash, id))
	errorHandler(err)
	fmt.Println(res)
}

func VerifyToken(db *sql.DB, data utils.TokenData) bool {
	res, err := db.Query("SELECT passhash FROM Token \nWHERE token = ?", data.Token)
	if err != nil {
		return false
	}

	var passhash string
	for res.Next() {
		err := res.Scan(&passhash)
		errorHandler(err)
	}
	return data.Passhash == passhash[:len(passhash)-1]
}
