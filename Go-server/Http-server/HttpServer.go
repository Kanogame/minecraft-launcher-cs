package Httpserver

import (
	"encoding/json"
	"fmt"
	"io"
	"net/http"
	"strconv"
)

type Userdata struct {
	Name       string `json:"name"`
	Email      string `json:"email"`
	Password   string `json:"password"`
	Mcusername string `json:"mcusername"`
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
		var json = PostJSON(r)
		var exist = RegNewUser(db, json)
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
	} else {
		fmt.Println("error")
	}
}

func PostJSON(r *http.Request) Userdata {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		panic(err)
	}
	var post Userdata
	err = json.Unmarshal(body, &post)
	if err != nil {
		panic(err)
	}
	fmt.Println(post)
	postType = ""
	return post
}
