function createQueryResultTable(table) {
    
    // Clear result table
    let rootTable = document.getElementById("resultTable");
    while (rootTable.lastElementChild) rootTable.removeChild(rootTable.lastElementChild);

    // Create table head
    let tHead = document.createElement("thead");
    rootTable.appendChild(tHead);

    table.columns.forEach(column => {
        let head = document.createElement("th");
        head.textContent = column.toString();
        tHead.appendChild(head);
    });

    // Create table body
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
}

function createDbInfoList(info) {
    let listRoot = document.getElementById("databaseInfo");
    while (listRoot.lastElementChild) listRoot.removeChild(listRoot.lastElementChild);
    
    info.result.schemas.forEach(schema => {
        let schemaSpan = document.createElement("span");
        schemaSpan.classList.add("caret");
        schemaSpan.textContent = schema.name;

        let schemaListItem = document.createElement("li");
        schemaListItem.appendChild(schemaSpan);

        listRoot.appendChild(schemaListItem);
        
        if (schema.tables === undefined || schema.tables.length === 0) return;
        
        let internalList = document.createElement("ul");
        internalList.classList.add("nested");
        schemaListItem.appendChild(internalList);
        
        schema.tables.forEach(table => {
            let li = document.createElement("li");
            li.classList.add("table-name");
            li.textContent = table;
            internalList.appendChild(li);
        });
        
        
    });
    
    setupSchemaList();
}

function setupSchemaList() {
    let toggles = document.getElementsByClassName("caret");
    for (let i = 0; i < toggles.length; i++) {
        toggles[i].addEventListener("click", function () {
            console.log("You click");
            this.parentElement.querySelector(".nested").classList.toggle("active");
            this.classList.toggle("caret-down");
        })
    }
}