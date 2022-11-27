package Httpserver

import (
	"fmt"
	"net/http"
	"strconv"
)

func StartHttpServer(port int) {
	http.HandleFunc("/", PostData)
	fmt.Println("Httpserver is up and listening on ", port)
	http.ListenAndServe(":"+strconv.Itoa(port), nil)
}

func PostData(w http.ResponseWriter, r *http.Request) {
	switch r.Method {

	case "GET":
		path := r.URL.Path
		if path == "/" {
			path = "./Website/"
		} else {
			path = "./Website/" + path
		}

		http.ServeFile(w, r, path)
	case "POST":
		fmt.Println(PostRead(r))
		fmt.Fprintf(w, "Server: %s \n", "text")
	default:
		fmt.Fprintf(w, "Request type other than GET or POSt not supported")

	}
}

func PostRead(r *http.Request) string {
	r.ParseMultipartForm(0)
	message := r.FormValue("message")
	return message
}

func PostWrite(w http.ResponseWriter, text string) {
	fmt.Println("sending to web: " + text)

}
