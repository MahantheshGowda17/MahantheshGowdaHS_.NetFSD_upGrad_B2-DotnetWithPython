const storageService = {

    // Headers with JWT
    _headers() {
        return {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + authService.getToken()
        };
    },

    // GET ALL (Pagination supported via params)
    async getEmployees(page = 1, pageSize = 5, search = '', department = '', status = '') {

        let url = `${CONFIG.API_BASE_URL}/employees?page=${page}&pageSize=${pageSize}`;

        if (search) url += `&search=${search}`;
        if (department) url += `&department=${department}`;
        if (status) url += `&status=${status}`;

        const res = await fetch(url, {
            headers: this._headers()
        });

        return await res.json();
    },

    // GET BY ID
    async getEmployeeById(id) {
        try {
            const res = await fetch(
                `${CONFIG.API_BASE_URL}/employees/${id}`,
                { headers: this._headers() }
            );

            if (!res.ok) return null;

            return await res.json();

        } catch (err) {
            console.error("Error fetching employee:", err);
            return null;
        }
    },

    // ADD
    async addEmployee(employee) {
        try {
            const res = await fetch(`${CONFIG.API_BASE_URL}/employees`, {
                method: "POST",
                headers: this._headers(),
                body: JSON.stringify(employee)
            });

            if (!res.ok) throw new Error("Add failed");

            return await res.json();

        } catch (err) {
            console.error("Error adding employee:", err);
            return null;
        }
    },

    // UPDATE
    async updateEmployee(id, employee) {
        try {
            const res = await fetch(`${CONFIG.API_BASE_URL}/employees/${id}`, {
                method: "PUT",
                headers: this._headers(),
                body: JSON.stringify(employee)
            });

            if (!res.ok) throw new Error("Update failed");

            return await res.json();

        } catch (err) {
            console.error("Error updating employee:", err);
            return null;
        }
    },

    // DELETE
    async deleteEmployee(id) {
        try {
            const res = await fetch(`${CONFIG.API_BASE_URL}/employees/${id}`, {
                method: "DELETE",
                headers: this._headers()
            });

            return res.ok;

        } catch (err) {
            console.error("Error deleting employee:", err);
            return false;
        }
    },

    //  DASHBOARD
    async getDashboard() {
        try {
            const res = await fetch(`${CONFIG.API_BASE_URL}/employees/dashboard`, {
                headers: this._headers()
            });

            if (!res.ok) return null;

            return await res.json();

        } catch (err) {
            console.error("Dashboard fetch error:", err);
            return null;
        }
    },
    async getAllEmployeesForDropdown() {
        try {
            const res = await fetch(
                `${CONFIG.API_BASE_URL}/employees?page=1&pageSize=1000`,
                { headers: this._headers() }
            );

            const result = await res.json();

            return result.data || [];

        } catch (err) {
            console.error("Dropdown fetch error:", err);
            return [];
        }
    }
};