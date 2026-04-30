// validationService.js

export function validateSignup(username, password, confirmPassword) {
    if (!username || !password || !confirmPassword)
        return "All fields are required";

    if (password.length < 6)
        return "Password must be at least 6 characters";

    if (password !== confirmPassword)
        return "Passwords do not match";

    return null;
}

export function validateEmployee(emp) {
    if (!emp.name || !emp.email || !emp.phone || !emp.department || !emp.salary || !emp.status)
        return "All fields required";

    if (!/^\d{10}$/.test(emp.phone))
        return "Phone must be 10 digits";

    if (emp.salary <= 0)
        return "Invalid salary";

    return null;
}