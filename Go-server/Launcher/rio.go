package Launcher

import (
	"encoding/binary"
	Httpserver "main/Http-server"
	"net"
)

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

func writeInt64(conn net.Conn, val int64) {
	var bytes = make([]byte, 8)
	binary.LittleEndian.PutUint64(bytes[0:8], uint64(val))
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

func readUserdata(conn net.Conn) Httpserver.UserLogData {
	var name = readString(conn)
	var password = readString(conn)
	var res Httpserver.UserLogData
	res.Name = name
	res.Password = password
	return res
}
