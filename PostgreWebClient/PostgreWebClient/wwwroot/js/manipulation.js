"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/manipHub").build();

let sendButton = document.getElementById("sendButton");

connection.start().then(function () {
    sendButton.disabled = false;
}).catch(logError);

connection.on("getTable", createQueryResultTable);
connection.on("getDatabaseInfo", createDbInfoList);

sendButton.addEventListener("click", function (event) {
    let queryText = editor.session.getValue();
    connection.invoke("ExecuteQuery", queryText).catch(logError);
    connection.invoke("GetDatabaseInfo").catch(logError);
    event.preventDefault();
})

function logError(err) {
    return console.error(err.toString());
}