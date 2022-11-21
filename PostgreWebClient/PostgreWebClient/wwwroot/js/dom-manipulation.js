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
        let schemaRoot = createCaret(schema.name);
        let schemaUl = createNestedList();
        schemaRoot.appendChild(schemaUl);
        listRoot.appendChild(schemaRoot);
        
        let tablesRoot = createCaret("Tables");
        schemaUl.appendChild(tablesRoot);
        let tablesUl = createNestedList();
        tablesRoot.appendChild(tablesUl);
        
        let viewsRoot = createCaret("Views");
        schemaUl.appendChild(viewsRoot);
        let viewsUl = createNestedList();
        viewsRoot.appendChild(viewsUl);
        
        if (schema.tables !== null && schema.tables.length !== 0) {

            schema.tables.forEach(table => {
                let li = document.createElement("li");
                li.ondblclick = event => {
                    editor.session.setValue(`SELECT * FROM ${schema.name}.${table};`);
                    sendQuery(event);
                };
                li.classList.add("table-name");
                li.textContent = table;
                tablesUl.appendChild(li);
            });
        }
        
        if (schema.views !== null && schema.views.length !== 0) {
            schema.views.forEach(view => {
                let li = document.createElement("li");
                li.ondblclick = event => {
                    editor.session.setValue(`SELECT * FROM ${schema.name}.${view};`);
                    sendQuery(event);
                };
                li.classList.add("table-name");
                li.textContent = view;
                viewsUl.appendChild(li);
            })
        }
        
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

function createNestedList() {
    let ul = document.createElement("ul");
    ul.classList.add("nested");
    return ul;
}

function createCaret(name) {
    let li = document.createElement("li");
    li.innerHTML = `<span class="caret">${name}</span>`
    
    return li;
}

function createPaginationButtons(paginationModel) {
    let queryResult = document.getElementById("queryResult");

    queryResult.querySelectorAll(".btn").forEach(el => queryResult.removeChild(el));
    
    if (hasPreviousPage(paginationModel)) {
        let button = createBtn("Previous", () => paginationModel.currentPage--);        
        queryResult.appendChild(button);
    }
    
    if (hasNextPage(paginationModel)) {
        let button = createBtn("Next", () => paginationModel.currentPage++);
        queryResult.appendChild(button);
    }
}

function createBtn(text, func) {
    let button = document.createElement("button");
    button.addEventListener("click", ev =>  {
        func();
        sendQuery(ev);
    })
    button.classList.add("btn", "m-2", "btn-dark");
    button.textContent = text;
    
    return button;
}