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

type PostType struct {
	Type string `json:"type"`
	Test string `json:"test"`
}

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
	//var db = FindDB("root", "password")
	//defer db.Close()
	PostGetType(r)
	fmt.Fprintf(w, "success")
	PostJSON(r)
	fmt.Fprintf(w, "success")
	//fmt.Fprintf(w, "success")
	//fmt.Println(NewUserdata)
}

func PostGetType(r *http.Request) PostType {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		panic(err)
	}
	var post PostType
	err = json.Unmarshal(body, &post)
	if err != nil {
		panic(err)
	}
	fmt.Println(string(body))
	fmt.Println(post)
	return post
}

func PostJSON(r *http.Request) {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		panic(err)
	}
	//var post Userdata
	//err = json.Unmarshal(body, &post)
	//if err != nil {
	//	fmt.Println(err)
	//}
	fmt.Println(string(body))
	//return t
}
