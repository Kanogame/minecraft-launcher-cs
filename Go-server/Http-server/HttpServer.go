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
	MCusername string `json:"mcusername"`
}

func StartHttpServer(port int) {
	FindDB("root", "password")
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
	var NewUserdata = PostJSON(r)
	fmt.Println(NewUserdata)
	fmt.Fprintf(w, "success")
}

func PostJSON(r *http.Request) Userdata {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		panic(err)
	}
	var t Userdata
	err = json.Unmarshal(body, &t)
	if err != nil {
		panic(err)
	}
	return t
}

func PostWrite(w http.ResponseWriter, text string) {
	fmt.Println("sending to web: " + text)
}
