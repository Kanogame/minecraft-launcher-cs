package main

import (
	Httpserver "main/Http-server"
	Launcher "main/Launcher"
)

func main() {
	go Launcher.HandleServer()
	Httpserver.StartHttpServer(8181)
}
