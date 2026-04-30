// app.js

import { signup, login } from "./authService.js";
import { addEmployee, getEmployees, updateEmployee, deleteEmployee } from "./employeeService.js";
import { getDashboardStats } from "./dashboardService.js";
import { showToast, switchToDashboard, switchToLogin, renderTable, updateStats } from "./uiService.js";

let editIndex = -1;

// AUTH
window.handleSignup = function () {
    let res = signup(signupUsername.value, signupPassword.value, confirmPassword.value);

    if (res !== "SUCCESS") return showToast(res);

    showToast("Signup successful");
    switchToLogin();
};

window.handleLogin = function () {
    let res = login(loginUsername.value, loginPassword.value);

    if (res !== "SUCCESS") return showToast(res);

    loadDashboard();
};

// DASHBOARD
function loadDashboard() {
    switchToDashboard();
    refreshUI();
}

// EMPLOYEE
window.handleSaveEmployee = function () {
    let emp = {
        name: empName.value,
        email: empEmail.value,
        phone: empPhone.value,
        department: empDepartment.value,
        salary: empSalary.value,
        status: empStatus.value
    };

    let res;

    if (editIndex === -1) {
        res = addEmployee(emp);
    } else {
        res = updateEmployee(editIndex, emp);
        editIndex = -1;
    }

    if (res !== "SUCCESS") return showToast(res);

    showToast("Saved successfully");
    refreshUI();
};

// DELETE
window.deleteEmployee = function (index) {
    deleteEmployee(index);
    refreshUI();
};

// EDIT
window.editEmployee = function (index) {
    let emp = getEmployees()[index];

    empName.value = emp.name;
    empEmail.value = emp.email;
    empPhone.value = emp.phone;
    empDepartment.value = emp.department;
    empSalary.value = emp.salary;
    empStatus.value = emp.status;

    editIndex = index;
};

// REFRESH UI
function refreshUI() {
    let employees = getEmployees();

    renderTable(employees);

    let stats = getDashboardStats(employees);
    updateStats(stats);
}