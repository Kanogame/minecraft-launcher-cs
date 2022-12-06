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
