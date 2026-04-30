let _employees = [];

const employeeService = {

    async loadAll() {

        const result = await storageService.getEmployees();

    
        _employees = result.data ? result.data : result;

        return _employees;
    },

    getAll() {
        return [..._employees];
    },

    getById(id) {
        return _employees.find(e => e.id === id);
    },

    getDepartments() {
        return [...new Set(_employees.map(e => e.department))];
    },

    applyFilters(employees, search, department, status) {
        return employees.filter(emp => {
            const fullName = (emp.firstName + " " + emp.lastName).toLowerCase();

            return (!search || fullName.includes(search.toLowerCase())) &&
                   (!department || emp.department === department) &&
                   (!status || emp.status === status);
        });
    },

    sort(employees, sortBy) {
        if (!sortBy) return employees;

        return [...employees].sort((a, b) => {
            if (sortBy === "name-asc") return a.firstName.localeCompare(b.firstName);
            if (sortBy === "name-desc") return b.firstName.localeCompare(a.firstName);

            if (sortBy === "salary-asc") return a.salary - b.salary;
            if (sortBy === "salary-desc") return b.salary - a.salary;

            if (sortBy === "date-asc") return new Date(a.joinDate) - new Date(b.joinDate);
            if (sortBy === "date-desc") return new Date(b.joinDate) - new Date(a.joinDate);

            return 0;
        });
    },

    async add(emp) {
        await storageService.addEmployee(emp);
        return this.loadAll();
    },

    async update(id, emp) {
        await storageService.updateEmployee(id, emp);
        return this.loadAll();
    },

    async remove(id) {
        await storageService.deleteEmployee(id);
        return this.loadAll();
    }
};
if (typeof module !== "undefined") {
    module.exports = employeeService;
}