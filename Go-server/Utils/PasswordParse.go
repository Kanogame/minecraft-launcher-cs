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
	fmt.Println("new key is ", string(b))
	return string(b)
}

func passLateParse(password string) (res string) {
	for i := 0; i < len(password); i++ {
		if string(password[i]) != " " {
			fmt.Println(string(password[i]) != " ")
			fmt.Println(string(password[i]))
			res += string(password[i])
		}
	}
	return res
}
