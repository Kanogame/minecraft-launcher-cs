package Httpserver

import (
	"database/sql"
	"fmt"

	_ "github.com/go-sql-driver/mysql"
)

func FindDB(user string, password string) {
	db, err := sql.Open("mysql", user+":"+password+"@tcp(localhost:3306)/userdata")
	if err != nil {
		panic(err)
	}

	defer db.Close()

	fmt.Println("подключено")

	GetUserByName(db, "baaanchic")
	/*
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
}

func GetUserByName(db *sql.DB, name string) {
	res, err := db.Query("SELECT username, passwrd, email, mcusername FROM Users \nWHERE username = '" + name + "';")
	if err != nil {
		panic(err)
	}

	for res.Next() {
		var user Userdata

		err := res.Scan(&user.Name, &user.Password, &user.Email, &user.MCusername)
		if err != nil {
			panic(err)
		}
		fmt.Println(user)
	}
}
