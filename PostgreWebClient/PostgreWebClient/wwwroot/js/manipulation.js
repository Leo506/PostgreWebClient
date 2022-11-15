"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/manipHub").build();

let sendButton = document.getElementById("sendButton");

connection.start().then(function () {
    sendButton.disabled = false;
    getDatabaseInfo();
}).catch(logError);

connection.on("getTable", createQueryResultTable);
connection.on("getDatabaseInfo", createDbInfoList);

sendButton.addEventListener("click", function (event) {
    executeQuery()
    getDatabaseInfo();
    event.preventDefault();
});



function executeQuery() {
    let queryText = editor.session.getValue();
    connection.invoke("ExecuteQuery", queryText).catch(logError);
}

function getDatabaseInfo() {
    connection.invoke("GetDatabaseInfo").catch(logError);
}

function logError(err) {
    return console.error(err.toString());
}