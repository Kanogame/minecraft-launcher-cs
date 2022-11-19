package main

import (
	"bufio"
	"encoding/binary"
	"fmt"
	"net"
	"os"
	"strconv"
)

func main() {
	HandleServer()
}

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
	/*
		port := scanner.Text()
		portI, err := strconv.Atoi(port)
		if err != nil {
			panic(err)
		}

		fmt.Println(portI)

		writeInt(conn, portI)
		/*
			count := scanner.Text()
			countI, err := strconv.Atoi(count)
			if err != nil {
				panic(err)
			}
			writeInt(conn, countI)
			for i := 0; i < countI; i++ {
				serverName := scanner.Text()
				writeString(conn, serverName)
				serverIP := scanner.Text()
				writeString(conn, serverIP)
				serverPort := scanner.Text()
				writeString(conn, serverPort)
				serverDesc := scanner.Text()
				writeString(conn, serverDesc)
			}
	*/
}

func writeString(conn net.Conn, val string) {
	var bytes = []byte(val)
	writeInt(conn, len(bytes))
	conn.Write(bytes)
}

func writeInt(conn net.Conn, val int) {
	var bytes = make([]byte, 4)
	binary.LittleEndian.PutUint32(bytes[0:4], uint32(val))
	conn.Write(bytes)
}

func readInt(conn net.Conn) int {
	var bytes = make([]byte, 4)
	conn.Read(bytes)
	res := binary.LittleEndian.Uint32(bytes)
	return int(res)
}

func readString(conn net.Conn) string {
	var len = readInt(conn)
	var bytes = make([]byte, len)
	conn.Read(bytes)
	return string(bytes)
}
