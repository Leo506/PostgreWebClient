"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl("/manipHub").build();

let sendButton = document.getElementById("sendButton");

let paginationModel = new PaginationModel();

connection.start().then(function () {
    sendButton.disabled = false;
    let sessionId = getCookieByName("session_id");
    getDatabaseInfo(sessionId);
}).catch(logError);

connection.on("getTable", (table, pagination) => {
    createQueryResultTable(table);
    paginationModel.totalCount = pagination.totalCount;
    createPaginationButtons(paginationModel);
});
connection.on("getDatabaseInfo", createDbInfoList);

sendButton.addEventListener("click", sendQuery);



function executeQuery(sessionId, pagination) {
    let queryText = editor.session.getValue();
    connection.invoke("ExecuteQuery", queryText, sessionId, pagination).catch(logError);
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

function sendQuery(event) {
    let sessionId = getCookieByName("session_id");
    executeQuery(sessionId, paginationModel)
    getDatabaseInfo(sessionId);

    event.preventDefault();
}