// storageService.js

const USER_KEY = "users";
const EMPLOYEE_KEY = "employees";

export function saveUsers(users) {
    localStorage.setItem(USER_KEY, JSON.stringify(users));
}

export function loadUsers() {
    let data = localStorage.getItem(USER_KEY);
    return data ? JSON.parse(data) : [];
}

export function saveEmployees(employees) {
    localStorage.setItem(EMPLOYEE_KEY, JSON.stringify(employees));
}

export function loadEmployees() {
    let data = localStorage.getItem(EMPLOYEE_KEY);
    return data ? JSON.parse(data) : [];
}