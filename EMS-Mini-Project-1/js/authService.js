const authService = {

    // 🔐 LOGIN (API)
    async login(username, password) {

        try {
            const res = await fetch(`${CONFIG.API_BASE_URL}/auth/login`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ username, password })
            });

            // ❗ check response first
            if (!res.ok) {
                return { success: false, errors: { general: "Invalid credentials" } };
            }

            const data = await res.json();

            // ❗ safe handling
            if (!data || !data.token) {
                return { success: false, errors: { general: data?.message || "Login failed" } };
            }

            // ✅ Save session
            sessionStorage.setItem("token", data.token);
            sessionStorage.setItem("user", data.username || username);
            sessionStorage.setItem("role", data.role || "Viewer");

            return { success: true, message: "Login successful!" };

        } catch (err) {
            console.error("Login error:", err);
            return { success: false, errors: { general: "Server error" } };
        }
    },

    // 🔐 SIGNUP (API)
    async signup(username, password) {

        try {
            const res = await fetch(`${CONFIG.API_BASE_URL}/auth/register`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ username, password })
            });

            const data = await res.json();

            if (!res.ok) {
                return { success: false, errors: { general: data?.message || "Signup failed" } };
            }

            return { success: true, message: "Signup successful! Please login." };

        } catch (err) {
            console.error("Signup error:", err);
            return { success: false, errors: { general: "Server error" } };
        }
    },

    // 🔐 TOKEN
    getToken() {
        return sessionStorage.getItem("token");
    },

    // 🔐 AUTH CHECK
    isLoggedIn() {
        return !!sessionStorage.getItem("token");
    },

    getCurrentUser() {
        return sessionStorage.getItem("user");
    },

    getRole() {
        return sessionStorage.getItem("role");
    },

    // 🔐 LOGOUT
    logout() {
        sessionStorage.removeItem("token");
        sessionStorage.removeItem("user");
        sessionStorage.removeItem("role");
    }
};


// Export for Node.js tests
if (typeof module !== "undefined") {
    module.exports = authService;
}