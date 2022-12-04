"use strict"

const url = "http://192.168.1.8:8181"

var inputForm = document.getElementById("loginForm");

inputForm.addEventListener("submit", (e)=>{
    e.preventDefault()
    const data = new FormData(e.target);
    const bodyData = CreateJSON(data.get("Username"), data.get("Password"))
    SendLogin(bodyData);
});

async function SendLogin(bodyData) {
    await RequestType("reg");
    await ServerRequest(bodyData);
}

function CreateJSON (username, password) {
    return {
        name: username,
        password: password,
    }
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
        alert("alreadyexits");
    }
}

