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
	/*
		var userVal = GetUserByName(db, "baaanchic")
		if ()

		res, err := db.Query("SELECT * FROM Users")
		if err != nil {
			panic(err)
		}

		for res.Next() {
			var user Userdata
			var id int

			err := res.Scan(&id, &user.Name, &user.Password, &user.Email, &user.MCusername)

			if err != nil {
				panic(err)
			}
			fmt.Print(id)
			fmt.Println(user)
		}
	*/
	return db
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
			user.Name = "nil"
			return user
		}
		fmt.Println(user)
	}
	return user
}
