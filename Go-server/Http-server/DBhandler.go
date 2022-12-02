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
	db.Exec("INSERT INTO Users (username, passwrd, email, mcusername) VALUES('asd', 'asd', 'asd', 'asd')")
	return db
}

func RegNewUser(db *sql.DB, data Userdata) bool {
	res, err := db.Exec(fmt.Sprintf("INSERT INTO Users (username, passwrd, email, mcusername) VALUES('%v', '%v', '%v', '%v')", data.Name, data.Password, data.Email, data.Mcusername))
	if err != nil {
		fmt.Println(err)
		return false
	}
	fmt.Println(res)
	return true
}

func GetUserByName(db *sql.DB, name string) Userdata {
	res, err := db.Query("SELECT username, passwrd, email, mcusername FROM Users \nWHERE username = '" + name + "';")
	if err != nil {
		panic(err)
	}

	var user Userdata

	for res.Next() {

		err := res.Scan(&user.Name, &user.Password, &user.Email, &user.Mcusername)
		if err != nil {
			user.Name = ""
			return user
		}
		fmt.Println(user)
	}
	return user
}
