using SGE.Application.DTOs;

namespace SGE.Application.Services;

public interface IEmployeeService
{
    Task<PagedResult<EmployeeListItemDto>> GetEmployeesAsync(string? query = null, int page = 1, int pageSize = 10, string orderBy = "FullName", bool desc = false);
    Task<EmployeeListItemDto?> GetEmployeeByIdAsync(int id);
    Task<EmployeeListItemDto> CreateEmployeeAsync(EmployeeCreateDto dto);
    Task<EmployeeListItemDto> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto);
    Task<bool> DeleteEmployeeAsync(int id);
}

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
}