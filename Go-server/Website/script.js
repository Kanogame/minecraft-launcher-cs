"use strict"

const url = "http://127.0.0.1:8181"

var inputForm = document.getElementById("inputForm");

inputForm.addEventListener("submit", (e)=>{
    e.preventDefault()

    const formdata = new FormData(inputForm)
    fetch(url,{
        method:"POST",
        body:formdata,
    }).then(
        response => response.text()
    ).then(
        (data) => {console.log(data);document.getElementById("serverMessageBox").innerHTML=data}
    ).catch(
        error => console.error(error)
    )
})