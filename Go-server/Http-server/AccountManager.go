package Httpserver

import (
	"encoding/json"
	"fmt"
	"io"
	"net/http"
)

func PostGetType(r *http.Request) {
	body, err := io.ReadAll(r.Body)
	if err != nil {
		panic(err)
	}
	var res = string(body)
	if res == "reg" || res == "login" {
		postType = res
	} else {
		fmt.Println("error while handling GetType")
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
		panic(err)
	}
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
		panic(err)
	}
	postType = ""
	return post
}
