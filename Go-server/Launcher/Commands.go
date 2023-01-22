package Launcher

import (
	"bufio"
	"crypto/sha1"
	"database/sql"
	"fmt"
	"io"
	Httpserver "main/Http-server"
	Utils "main/Utils"
	"net"
	"os"
	"strconv"
)

func verifyToken(conn net.Conn, db *sql.DB) (bool, int) {
	var token = readString(conn)
	var passhash = readString(conn)
	fmt.Println("new token created")
	var data = Utils.TokenData{
		Token:    token,
		Passhash: passhash,
	}
	if Httpserver.VerifyToken(db, data) {
		writeInt(conn, 1)
		return true, Httpserver.GetTokenById(db, token)
	} else {
		writeInt(conn, 0)
		return false, -1
	}
}

func addToken(conn net.Conn, db *sql.DB, id int) {
	var token = Utils.RandSeq(64)
	var tokenPassword = Utils.RandSeq(16)
	passhash := sha1.Sum([]byte(tokenPassword))
	var data = Utils.TokenData{
		Token:    token,
		Passhash: fmt.Sprintf("%x\n", passhash),
	}
	Httpserver.AddToken(db, id, data)
	writeString(conn, token)
	writeString(conn, tokenPassword)
}

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
	serverCount, err := strconv.Atoi(textFile[0])
	if err != nil {
		panic(err)
	}

	stringCount, err := strconv.Atoi(textFile[1])
	if err != nil {
		panic(err)
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
		addToken(conn, db, Httpserver.GetId(db, data.Name))
	} else {
		fmt.Println("user error")
		writeInt(conn, 0)
	}
}

func getMCN(conn net.Conn) {
	var db = Httpserver.FindDB("root", "password")
	vt, id := verifyToken(conn, db)
	if vt {
		writeString(conn, Httpserver.GetMCnickByID(db, id))
	}
}

func filecr(conn net.Conn) {
	var db = Httpserver.FindDB("root", "password")
	vt, id := verifyToken(conn, db)
	if vt {
		var name = readString(conn)
		if !Httpserver.KeyExist(db, name) {
			var key = Utils.RandSeq(16)
			Httpserver.AddKey(db, name, key)
			writeString(conn, key)
		} else {
			writeString(conn, Httpserver.GetKey(db, name))
		}
		writeInt(conn, Httpserver.GetId(db, name))
		fmt.Println("user file crypted", id)
	}
}

func decrypt(conn net.Conn) {
	var db = Httpserver.FindDB("root", "password")
	var data = readString(conn)
	var id, encpass = Utils.ParseData(data)
	var userdata Httpserver.UserLogData
	userdata.Name = Httpserver.GetNameByID(db, id)
	userdata.Password = Utils.DecryptPW(encpass, Httpserver.GetKey(db, userdata.Name))
	if Httpserver.CheckPasswd(db, userdata) {
		fmt.Println("user logged in")
		writeInt(conn, 1)
	} else {
		fmt.Println("user error")
		writeInt(conn, 0)
	}
}

func imageHandler(conn net.Conn) {

	var filePath = "./Configs/Images/images.zip"
	file, err := os.Open(filePath)
	if err != nil {
		panic(err)
	}

	fi, err := file.Stat()
	if err != nil {
		panic(err)
	}

	writeInt64(conn, fi.Size())
	writeString(conn, "images.zip")

	defer file.Close()
	buf := make([]byte, 1024)

	for {
		_, err = file.Read(buf)
		if err == io.EOF {
			break
		}
		conn.Write(buf)
	}
}
