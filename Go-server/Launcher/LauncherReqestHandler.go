package Launcher

import (
	"bufio"
	"fmt"
	"net"
	"os"
)

func HandleServer() {
	fmt.Println("Launching server...")
	var dbargs = GetDBArgs()
	fmt.Println("Server succsesfully launched, serving on port 8081")

	ln, _ := net.Listen("tcp", "localhost:8081")

	defer ln.Close()

	for {
		conn, err := ln.Accept()
		if err != nil {
			fmt.Println("Error accepting: ", err.Error())
		}
		request := readString(conn)
		fmt.Println(request)
		switch request {
		case "getbackip":
			getBackIp(conn)
		case "getserverlist":
			getServerList(conn)
		case "verifyuser":
			verifyuser(conn, dbargs)
		case "filecr":
			filecr(conn, dbargs)
		case "decrypt":
			decrypt(conn, dbargs)
		case "images":
			imageHandler(conn)
		case "getmcname":
			getMCN(conn, dbargs)
		}
		conn.Close()
	}
}

func GetDBArgs() (dbargs string) {
	file, err := os.Open("./Configs/DataBase.txt")
	if err != nil {
		panic(err)
	}

	scanner := bufio.NewScanner(file)

	var strRead string
	for scanner.Scan() {
		strRead = scanner.Text()
	}

	return strRead
}
