package Launcher

import (
	"bufio"
	Httpserver "main/Http-server"
	"math/rand"
	"net"
	"os"
	"strconv"
)

func getServerList(conn net.Conn) {
	file, err1 := os.Open("./Configs/ServerList.txt")
	if err1 != nil {
		panic(err1)
	}

	var textFile []string
	scanner := bufio.NewScanner(file)

	i := 0
	for scanner.Scan() {
		textFile = append(textFile, scanner.Text())
		i++
	}
	var serverCount, err2 = strconv.Atoi(textFile[0])
	if err2 != nil {
		panic(err2)
	}

	var stringCount, err3 = strconv.Atoi(textFile[1])
	if err3 != nil {
		panic(err3)
	}

	writeInt(conn, serverCount)
	writeInt(conn, stringCount)
	for i := 0; i < serverCount; i++ {
		for j := 0; j < stringCount; j++ {
			writeString(conn, textFile[i*stringCount+j+2])
		}
	}
}

func getBackIp(conn net.Conn) {
	file, err := os.Open("./Configs/backIP.txt")
	if err != nil {
		panic(err)
	}

	scanner := bufio.NewScanner(file)

	i := 0
	var strRead = make([]string, 2)
	for scanner.Scan() {
		strRead[i] = scanner.Text()
		i++
	}
	writeString(conn, strRead[0])

	portI, err := strconv.Atoi(strRead[1])
	if err != nil {
		panic(err)
	}

	writeInt(conn, portI)
}

func verifyuser(conn net.Conn) {
	var data = readUserdata(conn)
	var db = Httpserver.FindDB("root", "password")
	var res = Httpserver.CheckPasswd(db, data)
	if res {
		writeInt(conn, 1)
	} else {
		writeInt(conn, 0)
	}
}

var letters = []rune("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ")

func randSeq(n int) string {
	b := make([]rune, n)
	for i := range b {
		b[i] = letters[rand.Intn(len(letters))]
	}
	return string(b)
}

func filecr(conn net.Conn) {
	var name = readString(conn)
	var db = Httpserver.FindDB("root", "password")
	if !Httpserver.KeyExist(db, name) {
		var key = randSeq(16)
		Httpserver.AddKey(db, name, key)
		writeString(conn, key)
	} else {
		writeString(conn, Httpserver.GetKey(db, name))
	}
	writeInt(Httpserver.GetId(db, name))
}
