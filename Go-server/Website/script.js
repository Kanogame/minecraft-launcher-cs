"use strict"

const url = "http://176.65.35.172:8181"

var inputForm = document.getElementById("inputForm");
var loginForm = document.getElementById("loginForm");

loginForm.addEventListener("submit", (e)=>{})

inputForm.addEventListener("submit", (e)=>{
    e.preventDefault()
    const data = new FormData(e.target);
    const bodyData = CreateJSON(data.get("Username"), data.get("Email"), data.get("Password"), data.get("MCusername"))
    SendReg(bodyData);
});

async function SendReg(bodyData) {
    await RequestType("reg");
    await ServerRequest(bodyData);
}

function CreateJSON (username, email, password, mcusername) {
    return {
        name: username,
        email: email,
        password: password,
        mcusername: mcusername,
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

