let count;
let pageSize = 5;
let contacts = [];
let currentPage = 1;
class Contact {
    constructor(id, name, dateOfBirth, married, phone, salary) {
        this.id = id;
        this.name = name;
        this.dateOfBirth = dateOfBirth;
        this.married = married;
        this.phone = phone;
        this.salary = salary;
    }
}
class GetDataRequest {
    constructor(sort, pagination, filters) {
        this.sort = sort;
        this.pagination = pagination;
        this.filters = filters;
    }
}

class Pagination {
    constructor(page, count) {
        this.page = page;
        this.count = count;
    }
}

class Filter {
    constructor(by, value) {
        this.by = by;
        this.value = value;
    }
}

class SortInfo {
    constructor(by, order) {
        this.by = by;
        this.order = order;
    }
}

const Order = {
    Asc: 'Asc',
    Desc: 'Desc'
};

addInitEventListeners();

const sort = null;
const pagination = new Pagination(currentPage, pageSize);
const filters = [];
const request = new GetDataRequest(sort, pagination, filters);
await fetchAndRender(request);






function addPaginationButtons() {

    const pagination = document.querySelector(".pagination");
    pagination.innerHTML = "";
    const pageCount = Math.ceil(count / pageSize);
    for (let i = 1; i <= pageCount; i++) {
        const a = document.createElement("a");
        a.innerText = i;
        a.href = "#";
        if (i == currentPage) {
            a.classList.add("page-active");
        }
        a.addEventListener("click", async function () {
            const { filters, sort } = getFiltersAndSort();
            const pagination = new Pagination(i, pageSize);
            const request = new GetDataRequest(sort, pagination, filters);
            currentPage = i;
            await fetchAndRender(request);
        });
        pagination.appendChild(a);
    }
}



function getFiltersAndSort() {
    const sortByElement = document.querySelector("input[name='sort-by']:checked");
    const sortOrder = document.querySelector("input[name='sort-order']:checked").value === Order.Asc ? 0 : 1;
    let sort = null;

    if (sortByElement) {
        sort = new SortInfo(sortByElement.value, sortOrder);
    }
    const filters = [];
    const name = document.querySelector("#filter-name").value;
    if (name) {
        filters.push(new Filter("Name", name));
    }
    const dateOfBirth = document.querySelector("#filter-date-of-birth").value;
    if (dateOfBirth) {
        filters.push(new Filter("DateOfBirth", dateOfBirth));
    }

    const married = document.querySelector("input[name='married']:checked");
    switch (married.value) {
        case "yes":
            filters.push(new Filter("Married", 'true'));
            break;
        case "no":
            filters.push(new Filter("Married", 'false'));
            break;
    }

    const phone = document.querySelector("#filter-phone").value;
    if (phone) {
        filters.push(new Filter("Phone", phone));
    }
    const salary = document.querySelector("#filter-salary").value;
    if (salary) {
        filters.push(new Filter("Salary", salary));
    }
    return { filters, sort };
}
async function getData(request) {
    console.log(request);
    const response = await fetch("/contacts/data", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(request)
    });
    const data = await response.json();
    console.log(data);
    return data;
}

function renderContacts() {
    const table = document.querySelector("#contactsTable tbody");
    table.innerHTML = "";
    contacts.forEach(contact => {
        const tr = document.createElement("tr");
        tr.setAttribute("data-id", contact.id);
        tr.innerHTML = `
                  <td>
                      <input value="${contact.name}" data-id="${contact.id}" data-field="name"/>
                  </td>
                  <td>
                      <input type="date" value="${contact.dateOfBirth}" data-id="${contact.id}" data-field="dateOfBirth" />
                  </td>
                  <td>
                      <input type="checkbox" ${contact.married ? "checked" : ""} data-id="${contact.id}" data-field="married" />
                  </td>
                  <td>
                      <input value="${contact.phone}" data-id="${contact.id}" data-field="phone" />
                  </td>
                  <td>
                      <input type="number" value="${contact.salary}" data-id="${contact.id}" data-field="salary" />
                  </td>
                  <th>
                      <button class="inactive" data-id="${contact.id}" data-action="save" id="save-button-${contact.id}">Save</button>
                      <button class="delete" id="delete-button-${contact.id}" data-action="delete" data-id="${contact.id}">Delete</button>
                  </th>
              `;
        table.appendChild(tr);
    });
    addEventListenersToContacts();


}

function addInitEventListeners() {
    document.querySelectorAll("input[name='sort-by']").forEach(input => {
        input.addEventListener("change", async function () {
            const { filters, sort } = getFiltersAndSort();
            const pagination = new Pagination(1, pageSize);
            const request = new GetDataRequest(sort, pagination, filters);
            await fetchAndRender(request);
        });
    });
    document.querySelectorAll("input[name='sort-order']").forEach(input => {
        input.addEventListener("change", async function () {
            const { filters, sort } = getFiltersAndSort();
            const pagination = new Pagination(1, pageSize);
            const request = new GetDataRequest(sort, pagination, filters);
            await fetchAndRender(request);
        });
    });
    document.querySelectorAll(".filters input").forEach(input => {
        input.addEventListener("change", async function () {
            const { filters, sort } = getFiltersAndSort();
            const pagination = new Pagination(1, pageSize);
            const request = new GetDataRequest(sort, pagination, filters);
            await fetchAndRender(request);
        });
    });
    document.querySelector("input[type='file']").addEventListener("change", function () {
        const button = document.querySelector("button[type='submit']");
        button.classList.remove("inactive");
        button.classList.add("active");
    });
    document.querySelector("#pageSize").addEventListener("change", async function () {
        pageSize = parseInt(this.value);
        const { filters, sort } = getFiltersAndSort();
        const pagination = new Pagination(1, pageSize);
        const request = new GetDataRequest(sort, pagination, filters);
        await fetchAndRender(request);
    });
}

async function fetchAndRender(request) {
    const response = await getData(request);
    currentPage = request.pagination.page;
    contacts = response.contacts;
    count = response.totalCount;
    renderContacts();
    addPaginationButtons();
}


function addEventListenersToContacts() {
    document.querySelectorAll("#contactsTable input").forEach(input => {
        input.addEventListener("change", function () {
            console.log("Changed");
            const id = this.getAttribute("data-id");
            const saveButton = document.querySelector(`button[data-id='${id}'][data-action='save']`);
            saveButton.classList.remove("inactive");
            saveButton.classList.add("active");
        });
    });
    document.querySelectorAll("button[data-action=save]").forEach(button => {
        button.addEventListener("click", async function () {
            const id = parseInt(this.getAttribute("data-id"));
            const name = document.querySelector(`input[data-id='${id}'][data-field='name']`).value;
            const dateOfBirth = new Date(document.querySelector(`input[data-id='${id}'][data-field='dateOfBirth']`).value).toISOString().split("T")[0];
            const married = document.querySelector(`input[data-id='${id}'][data-field='married']`).checked;
            const phone = document.querySelector(`input[data-id='${id}'][data-field='phone']`).value;
            const salary = parseFloat(document.querySelector(`input[data-id='${id}'][data-field='salary']`).value);
            const contact = {
                id,
                name,
                dateOfBirth,
                married,
                phone,
                salary
            };
            console.log(contact);
            this.classList.remove("active");
            this.classList.add("inactive");
            try {
                const res = await fetch(`/contacts/${id}`, {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(contact)
                });
                const errors = await res.json();
                console.log(errors);
                if (res.status === 400) {
                    showErrorMessage(id, errors);
                }
            } catch (err) {
                console.error(err);
            }
        });
    });
    function showErrorMessage(id, errors) {
        const row = document.querySelector(`tr[data-id='${id}']`);
        const rowWithMessage = document.createElement("tr");
        const content = errors.map(error => `<div>${error.errorMessage}</div>`).join("");
        const closeMessageButton = document.createElement("button");
        closeMessageButton.innerText = "X";
        closeMessageButton.addEventListener("click", function () {
            rowWithMessage.remove();
        });

        rowWithMessage.innerHTML = `<td colspan="5">${content}</td>`;
        rowWithMessage.appendChild(closeMessageButton);
        row.parentNode.insertBefore(rowWithMessage, row);
    }

    document.querySelectorAll("button[data-action=delete]").forEach(button => {
        button.addEventListener("click", async function () {
            const id = parseInt(this.getAttribute("data-id"));
            await fetch(`/contacts/${id}`, {
                method: "DELETE"
            });
            document.querySelector(`tr[data-id='${id}']`).remove();
            console.log("Deleted");
        });
    });
}