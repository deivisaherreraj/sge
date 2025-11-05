namespace SGE.Application.DTOs;

public class EmployeeListItemDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public decimal Salary { get; set; }
    public string Department { get; set; } = string.Empty;
}