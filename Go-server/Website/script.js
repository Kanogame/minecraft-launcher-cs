"use strict"

const url = "http://192.168.1.8:8181"

const bodyData = {
    name: "KA",
    age: 63
}

ServerRequest();

async function ServerRequest() {
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

