"use strict"

const url = "http://192.168.1.8:8181"

var inputForm = document.getElementById("inputForm");

inputForm.addEventListener("submit", (e)=>{
    e.preventDefault()
    const data = new FormData(e.target);
    const bodyData = CreateJSON(data.get("Username"), data.get("Email"), data.get("Password"), data.get("MCusername"))
    ServerRequest(bodyData);
});

function CreateJSON (username, email, password, mcusername) {
    return {
        name: username,
        email: email,
        password: password,
        mcusername: mcusername,
    }
}

async function ServerRequest(bodyData) {
    let response = await fetch(url, {
        method: 'POST',
        headers: {
        'Content-Type': 'application/json'
        },
        body: JSON.stringify(bodyData),
    });
    let text = await response.text();
    console.log(text);
}

