"use strict"

const url = "http://192.168.1.8:8181"

var inputForm = document.getElementById("inputForm");

inputForm.addEventListener("submit", (e)=>{
    e.preventDefault()
    const data = new FormData(e.target);
    const bodyData = {
        name: data.get("Username"),
        email: data.get("Email"),
        num: +data.get("Number"),
        password: data.get("Password"),
    }
    ServerRequest(bodyData);
});

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

