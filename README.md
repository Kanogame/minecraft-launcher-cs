# Custom Minecraft launcher with account system written in c#!
Minecraft launcher infustructute builded around Go backend for website hosting and c# file server

# Launcher 
C# WPF launcher can communicate with Go Backend, install versions of minecraft and download pre-made modpacks from C# file server.
![screenshot](https://i.imgur.com/sOTiSmB.png)

Uses [CmlLib.Core](https://github.com/CmlLib/CmlLib.Core)

Uses minestat
//in future

# GoLang Backend
launcher through the Go backend communicates with the mySQL database and gets the latest information about available modpacks and servers .Also GoLang Backend hosts a website on which you can register/login to the system, see the status of minecraft servers lists and much more.

# C# file srever
Just a simple and convenient host for the files needed to run minecraft
