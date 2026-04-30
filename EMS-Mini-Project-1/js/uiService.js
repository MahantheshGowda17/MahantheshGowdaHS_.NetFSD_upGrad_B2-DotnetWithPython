// uiService.js

export function showToast(message) {
    document.getElementById("toastText").innerText = message;
    new bootstrap.Toast(document.getElementById("toastMsg")).show();
}

export function switchToDashboard() {
    signupSection.classList.add("d-none");
    loginSection.classList.add("d-none");
    dashboardSection.classList.remove("d-none");
}

export function switchToLogin() {
    dashboardSection.classList.add("d-none");
    signupSection.classList.add("d-none");
    loginSection.classList.remove("d-none");
}

export function renderTable(employees) {
    let table = document.getElementById("employeeTable");
    table.innerHTML = "";

    employees.forEach((e, i) => {
        table.innerHTML += `
        <tr>
            <td>${e.name}</td>
            <td>${e.email}</td>
            <td>${e.department}</td>
            <td>${e.status}</td>
            <td>
                <button onclick="window.editEmployee(${i})">Edit</button>
                <button onclick="window.deleteEmployee(${i})">Delete</button>
            </td>
        </tr>`;
    });
}

export function updateStats(stats) {
    totalEmployees.innerText = stats.total;
    activeEmployees.innerText = stats.active;
    inactiveEmployees.innerText = stats.inactive;
    departments.innerText = stats.departments;
}