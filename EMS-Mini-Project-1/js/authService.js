// authService.js

import { loadUsers, saveUsers } from "./storageService.js";
import { validateSignup } from "./validationService.js";

export function signup(username, password, confirmPassword) {
    let error = validateSignup(username, password, confirmPassword);
    if (error) return error;

    let users = loadUsers();

    if (users.find(u => u.username === username))
        return "User already exists";

    users.push({ username, password });
    saveUsers(users);

    return "SUCCESS";
}

export function login(username, password) {
    let users = loadUsers();

    let user = users.find(u => u.username === username && u.password === password);

    if (!user) return "Invalid credentials";

    return "SUCCESS";
}