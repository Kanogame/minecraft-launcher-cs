package Httpserver

import (
	"fmt"
	"net/http"
	"strconv"
)

type UserRegData struct {
	Name       string `json:"name"`
	Email      string `json:"email"`
	Password   string `json:"password"`
	Mcusername string `json:"mcusername"`
}

type UserLogData struct {
	Name     string `json:"name"`
	Password string `json:"password"`
}

var postType = ""

func StartHttpServer(port int) {
	http.HandleFunc("/", HttpHandler)
	fmt.Println("Httpserver is up and listening on ", port)
	http.ListenAndServe(":"+strconv.Itoa(port), nil)
}

func HttpHandler(w http.ResponseWriter, r *http.Request) {
	if r.Method == "GET" {
		path := r.URL.Path

		if path == "/" {
			path = "./Website/"
		} else {
			path = "./Website/" + path
		}

		http.ServeFile(w, r, path)
	} else if r.Method == "POST" {
		postHandler(w, r)
	} else {
		fmt.Fprintf(w, "Request type other than GET or POSt not supported")
	}
}

func postHandler(w http.ResponseWriter, r *http.Request) {
	var db = FindDB("root", "password")
	if postType == "" {
		PostGetType(r)
		fmt.Fprintf(w, "success")
	} else if postType == "reg" {
		var json = PostRegJSON(r)
		var exist = RegNewUser(db, json)
		if exist {
			fmt.Fprintf(w, "success")
		} else {
			fmt.Fprintf(w, "alreadyexits")
		}
	} else if postType == "login" {
		var json = PostLogJSON(r)
		var exist = CheckPasswd(db, json)
		if exist {
			fmt.Fprintf(w, "success")
		} else {
			fmt.Fprintf(w, "FalsePassword")
		}
	}
}
