"use strict"

const url = "http://127.0.0.1:8181"

var inputForm = document.getElementById("inputForm");

inputForm.addEventListener("submit", (e)=>{
    e.preventDefault();
    const data = new FormData(e.target);
    const bodyData = CreateRegJSON(data.get("Username"), data.get("Email"), data.get("Password"), data.get("MCusername"))
    SendReg(bodyData);
});

async function SendReg(bodyData) {
    await RequestType("reg");
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
        alert("alreadyexits");
    }
}

function CreateRegJSON (username, email, password, mcusername) {
    return {
        name: username,
        email: email,
        password: password,
        mcusername: mcusername,
    }
}