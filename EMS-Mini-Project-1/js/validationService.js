const validationService = {
    validateEmployee(data) {
        const errors = {};
        
        if (!data.firstName?.trim()) errors.firstName = "First name required";
        if (!data.lastName?.trim()) errors.lastName = "Last name required";
        
        if (!data.email?.trim()) errors.email = "Email required";
        else if (!/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/.test(data.email)) errors.email = "Invalid email";
        else if (storageService.emailExists(data.email)) errors.email = "Email already exists";
        
        if (!data.phone?.trim()) errors.phone = "Phone required";
        else if (!/^\d{10}$/.test(data.phone)) errors.phone = "10-digit phone required";
        
        if (!data.department) errors.department = "Department required";
        if (!data.designation?.trim()) errors.designation = "Designation required";
        if (!data.salary || data.salary <= 0) errors.salary = "Valid salary required";
        if (!data.joinDate) errors.joinDate = "Join date required";
        if (!data.status) errors.status = "Status required";
        
        return errors; 
    },
    
    validateLogin(username, password) {
        const errors = {};
        if (!username?.trim()) errors.username = "Username required";
        if (!password?.trim()) errors.password = "Password required";
        return errors; 
    },
    
    validateSignup(username, password, confirmPassword) {
        const errors = {};
        
        if (!username?.trim()) errors.username = "Username required";
        if (!password?.trim()) errors.password = "Password required";
        if (password && password.length < 6) errors.password = "Password must be 6+ characters";

        if (!confirmPassword?.trim()) {
            errors.confirmPassword = "Confirm password required";
        } else if (password !== confirmPassword) {
            errors.confirmPassword = "Passwords don't match";
        }

        return errors; 
    }
};