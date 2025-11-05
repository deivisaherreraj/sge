using Microsoft.EntityFrameworkCore;
using SGE.Domain.Entities;

namespace SGE.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(Persistence.SgeDbContext db)
    {
        if (!await db.Departments.AnyAsync())
        {
            db.Departments.AddRange(
                new Department { Name = "Recursos Humanos" },
                new Department { Name = "Finanzas" },
                new Department { Name = "Tecnología" },
                new Department { Name = "Operaciones" }
            );
            await db.SaveChangesAsync();
        }
        
        if (!await db.Employees.AnyAsync())
        {
            var tech = await db.Departments.FirstAsync(d => d.Name == "Tecnología");
            var rh = await db.Departments.FirstAsync(d => d.Name == "Recursos Humanos");
            var finanzas = await db.Departments.FirstAsync(d => d.Name == "Finanzas");
            var operaciones = await db.Departments.FirstAsync(d => d.Name == "Operaciones");
            
            db.Employees.AddRange(
                // Tecnología (6 empleados)
                new Employee { FullName = "Ana Pérez", HireDate = DateTime.UtcNow.AddYears(-3), Role = "Desarrollador Senior", Salary = 4500, DepartmentId = tech.Id },
                new Employee { FullName = "Luis Díaz", HireDate = DateTime.UtcNow.AddYears(-1), Role = "QA Tester", Salary = 3800, DepartmentId = tech.Id },
                new Employee { FullName = "Roberto Silva", HireDate = DateTime.UtcNow.AddYears(-4), Role = "Arquitecto de Software", Salary = 5200, DepartmentId = tech.Id },
                new Employee { FullName = "María González", HireDate = DateTime.UtcNow.AddMonths(-8), Role = "Desarrollador Junior", Salary = 3200, DepartmentId = tech.Id },
                new Employee { FullName = "Pedro Martínez", HireDate = DateTime.UtcNow.AddYears(-2), Role = "DevOps Engineer", Salary = 4800, DepartmentId = tech.Id },
                new Employee { FullName = "Laura Fernández", HireDate = DateTime.UtcNow.AddMonths(-6), Role = "Frontend Developer", Salary = 3900, DepartmentId = tech.Id },
                
                // Recursos Humanos (4 empleados)
                new Employee { FullName = "Marta Ríos", HireDate = DateTime.UtcNow.AddYears(-5), Role = "Gerente de RRHH", Salary = 4200, DepartmentId = rh.Id },
                new Employee { FullName = "Carlos Gómez", HireDate = DateTime.UtcNow.AddYears(-2), Role = "Especialista en Reclutamiento", Salary = 4000, DepartmentId = rh.Id },
                new Employee { FullName = "Sofia Morales", HireDate = DateTime.UtcNow.AddYears(-1), Role = "Analista de Compensaciones", Salary = 3700, DepartmentId = rh.Id },
                new Employee { FullName = "Diego Vargas", HireDate = DateTime.UtcNow.AddMonths(-10), Role = "Coordinador de Capacitación", Salary = 3500, DepartmentId = rh.Id },
                
                // Finanzas (3 empleados)
                new Employee { FullName = "Patricia López", HireDate = DateTime.UtcNow.AddYears(-3), Role = "Contador Senior", Salary = 4300, DepartmentId = finanzas.Id },
                new Employee { FullName = "Andrés Ruiz", HireDate = DateTime.UtcNow.AddYears(-1), Role = "Analista Financiero", Salary = 3900, DepartmentId = finanzas.Id },
                new Employee { FullName = "Elena Castro", HireDate = DateTime.UtcNow.AddMonths(-4), Role = "Asistente Contable", Salary = 3100, DepartmentId = finanzas.Id },
                
                // Operaciones (2 empleados)
                new Employee { FullName = "Miguel Torres", HireDate = DateTime.UtcNow.AddYears(-2), Role = "Supervisor de Operaciones", Salary = 4100, DepartmentId = operaciones.Id },
                new Employee { FullName = "Carmen Jiménez", HireDate = DateTime.UtcNow.AddMonths(-7), Role = "Coordinador Logístico", Salary = 3600, DepartmentId = operaciones.Id }
            );
            await db.SaveChangesAsync();
        }
    }
}