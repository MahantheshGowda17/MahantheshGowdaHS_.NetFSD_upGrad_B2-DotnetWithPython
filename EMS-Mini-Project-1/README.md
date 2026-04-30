# Name: Mahanthesh Gowda H S
# Batch: B2( uGE_Dotnet FSD with Python)
# Project: Employee Management System

##  Setup Instructions

### Backend

1. Open solution in Visual Studio
2. Configure SQL Server in `appsettings.json`
3. Run migrations:

   ```
   Update-Database
   ```
4. Run the API

### Frontend

1. Open `index.html`
2. Ensure API base URL is correct in `CONFIG`
3. Login and start using

##  API Endpoints

### Employees

* GET `/api/employees?page=1&pageSize=5`
* GET `/api/employees/{id}`
* POST `/api/employees`
* PUT `/api/employees/{id}`
* DELETE `/api/employees/{id}`

### Dashboard

* GET `/api/employees/dashboard`




