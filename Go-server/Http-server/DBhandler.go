package Httpserver

import (
	"database/sql"
	"fmt"

	_ "github.com/go-sql-driver/mysql"
)

func FindDB(user string, password string) *sql.DB {
	db, err := sql.Open("mysql", user+":"+password+"@tcp(localhost:3306)/userdata")
	if err != nil {
		panic(err)
	}

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
	if err != nil {
		panic(err)
	}

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
	res, err := db.Query("SELECT loginkey FROM Users \nWHERE username = ?", name)
	if err != nil {
		panic(err)
	}

	var dbkey string

	for res.Next() {
		err := res.Scan(&dbkey)
		if err != nil {
			return false
		}
	}
	return dbkey != ""
}

func AddKey(db *sql.DB, name string, key string) {
	res, err := db.Exec("UPDATE Users SET loginkey = ? WHERE username = ?", key, name)
	if err != nil {
		panic(err)
	}
	fmt.Println(res)
}
