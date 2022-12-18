package Launcher

import (
	"bufio"
	"crypto/rc4"
	"encoding/base64"
	"fmt"
	"log"
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
		fmt.Println("user logged in")
		writeInt(conn, 1)
	} else {
		fmt.Println("user error")
		writeInt(conn, 0)
	}
}

var letters = []rune("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ")

func randSeq(n int) string {
	b := make([]rune, n)
	for i := range b {
		b[i] = letters[rand.Intn(len(letters))]
	}
	fmt.Println("new key is ", b)
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
	writeInt(conn, Httpserver.GetId(db, name))
	fmt.Println("done")
}

func decrypt(conn net.Conn) {
	var data = readString(conn)
	fmt.Println(data)
	var id, encpass = parseData(data)
	var db = Httpserver.FindDB("root", "password")
	var userdata Httpserver.UserLogData
	userdata.Name = Httpserver.GetNameByID(db, id)
	userdata.Password = decryptPW(encpass, Httpserver.GetKey(db, userdata.Name))
	fmt.Println(userdata.Name, userdata.Password)
	if Httpserver.CheckPasswd(db, userdata) {
		fmt.Println("user logged in")
		writeInt(conn, 1)
	} else {
		fmt.Println("user error")
		writeInt(conn, 0)
	}
}

func parseData(data string) (id int, passwd string) {
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

func decryptPW(passwd string, key string) string {
	data, err := base64.StdEncoding.DecodeString(passwd)
	if err != nil {
		log.Fatal("error:", err)
	}

	c, err := rc4.NewCipher([]byte(key))
	if err != nil {
		panic(err)
	}

	var src2 []byte
	c.XORKeyStream(src2, data)
	fmt.Println("Plaintext': ", src2)
	return string(src2)
}

func Encrypt(pwd []byte, data []byte) []byte {
	var a, i, j, k, tmp int

	var key = make([]int, 256)
	var box = make([]int, 256)
	var cipher = make([]byte, len(data))

	for i = 0; i < 256; i++ {
		key[i] = pwd[i%len(pwd)]
		box[i] = i
	}
	i = 0
	for j = 0; i < 256; i++ {
		j = (j + box[i] + key[i]) % 256
		tmp = box[i]
		box[i] = box[j]
		box[j] = tmp
	}
	i = 0
	j = 0
	for a = 0; i < len(data); i++ {
		a++
		a %= 256
		j += box[a]
		j %= 256
		tmp = box[a]
		box[a] = box[j]
		box[j] = tmp
		k = box[((box[a] + box[j]) % 256)]
		cipher[i] = byte(data[i] * *k)
	}
	return cipher
}
