using EMS.API.Models;
using EMS.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EMS.API.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo)
        {
            _repo = repo;
        }

        public async Task<object> GetAllAsync(
            string? search,
            string? department,
            string? status,
            string sortBy = "name",
            string sortDir = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var query =  _repo.GetQueryableAsync();

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e =>
                    (e.FirstName + " " + e.LastName).ToLower().Contains(search.ToLower()) ||
                    e.Email.ToLower().Contains(search.ToLower()));
            }

            // FILTER
            if (!string.IsNullOrEmpty(department))
                query = query.Where(e => e.Department == department);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(e => e.Status == status);

            // SORT
            query = sortBy.ToLower() switch
            {
                "salary" => sortDir == "desc"
                    ? query.OrderByDescending(e => e.Salary)
                    : query.OrderBy(e => e.Salary),

                "joindate" => sortDir == "desc"
                    ? query.OrderByDescending(e => e.JoinDate)
                    : query.OrderBy(e => e.JoinDate),

                _ => sortDir == "desc"
                    ? query.OrderByDescending(e => e.LastName)
                           .ThenByDescending(e => e.FirstName)
                    : query.OrderBy(e => e.LastName)
                           .ThenBy(e => e.FirstName)
            };

            // PAGINATION
            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                data,
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                hasNextPage = page * pageSize < totalCount,
                hasPrevPage = page > 1
            };
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(Employee emp)
        {
            if (await _repo.EmailExistsAsync(emp.Email))
                throw new Exception("Email already exists");

            emp.CreatedAt = DateTime.UtcNow;
            emp.UpdatedAt = DateTime.UtcNow;

            await _repo.AddAsync(emp);
        }

        public async Task UpdateAsync(int id, Employee emp)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) throw new Exception("Not found");

            if (await _repo.EmailExistsAsync(emp.Email, id))
                throw new Exception("Email already exists");

            existing.FirstName = emp.FirstName;
            existing.LastName = emp.LastName;
            existing.Email = emp.Email;
            existing.Phone = emp.Phone;
            existing.Department = emp.Department;
            existing.Designation = emp.Designation;
            existing.Salary = emp.Salary;
            existing.JoinDate = emp.JoinDate;
            existing.Status = emp.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var emp = await _repo.GetByIdAsync(id);
            if (emp == null) throw new Exception("Not found");

            await _repo.DeleteAsync(emp);
        }

        public async Task<object> GetDashboardAsync()
        {
            var query = _repo.GetQueryableAsync();

            // TOTAL COUNT
            var totalEmployees = await query.CountAsync();

            //  ACTIVE / INACTIVE
            var active = await query.CountAsync(e => e.Status == "Active");
            var inactive = await query.CountAsync(e => e.Status == "Inactive");

            // DEPARTMENT BREAKDOWN
            var departmentsRaw = await query
                .GroupBy(e => e.Department)
                .Select(g => new
                {
                    department = g.Key,
                    count = g.Count()
                })
                .OrderBy(x => x.department)
                .ToListAsync();

            // CALCULATE PERCENTAGE 
            var departments = departmentsRaw.Select(d => new
            {
                d.department,
                d.count,
                percentage = totalEmployees == 0 ? 0 :
                Math.Round((d.count * 100.0) / totalEmployees,1)
            });

            // RECENT EMPLOYEES
            var recentEmployees = await query
                .OrderByDescending(e => e.CreatedAt)
                .ThenByDescending(e => e.Id)
                .Take(5)
                .ToListAsync();

            return new
            {
                totalEmployees,
                active,
                inactive,
                departments,
                recentEmployees
            };
        }
    }
}

