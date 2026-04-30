let _state = {
    page: 1,
    pageSize: CONFIG.PAGE_SIZE,
    search: "",
    department: "",
    status: ""
};

$(document).ready(function () {
    sessionStorage.clear();
    setupEventListeners();
    checkAuthState();
});



let sortState = {
    name: 'asc',
    salary: 'asc',
    date: 'asc'
};

// ================= SETUP =================
async function setupEventListeners() {

    // Auth
    $("#showSignup, #showLogin").click(toggleAuthForms);
    $("#loginForm, #signupForm").submit(handleAuthForms);
    $("#logoutBtn").click(handleLogout);

    // Navigation
    $('.nav-link[data-view]').click(handleNavigation);

    // Add Employee
    $(document).on('click', '#addEmployeeNavBtn, #addEmployeeDashboardBtn, #addEmployeeListBtn', function () {
        uiService.clearEmployeeForm();
        new bootstrap.Modal($('#employeeModal')[0]).show();
    });

    // VIEW
    $(document).on('click', '.view-btn', function () {
        const id = parseInt($(this).data('id'));
        uiService.showViewModal(employeeService.getById(id));
    });

    // EDIT
    $(document).on('click', '.edit-btn', function () {
        const id = parseInt($(this).data('id'));
        const emp = employeeService.getById(id);

        $('#modalTitle').text('Edit Employee');
        $('#submitEmployeeBtn').text('Update Employee');

        uiService.populateEmployeeForm(emp);
        new bootstrap.Modal($('#employeeModal')[0]).show();

        window.currentEditingId = id;
    });

    // DELETE
    $(document).on('click', '.delete-btn', function () {
        const id = parseInt($(this).data('id'));
        const emp = employeeService.getById(id);

        $('#deleteConfirmText').text(`Delete "${emp.firstName} ${emp.lastName}"?`);
        new bootstrap.Modal($('#deleteModal')[0]).show();

        window.deletingId = id;
    });

    $('#employeeForm').submit(handleEmployeeForm);
    $('#confirmDeleteBtn').click(handleDeleteConfirm);

    // Filters
    $('#searchInput').on('input', function () {
        _state.search = $(this).val();
        _state.page = 1;              // 🔥 RESET PAGE
        refreshEmployeeList();
    });

    $('#departmentFilter').on('change', function () {

        const val = $(this).val();

        _state.department = (val === "all" || val === "") ? "" : val;

        _state.page = 1;
        refreshEmployeeList();
    });

    $('input[name="statusFilter"]').on('change', function () {

        const val = $('input[name="statusFilter"]:checked').val();

        // 🔥 FIX
        if (val === "all") {
            _state.status = "";
        } else {
            _state.status = val;
        }

        _state.page = 1;
        refreshEmployeeList();
    });
    // Sorting
    $(document).on('click', '.sort-icon', async function () {

        const type = $(this).data('sort');
        let sortKey = '';

        if (type === 'name') {
            sortKey = sortState.name === 'asc' ? 'name-desc' : 'name-asc';
            sortState.name = sortState.name === 'asc' ? 'desc' : 'asc';
        }

        if (type === 'salary') {
            sortKey = sortState.salary === 'asc' ? 'salary-desc' : 'salary-asc';
            sortState.salary = sortState.salary === 'asc' ? 'desc' : 'asc';
        }

        if (type === 'date') {
            sortKey = sortState.date === 'asc' ? 'date-desc' : 'date-asc';
            sortState.date = sortState.date === 'asc' ? 'desc' : 'asc';
        }

        const employees = employeeService.getAll();
        const sorted = employeeService.sort(employees, sortKey);

        uiService.renderEmployeeTable(sorted);
    });
}

// ================= AUTH =================
async function checkAuthState() {

    if (authService.isLoggedIn()) {

        $('#loginSection, #signupSection').addClass('d-none');
        $('#dashboardSection').removeClass('d-none');

        $('#loggedInUser').text(authService.getCurrentUser());

        $('#dashboardView').removeClass('d-none');
        $('#employeeListView').addClass('d-none');

        // 🔥 LOAD DATA FIRST
        await employeeService.loadAll();

        loadDepartments();      // 🔥 FIX
        uiService.renderDashboard();
        uiService.applyRoleUI();

    } else {
        $('#dashboardSection').addClass('d-none');
        $('#loginSection').removeClass('d-none');
    }
}

async function handleNavigation(e) {
    e.preventDefault();

    const view = $(this).data('view');

    $('.nav-link').removeClass('active');
    $(this).addClass('active');

    $('#dashboardView, #employeeListView').addClass('d-none');

    if (view === 'dashboard') {
        $('#dashboardView').removeClass('d-none');
        uiService.renderDashboard();
    }

    if (view === 'employees') {
        $('#employeeListView').removeClass('d-none');

        await loadDepartments();   // 🔥 FIX
        refreshEmployeeList();
    }
}

function toggleAuthForms(e) {
    e.preventDefault();

    const targetId = e.target.id === 'showSignup' ? 'signupSection' : 'loginSection';

    $('#loginSection, #signupSection').addClass('d-none');
    $('#' + targetId).removeClass('d-none');
}

// ================= LOGIN =================
async function handleAuthForms(e) {
    e.preventDefault();

    const isLogin = $(e.currentTarget).attr('id') === 'loginForm';

    if (isLogin) {

        const username = $('#loginUsername').val();
        const password = $('#loginPassword').val();

        const result = await authService.login(username, password);

        if (result.success) {
            $('#loginError').addClass('d-none');
            uiService.showToast(result.message);

            await checkAuthState();   // 🔥 FIX

        } else {
            $('#loginError')
                .removeClass('d-none')
                .text(result.errors?.general || result.message);
        }

    } else {

        const username = $('#signupUsername').val();
        const password = $('#signupPassword').val();

        const result = await authService.signup(username, password);

        if (result.success) {
            uiService.showToast(result.message);
        } else {
            uiService.showFieldErrors(result.errors, '#signupForm');
        }
    }
}

function handleLogout() {
    authService.logout();
    checkAuthState();
}

// ================= EMPLOYEE LIST =================
async function refreshEmployeeList() {

    const result = await storageService.getEmployees(_state.page,
        _state.pageSize,
        _state.search,
        _state.department,
        _state.status); // API returns paged result

    const employees = result.data || result;

    // const filtered = employeeService.applyFilters(
    //     employees,
    //     _state.search,
    //     _state.department,
    //     _state.status
    // );

    // const sorted = employeeService.sort(filtered, null);

    uiService.renderEmployeeTable(employees);
    $('#employeeCount').text(
        `${(_state.page - 1) * _state.pageSize + 1} - ${(_state.page - 1) * _state.pageSize + employees.length} of ${result.totalCount}`
    );

    uiService.renderPagination(result);   // 🔥 ADD THIS
}

// ================= DEPARTMENTS =================
async function loadDepartments() {

    const employees = await storageService.getAllEmployeesForDropdown();

    const depts = [
        ...new Set(
            employees.map(e => e.department).filter(Boolean)
        )
    ];

    $('#departmentFilter').html(
        '<option value="">All Departments</option>' +
        depts.map(d => `<option value="${d}">${d}</option>`).join('')
    );
}
// ================= ADD / UPDATE =================
async function handleEmployeeForm(e) {
    e.preventDefault();

    const data = {
        firstName: $('#firstName').val().trim(),
        lastName: $('#lastName').val().trim(),
        email: $('#email').val().trim(),
        phone: $('#phone').val().trim(),
        department: $('#department').val(),
        designation: $('#designation').val().trim(),
        salary: parseFloat($('#salary').val()),
        joinDate: $('#joinDate').val(),
        status: $('#status').val()
    };

    if (window.currentEditingId) {
        await employeeService.update(window.currentEditingId, data);
    } else {
        await employeeService.add(data);
    }

    await employeeService.loadAll(_state.page, _state.pageSize);   // 🔥 FIX

    bootstrap.Modal.getInstance($('#employeeModal')[0]).hide();

    window.currentEditingId = null;

    await uiService.renderDashboard();
    refreshEmployeeList();
}

// ================= DELETE =================
async function handleDeleteConfirm() {

    if (!window.deletingId) return;

    await employeeService.remove(window.deletingId);
    await employeeService.loadAll();   // 🔥 FIX

    bootstrap.Modal.getInstance($('#deleteModal')[0]).hide();

    window.deletingId = null;

    uiService.renderDashboard();
    refreshEmployeeList();
}