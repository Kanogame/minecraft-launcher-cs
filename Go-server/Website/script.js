"use strict"

const url = "http://192.168.1.8:8181"

/*
inputForm.addEventListener("submit", (e)=>{
    e.preventDefault()
};
*/

function SendRequest(method, url, body = null) {
    return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();

        xhr.open(method, url)
        xhr.responseType = "json"
        xhr.setRequestHeader("Content-Type", "application/json")

        xhr.onload = () => {
            if (xhr.status >= 400) { 
                reject(xhr.response)
            } else {
                resolve(xhr.response)
            }
        }

        xhr.onerror = () => {
            reject(xhr.response)
        }

        xhr.send(JSON.stringify(body))
    })
}

const body = {
    name: "KA",
    age: 63
}

SendRequest("POST", url, body)
.then(data => console.log(data))
.catch(error => console.log(error))

SendRequest("GET", url)
.then(data => console.log(data))
.catch(error => console.log(error))