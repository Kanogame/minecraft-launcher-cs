"use strict"

const loginbutton = document.getElementById("login");
const resr = document.getElementById("resr");

loginbutton.addEventListener("click", function () {
    let data = {
        Name: value,
        Time: new Date().toLocaleString("en-IE"),
    };
    fetch("/get_time", {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        method: "POST",
        body: JSON.stringify(data)
    });
})