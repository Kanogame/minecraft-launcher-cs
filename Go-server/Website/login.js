"use strict"

const url = "http://192.168.1.8:8181"

var loginForm = document.getElementById("loginForm");

loginForm.addEventListener("submit", (e)=>{
    e.preventDefault();
    const data = new FormData(e.target);
    const bodyData = CreateLogJSON(data.get("Username"), data.get("Password"))
    SendLogin(bodyData);
});

async function SendLogin(bodyData) {
    await RequestType("login");
    await ServerRequest(bodyData);
}

async function RequestType(type) {
    let response = await fetch(url, {
        method: 'POST',
        body: type
    });
    let text = await response.text();
    console.log(text);
}

async function ServerRequest(bodyData) {
    let text = "";
    let response = await fetch(url, {
        method: 'POST',
        headers: {
        'Content-Type': 'application/json'
        },
        body: JSON.stringify(bodyData),
    });
    text = await response.text();
    if (text === "success") {
        console.log(text);
    }
    else {
        alert("false password");
    }
}

function CreateLogJSON (username, password) {
    return {
        name: username,
        password: password,
    }
}