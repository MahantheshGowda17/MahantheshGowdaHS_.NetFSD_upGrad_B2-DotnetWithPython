using EMS.API.Models;

namespace EMS.API.Repositories
{
    public interface IEmployeeRepository
    {
        IQueryable<Employee> GetQueryableAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task AddAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(Employee employee);

        Task<bool> EmailExistsAsync(string email, int? excludeId = null);


    }
}
