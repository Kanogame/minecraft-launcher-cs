package Launcher

import (
	"bufio"
	"fmt"
	"net"
	"os"
	"strconv"
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
		if request == "getbackip" {
			ReadNearestConf(conn)
		}

		conn.Close()
	}
}

func ReadNearestConf(conn net.Conn) {
	file, err := os.Open("./config.txt")
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
