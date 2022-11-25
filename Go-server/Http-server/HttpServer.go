package Httpserver

import (
	"fmt"
	"net/http"
	"strconv"
)

func StartHttpServer(port int) {
	fmt.Println("Httpserver is up and listening on ", port)
	http.ListenAndServe(":"+strconv.Itoa(port), http.FileServer(http.Dir("./Website/")))
	http.HandleFunc("/get_time", urlHandler)
}

func urlHandler(rpW http.ResponseWriter, request *http.Request) {
	fmt.Println("getted")
}
