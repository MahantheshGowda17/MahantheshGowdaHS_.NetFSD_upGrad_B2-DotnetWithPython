// employeeService.js

import { loadEmployees, saveEmployees } from "./storageService.js";
import { validateEmployee } from "./validationService.js";

export function getEmployees() {
    return loadEmployees();
}

export function addEmployee(emp) {
    let error = validateEmployee(emp);
    if (error) return error;

    let employees = loadEmployees();

    if (employees.find(e => e.email === emp.email))
        return "Email already exists";

    employees.push(emp);
    saveEmployees(employees);

    return "SUCCESS";
}

export function updateEmployee(index, updatedEmp) {
    let employees = loadEmployees();

    let error = validateEmployee(updatedEmp);
    if (error) return error;

    let emailExists = employees.find((e, i) => e.email === updatedEmp.email && i !== index);
    if (emailExists) return "Email already exists";

    employees[index] = updatedEmp;
    saveEmployees(employees);

    return "SUCCESS";
}

export function deleteEmployee(index) {
    let employees = loadEmployees();

    employees.splice(index, 1);
    saveEmployees(employees);

    return employees;
}