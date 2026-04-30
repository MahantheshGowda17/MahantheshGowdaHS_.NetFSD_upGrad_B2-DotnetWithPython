// dashboardService.js

export function getDashboardStats(employees) {
    let total = employees.length;

    let active = employees.filter(e => e.status === "Active").length;
    let inactive = employees.filter(e => e.status === "Inactive").length;

    let departments = new Set(employees.map(e => e.department)).size;

    return { total, active, inactive, departments };
}