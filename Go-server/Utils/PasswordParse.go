package Utils

import (
	"fmt"
	"math/rand"
	"strconv"
)

func ParseData(data string) (id int, passwd string) {
	var ids string
	var i = 0
	for i < len(data) {
		if string(data[i]) == "-" {
			break
		}
		ids += string(data[i])
		i++
	}

	for i < len(data) {
		if !(string(data[i]) == "-") {
			passwd += string(data[i])
		}
		i++
	}
	id, err := strconv.Atoi(ids)
	if err != nil {
		panic(err)
	}

	return
}

var letters = []rune("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ")

func RandSeq(n int) string {
	b := make([]rune, n)
	for i := range b {
		b[i] = letters[rand.Intn(len(letters))]
	}
	fmt.Println("new key is ", b)
	return string(b)
}
