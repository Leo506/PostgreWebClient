"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/manipHub").build();

let sendButton = document.getElementById("sendButton");

connection.start().then(function () {
    sendButton.disabled = false;
    let sessionId = getCookieByName("session_id");
    getDatabaseInfo(sessionId);
}).catch(logError);

connection.on("getTable", createQueryResultTable);
connection.on("getDatabaseInfo", createDbInfoList);

sendButton.addEventListener("click", function (event) {
    let sessionId = getCookieByName("session_id");
    executeQuery(sessionId)
    getDatabaseInfo(sessionId);
    event.preventDefault();
});

function executeQuery(sessionId) {
    let queryText = editor.session.getValue();
    connection.invoke("ExecuteQuery", queryText, sessionId).catch(logError);
}

function getDatabaseInfo(sessionId) {
    connection.invoke("GetDatabaseInfo", sessionId).catch(logError);
}

function getCookieByName(name) {
    name += "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let cookieArr = decodedCookie.split(";");
    let result;
    cookieArr.forEach(c => {
        if (c.indexOf(name) === 0) result = c.substring(name.length);
    });
    return result;
}

function logError(err) {
    return console.error(err.toString());
}