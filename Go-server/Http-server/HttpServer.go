package Httpserver

import (
	"fmt"
	"net/http"
	"strconv"
	"time"
)

func StartHttpServer(port int) {
	http.HandleFunc("/", PostData)
	fmt.Println("Httpserver is up and listening on ", port)
	http.ListenAndServe(":"+strconv.Itoa(port), nil)
	http.FileServer(http.Dir("./Website/"))
}

func PostData(w http.ResponseWriter, r *http.Request) {
	switch r.Method {

	case "GET":

		path := r.URL.Path

		fmt.Println(path)

		if path == "/" {

			path = "./Website/"
		} else {

			path = "." + path
		}

		http.ServeFile(w, r, path)

	case "POST":

		r.ParseMultipartForm(0)

		message := r.FormValue("message")

		fmt.Println("----------------------------------")
		fmt.Println("Message from Client: ", message)
		// respond to client's request
		fmt.Fprintf(w, "Server: %s \n", message+" | "+time.Now().Format(time.RFC3339))

	default:
		fmt.Fprintf(w, "Request type other than GET or POSt not supported")

	}
}
