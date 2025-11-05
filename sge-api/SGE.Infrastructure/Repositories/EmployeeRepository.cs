using Microsoft.EntityFrameworkCore;
using SGE.Domain.Contracts;
using SGE.Domain.Entities;
using SGE.Infrastructure.Persistence;

namespace SGE.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly SgeDbContext _context;

    public EmployeeRepository(SgeDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync(string? query = null, int page = 1, int pageSize = 10, string orderBy = "FullName", bool desc = false)
    {
        var queryable = _context.Employees.Include(e => e.Department).AsQueryable();

        // Búsqueda por ID o nombre
        if (!string.IsNullOrWhiteSpace(query))
        {
            if (int.TryParse(query, out int id))
            {
                queryable = queryable.Where(e => e.Id == id);
            }
            else
            {
                queryable = queryable.Where(e => e.FullName.ToLower().Contains(query.ToLower()));
            }
        }

        // Ordenamiento
        queryable = orderBy.ToLower() switch
        {
            "hiredate" => desc ? queryable.OrderByDescending(e => e.HireDate) : queryable.OrderBy(e => e.HireDate),
            _ => desc ? queryable.OrderByDescending(e => e.FullName) : queryable.OrderBy(e => e.FullName)
        };

        // Paginación
        return await queryable
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync(string? query = null)
    {
        var queryable = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            if (int.TryParse(query, out int id))
            {
                queryable = queryable.Where(e => e.Id == id);
            }
            else
            {
                queryable = queryable.Where(e => e.FullName.ToLower().Contains(query.ToLower()));
            }
        }

        return await queryable.CountAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Employee> CreateAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(employee.Id) ?? employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(employee.Id) ?? employee;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id);
    }
}

public class DepartmentRepository : IDepartmentRepository
{
    private readonly SgeDbContext _context;

    public DepartmentRepository(SgeDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments.OrderBy(d => d.Name).ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments.FindAsync(id);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Departments.AnyAsync(d => d.Id == id);
    }
}