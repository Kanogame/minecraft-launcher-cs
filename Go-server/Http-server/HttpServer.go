package Httpserver

import (
	"encoding/json"
	"fmt"
	"io"
	"net/http"
	"strconv"
)

func StartHttpServer(port int) {
	http.HandleFunc("/", PostData)
	fmt.Println("Httpserver is up and listening on ", port)
	http.ListenAndServe(":"+strconv.Itoa(port), nil)
}

func PostData(w http.ResponseWriter, r *http.Request) {
	if r.Method == "GET" {
		path := r.URL.Path

		if path == "/" {
			path = "./Website/"
		} else {
			path = "./Website/" + path
		}

		http.ServeFile(w, r, path)
	} else if r.Method == "POST" {
		fmt.Println(PostRead(r))
		fmt.Fprintf(w, "Server: %s \n", "text")
	} else {
		fmt.Fprintf(w, "Request type other than GET or POSt not supported")
	}
}

type bodys struct {
	Name string `json:"name"`
	Age  int    `json:"age"`
}

func PostRead(r *http.Request) string {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		panic(err)
	}
	fmt.Println(string(body))
	var t bodys
	err = json.Unmarshal(body, &t)
	if err != nil {
		panic(err)
	}
	fmt.Println(t.Name)
	fmt.Println(t.Age)
	return ""
}

func PostWrite(w http.ResponseWriter, text string) {
	fmt.Println("sending to web: " + text)

}
