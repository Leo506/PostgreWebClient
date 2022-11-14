"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/manipHub").build();

let sendButton = document.getElementById("sendButton");

connection.start().then(function () {
    sendButton.disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("getTable", function (table) {
    let rootTable = document.getElementById("resultTable");
    while (rootTable.lastElementChild) rootTable.removeChild(rootTable.lastElementChild);
    
    let tHead = document.createElement("thead");
    rootTable.appendChild(tHead);
    
    table.columns.forEach(column => {
        let head = document.createElement("th");
        head.textContent = column.toString();
        tHead.appendChild(head);
    });
    
    let tBody = document.createElement("tbody");
    rootTable.appendChild(tBody);
    table.rows.forEach(row => {
        let tr = document.createElement("tr");
        row.forEach(function (element) {
            let td = document.createElement("td");
            td.textContent = element.toString();
            tr.appendChild(td);
        });
        tBody.appendChild(tr);
    });
});


console.log(sendButton);
sendButton.addEventListener("click", function (event) {
    connection.invoke("ExecuteQuery", "select 1").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
})