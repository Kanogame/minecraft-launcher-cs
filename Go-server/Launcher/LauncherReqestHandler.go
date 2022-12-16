package Launcher

import (
	"fmt"
	"net"
)

func HandleServer() {
	fmt.Println("Launching server...")

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
			verifyuser(conn)
		case "filecr":
			filecr(conn)
		}
		conn.Close()
	}
}
