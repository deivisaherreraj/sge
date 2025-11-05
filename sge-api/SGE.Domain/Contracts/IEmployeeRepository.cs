using SGE.Domain.Entities;

namespace SGE.Domain.Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync(string? query = null, int page = 1, int pageSize = 10, string orderBy = "FullName", bool desc = false);
    Task<int> GetTotalCountAsync(string? query = null);
    Task<Employee?> GetByIdAsync(int id);
    Task<Employee> CreateAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(int id);
}