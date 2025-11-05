using SGE.Application.DTOs;
using SGE.Domain.Contracts;
using SGE.Domain.Entities;

namespace SGE.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    public async Task<PagedResult<EmployeeListItemDto>> GetEmployeesAsync(string? query = null, int page = 1, int pageSize = 10, string orderBy = "FullName", bool desc = false)
    {
        var employees = await _employeeRepository.GetAllAsync(query, page, pageSize, orderBy, desc);
        var totalCount = await _employeeRepository.GetTotalCountAsync(query);

        var items = employees.Select(e => new EmployeeListItemDto
        {
            Id = e.Id,
            FullName = e.FullName,
            Role = e.Role,
            HireDate = e.HireDate,
            Salary = e.Salary,
            Department = e.Department?.Name ?? string.Empty
        });

        return new PagedResult<EmployeeListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<EmployeeListItemDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null) return null;

        return new EmployeeListItemDto
        {
            Id = employee.Id,
            FullName = employee.FullName,
            Role = employee.Role,
            HireDate = employee.HireDate,
            Salary = employee.Salary,
            Department = employee.Department?.Name ?? string.Empty
        };
    }

    public async Task<EmployeeListItemDto> CreateEmployeeAsync(EmployeeCreateDto dto)
    {
        // Validar que el departamento exista
        if (!await _departmentRepository.ExistsAsync(dto.DepartmentId))
        {
            throw new ArgumentException($"El departamento con ID {dto.DepartmentId} no existe");
        }

        var employee = new Employee
        {
            FullName = dto.FullName,
            HireDate = dto.HireDate,
            Role = dto.Role,
            Salary = dto.Salary,
            DepartmentId = dto.DepartmentId
        };

        var created = await _employeeRepository.CreateAsync(employee);
        var department = await _departmentRepository.GetByIdAsync(created.DepartmentId);

        return new EmployeeListItemDto
        {
            Id = created.Id,
            FullName = created.FullName,
            Role = created.Role,
            HireDate = created.HireDate,
            Salary = created.Salary,
            Department = department?.Name ?? string.Empty
        };
    }

    public async Task<EmployeeListItemDto> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Empleado con ID {id} no encontrado");
        }

        // Validar que el departamento exista
        if (!await _departmentRepository.ExistsAsync(dto.DepartmentId))
        {
            throw new ArgumentException($"El departamento con ID {dto.DepartmentId} no existe");
        }

        employee.FullName = dto.FullName;
        employee.HireDate = dto.HireDate;
        employee.Role = dto.Role;
        employee.Salary = dto.Salary;
        employee.DepartmentId = dto.DepartmentId;

        var updated = await _employeeRepository.UpdateAsync(employee);
        var department = await _departmentRepository.GetByIdAsync(updated.DepartmentId);

        return new EmployeeListItemDto
        {
            Id = updated.Id,
            FullName = updated.FullName,
            Role = updated.Role,
            HireDate = updated.HireDate,
            Salary = updated.Salary,
            Department = department?.Name ?? string.Empty
        };
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        if (!await _employeeRepository.ExistsAsync(id))
        {
            throw new KeyNotFoundException($"Empleado con ID {id} no encontrado");
        }

        return await _employeeRepository.DeleteAsync(id);
    }
}

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(d => new DepartmentDto
        {
            Id = d.Id,
            Name = d.Name
        });
    }
}