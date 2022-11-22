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
			getBackIp(conn)
		} else if request == "getserverlist" {
			getServerList(conn)
		}

		conn.Close()
	}
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
