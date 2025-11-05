using Moq;
using SGE.Application.DTOs;
using SGE.Application.Services;
using SGE.Domain.Contracts;
using SGE.Domain.Entities;
using Xunit;

namespace SGE.Tests;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepo;
    private readonly Mock<IDepartmentRepository> _mockDepartmentRepo;
    private readonly EmployeeService _service;

    public EmployeeServiceTests()
    {
        _mockEmployeeRepo = new Mock<IEmployeeRepository>();
        _mockDepartmentRepo = new Mock<IDepartmentRepository>();
        _service = new EmployeeService(_mockEmployeeRepo.Object, _mockDepartmentRepo.Object);
    }

    [Fact]
    public async Task GetEmployeesAsync_ReturnsPagedResult()
    {
        // Arrange
        var employees = new List<Employee>
        {
            new Employee 
            { 
                Id = 1, 
                FullName = "Test User", 
                Role = "Dev", 
                HireDate = DateTime.Now, 
                Salary = 5000, 
                DepartmentId = 1,
                Department = new Department { Id = 1, Name = "IT" }
            }
        };

        _mockEmployeeRepo.Setup(r => r.GetAllAsync(null, 1, 10, "FullName", false))
            .ReturnsAsync(employees);
        _mockEmployeeRepo.Setup(r => r.GetTotalCountAsync(null))
            .ReturnsAsync(1);

        // Act
        var result = await _service.GetEmployeesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ReturnsEmployee_WhenExists()
    {
        // Arrange
        var employee = new Employee
        {
            Id = 1,
            FullName = "Test User",
            Role = "Dev",
            HireDate = DateTime.Now,
            Salary = 5000,
            DepartmentId = 1,
            Department = new Department { Id = 1, Name = "IT" }
        };

        _mockEmployeeRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(employee);

        // Act
        var result = await _service.GetEmployeeByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test User", result.FullName);
    }

    [Fact]
    public async Task GetEmployeeByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        _mockEmployeeRepo.Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((Employee?)null);

        // Act
        var result = await _service.GetEmployeeByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateEmployeeAsync_ThrowsException_WhenDepartmentNotExists()
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            FullName = "New User",
            Role = "Dev",
            HireDate = DateTime.Now,
            Salary = 5000,
            DepartmentId = 999
        };

        _mockDepartmentRepo.Setup(r => r.ExistsAsync(999))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateEmployeeAsync(dto));
    }

    [Fact]
    public async Task CreateEmployeeAsync_CreatesEmployee_WhenValid()
    {
        // Arrange
        var dto = new EmployeeCreateDto
        {
            FullName = "New User",
            Role = "Dev",
            HireDate = DateTime.Now,
            Salary = 5000,
            DepartmentId = 1
        };

        var createdEmployee = new Employee
        {
            Id = 1,
            FullName = dto.FullName,
            Role = dto.Role,
            HireDate = dto.HireDate,
            Salary = dto.Salary,
            DepartmentId = dto.DepartmentId
        };

        _mockDepartmentRepo.Setup(r => r.ExistsAsync(1))
            .ReturnsAsync(true);
        _mockDepartmentRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Department { Id = 1, Name = "IT" });
        _mockEmployeeRepo.Setup(r => r.CreateAsync(It.IsAny<Employee>()))
            .ReturnsAsync(createdEmployee);

        // Act
        var result = await _service.CreateEmployeeAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New User", result.FullName);
        _mockEmployeeRepo.Verify(r => r.CreateAsync(It.IsAny<Employee>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_ThrowsException_WhenNotExists()
    {
        // Arrange
        _mockEmployeeRepo.Setup(r => r.ExistsAsync(999))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteEmployeeAsync(999));
    }
}