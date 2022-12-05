package Httpserver

import (
	"encoding/json"
	"fmt"
	"io"
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
		fmt.Println(json)
		var exist = RegNewUser(db, json)
		if exist {
			fmt.Fprintf(w, "success")
		} else {
			fmt.Fprintf(w, "alreadyexits")
		}
	} else if postType == "login" {
		var json = PostLogJSON(r)
		fmt.Println(json)
		var exist = CheckPasswd(db, json)
		if exist {
			fmt.Fprintf(w, "success")
		} else {
			fmt.Fprintf(w, "alreadyexits")
		}
	}
}

func PostGetType(r *http.Request) {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		panic(err)
	}
	var res = string(body)
	if res == "reg" {
		postType = "reg"
	} else if res == "login" {
		postType = "login"
	} else {
		fmt.Println("error")
	}
}

func PostRegJSON(r *http.Request) UserRegData {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		panic(err)
	}
	var post UserRegData
	err = json.Unmarshal(body, &post)
	if err != nil {
		fmt.Println(body)
		panic(err)
	}
	fmt.Println(post)
	postType = ""
	return post
}

func PostLogJSON(r *http.Request) UserLogData {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		panic(err)
	}
	var post UserLogData
	err = json.Unmarshal(body, &post)
	if err != nil {
		fmt.Println(body)
		panic(err)
	}
	fmt.Println(post)
	postType = ""
	return post
}
