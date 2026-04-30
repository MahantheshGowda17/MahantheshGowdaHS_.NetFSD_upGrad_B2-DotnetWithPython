const uiService = {

    getDepartmentColor(department) {
        const colors = {
            "Engineering": "bg-primary",
            "Marketing": "bg-warning",
            "HR": "bg-info",
            "Finance": "bg-success",
            "Operations": "bg-secondary"
        };

        return colors[department] || "bg-secondary";
    },

    // ================= DASHBOARD =================
    async renderDashboard() {

        const data = await storageService.getDashboard();

        if (!data) return;

        $('#totalEmployees').text(data.totalEmployees || 0);
        $('#activeEmployees').text(data.active || 0);
        $('#inactiveEmployees').text(data.inactive || 0);
        $('#totalDepartments').text(data.departments?.length || 0);

        // Departments
        if (!data.departments?.length) {
            $('#departmentBreakdown').html(
                '<div class="text-center text-muted py-3">No data available</div>'
            );
        } else {
            $('#departmentBreakdown').html(`
                <table class="table align-middle mb-0">
                    <thead class="text-muted small">
                        <tr>
                            <th>Department</th>
                            <th>Count</th>
                            <th style="width:50%">Distribution</th>
                            <th>%</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${data.departments.map(item => `
                            <tr>
                                <td>
                                    <span class="badge rounded-pill px-3 py-2 ${this.getDepartmentColor(item.department)}">
                                        ${item.department}
                                    </span>
                                </td>
                                <td class="fw-semibold">${item.count}</td>
                                <td>
                                    <div class="progress" style="height:8px;">
                                        <div class="progress-bar ${this.getDepartmentColor(item.department)}"
                                         style="width:${item.percentage || 0}%">
                                    </div>
                                    </div>
                                </td>
                                <td class="text-muted">${item.percentage || 0}%</td>
                            </tr>
                        `).join('')}
                    </tbody>
                </table>
            `);
        }

        // Recent employees
        $('#recentEmployees').html(
            (data.recentEmployees || []).map(emp => `
                <div class="d-flex justify-content-between align-items-center py-2 border-bottom" style="min-height:70px;">
                    <div class="d-flex align-items-center gap-3">
                        <img 
                            src="https://ui-avatars.com/api/?name=${emp.firstName}+${emp.lastName}&background=0D6EFD&color=fff&rounded=true"
                            width="45" height="45"
                            class="rounded-circle"
                        />
                        <div>
                            <div class="fw-semibold small">
                                ${emp.firstName} ${emp.lastName}
                            </div>
                            <small class="text-muted">${emp.designation}</small>
                        </div>
                    </div>

                    <div class="d-flex align-items-center gap-2">
                        <span class="badge ${this.getDepartmentColor(emp.department)}">
                            ${emp.department}
                        </span>
                        <span class="badge ${emp.status === 'Active' ? 'bg-success' : 'bg-danger'}">
                            ${emp.status}
                        </span>
                    </div>
                </div>
            `).join('') || '<div class="text-center text-muted py-2">No employees</div>'
        );
    },

    // ================= EMPLOYEE TABLE =================
    renderEmployeeTable(employees) {

        const tbody = $('#employeeTable tbody');
        tbody.empty();

        // $('#employeeCount').text(employees.length);

        if (!employees.length) {
            tbody.html('<tr><td colspan="9" class="text-center py-4">No employees found</td></tr>');
            return;
        }

        employees.forEach(emp => {
            tbody.append(`
                <tr>
                    <td>${'#'}${emp.id}</td>
                    <td>
                        <img 
                            src="https://ui-avatars.com/api/?name=${emp.firstName}+${emp.lastName}&background=0D6EFD&color=fff&rounded=true"
                            width="40" height="40"
                            class="rounded-circle"
                        />
                    </td>
                    <td>${emp.firstName} ${emp.lastName}</td>
                    <td>${emp.email}</td>
                    <td>
                        <span class="badge ${this.getDepartmentColor(emp.department)}">
                            ${emp.department}
                        </span>
                    </td>
                    <td>${emp.designation}</td>
                    <td>₹${(emp.salary || 0).toLocaleString()}</td>
                    <td>${emp.joinDate?.split('T')[0]}</td>
                    <td>
                        <span class="badge ${emp.status === 'Active' ? 'bg-success' : 'bg-danger'}">
                            ${emp.status}
                        </span>
                    </td>
                    <td>
                        <button class="btn btn-sm btn-info view-btn me-1" data-id="${emp.id}">
                            <i class="bi bi-eye"></i>
                        </button>
                        <button class="btn btn-sm btn-warning edit-btn me-1" data-id="${emp.id}">
                            <i class="bi bi-pencil-square"></i>
                        </button>
                        <button class="btn btn-sm btn-danger delete-btn" data-id="${emp.id}">
                            <i class="bi bi-trash"></i>
                        </button>
                    </td>
                </tr>
            `);
        });

        this.applyRoleUI();
    },

    // ================= PAGINATION =================
    renderPagination(result) {

        const container = $('#pagination');
        container.empty();

        if (!result || result.totalPages === 0) return;

        const currentPage = result.page;
        const totalPages = result.totalPages;

        // 🔥 FIX: calculate manually
        const hasPrevPage = currentPage > 1;
        const hasNextPage = currentPage < totalPages;

        // PREV
        const prevBtn = $(`
        <button class="btn btn-sm btn-outline-primary me-1" ${!hasPrevPage ? 'disabled' : ''}>
            Prev
        </button>
    `);

        prevBtn.click(() => {
            if (hasPrevPage) {
                _state.page--;
                refreshEmployeeList();
            }
        });

        container.append(prevBtn);

        // PAGE NUMBERS
        for (let i = 1; i <= totalPages; i++) {

            const btn = $(`
            <button class="btn btn-sm ${i === currentPage ? 'btn-primary' : 'btn-light'} me-1">
                ${i}
            </button>
        `);

            btn.click(() => {
                _state.page = i;
                refreshEmployeeList();
            });

            container.append(btn);
        }

        // NEXT
        const nextBtn = $(`
        <button class="btn btn-sm btn-outline-primary ms-1" ${!hasNextPage ? 'disabled' : ''}>
            Next
        </button>
    `);

        nextBtn.click(() => {
            if (hasNextPage) {
                _state.page++;
                refreshEmployeeList();
            }
        });

        container.append(nextBtn);
    },


    // ================= ROLE UI =================
    applyRoleUI() {

        const role = sessionStorage.getItem("role");

        if (role === "Admin") {
            $('#addEmployeeNavBtn, #addEmployeeDashboardBtn, #addEmployeeListBtn').show();
            $('.edit-btn, .delete-btn').show();
        } else {
            $('#addEmployeeNavBtn, #addEmployeeDashboardBtn, #addEmployeeListBtn').hide();
            $('.edit-btn, .delete-btn').hide();
        }
    },

    // ================= FORM =================
    populateEmployeeForm(emp) {
        $('#firstName').val(emp.firstName);
        $('#lastName').val(emp.lastName);
        $('#email').val(emp.email);
        $('#phone').val(emp.phone);
        $('#department').val(emp.department);
        $('#designation').val(emp.designation);
        $('#salary').val(emp.salary);
        $('#joinDate').val(emp.joinDate?.split('T')[0]);
        $('#status').val(emp.status);
    },

    clearEmployeeForm() {
        $('#employeeForm')[0].reset();
        $('.is-invalid').removeClass('is-invalid').next('.invalid-feedback').empty();
        $('#modalTitle').text('Add Employee');
        $('#submitEmployeeBtn').text('Save Employee');
    },

    showFieldErrors(errors, formSelector) {
        const form = $(formSelector);

        form.find('.form-control').removeClass('is-invalid');
        form.find('.invalid-feedback').text('');

        Object.keys(errors).forEach(key => {
            const input = form.find(`[name="${key}"]`);
            if (input.length) {
                input.addClass('is-invalid');
                input.closest('.mb-3').find('.invalid-feedback').text(errors[key]);
            }
        });
    },

    showViewModal(emp) {

        const avatarUrl = `https://ui-avatars.com/api/?name=${emp.firstName}+${emp.lastName}&background=0D6EFD&color=fff&rounded=true&size=100`;

        $('#viewContent').html(`
            <div class="text-center mb-3">
                <img src="${avatarUrl}" width="80" height="80" class="rounded-circle shadow-sm mb-2"/>
                <h5>${emp.firstName} ${emp.lastName}</h5>
                <small>${emp.designation}</small>
            </div>
        `);

        new bootstrap.Modal($('#viewModal')[0]).show();
    },

    showToast(message, type = 'success') {
        const toast = $('#authToast');
        toast.removeClass().addClass(`toast align-items-center text-bg-${type}`);
        toast.find('.toast-body').text(message);
        new bootstrap.Toast(toast[0]).show();
    }
};