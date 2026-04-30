const dashboardService = {

    getSummary() {
        const employees = employeeService.getAll();
        const active = employees.filter(e => e.status === 'Active').length;

        return {
            total: employees.length,
            active,
            inactive: employees.length - active,
            departments: new Set(employees.map(e => e.department)).size
        };
    },

    getDepartmentBreakdown() {
        const employees = employeeService.getAll();
        const total = employees.length;
        const breakdown = {};

        employees.forEach(emp => {
            const dept = emp.department || "Unknown"; 
            breakdown[dept] = (breakdown[dept] || 0) + 1;
        });

        return Object.keys(breakdown).map(dept => {
            const count = breakdown[dept];

            return {
                department: dept,
                count: count,
                percentage: total ? Math.round((count / total) * 1000) / 10 : 0
            };
        });
    },

    getRecentEmployees(limit = 5) {
        return [...employeeService.getAll()]
            .sort((a, b) => b.id - a.id)
            .slice(0, limit);
    }
};

if (typeof module !== "undefined") {
    module.exports = dashboardService;
}