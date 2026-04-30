using EMS.API.Data;
using EMS.API.Models;
using EMS.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace EMS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _service;
        private readonly AppDbContext _context;

        public EmployeesController(EmployeeService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        //GET:api/employees (with pagination + filters)
        [HttpGet]
        [Authorize(Roles = "Admin,Viewer")]
        public async Task<IActionResult> GetAll(
    int page = 1,
    int pageSize = 5,
    string? search = null,
    string? department = null,
    string? status = null,
    string sortBy = "name",
    string sortDir = "asc")
        {
            var query = _context.Employees
                .AsNoTracking()
                .AsQueryable();

            // SEARCH
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e =>
                    e.FirstName.Contains(search) ||
                    e.LastName.Contains(search) ||
                    e.Email.Contains(search));
            }

            // FILTER
            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(e => e.Department == department);
            }
            // STATUS FILTER
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(e => e.Status == status);
            }
            // SORT
            sortBy = string.IsNullOrWhiteSpace(sortBy) ? "name" : sortBy.ToLower();
            sortDir = sortDir?.ToLower() == "desc" ? "desc" : "asc";

            query = sortBy switch
            {
                "salary" => sortDir == "desc"
                    ? query.OrderByDescending(e => e.Salary).ThenByDescending(e => e.Id)
                    : query.OrderBy(e => e.Salary).ThenBy(e => e.Id),

                "joindate" => sortDir == "desc"
                    ? query.OrderByDescending(e => e.JoinDate).ThenByDescending(e => e.Id)
                    : query.OrderBy(e => e.JoinDate).ThenBy(e => e.Id),

                _ => sortDir == "desc"
                    ? query.OrderByDescending(e => e.FirstName)
                           .ThenByDescending(e => e.LastName)
                           .ThenByDescending(e => e.Id)
                    : query.OrderBy(e => e.FirstName)
                           .ThenBy(e => e.LastName)
                           .ThenBy(e => e.Id)
            };

            //  COUNT
            var totalCount = await query.CountAsync();

            // PAGINATION
            var employees = await query
                .OrderBy(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                data = employees
            });
        } 

        //  GET: api/employees/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Viewer")]
        public async Task<IActionResult> GetById(int id)
        {
            var emp = await _service.GetByIdAsync(id);

            if (emp == null)
                return NotFound();

            return Ok(emp);
        }

        //  POST: api/employees
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] Employee emp)
        {
            try
            {
                await _service.AddAsync(emp);
                return Ok(emp);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/employees/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee emp)
        {
            try
            {
                await _service.UpdateAsync(id, emp);
                return Ok(new { message = "Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //  api/employees/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // api/employees/dashboard
        [HttpGet("dashboard")]
        [Authorize(Roles = "Admin,Viewer")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _service.GetDashboardAsync();
            return Ok(result);
        }
    }
}
