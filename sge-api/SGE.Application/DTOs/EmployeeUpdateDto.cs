using System.ComponentModel.DataAnnotations;

namespace SGE.Application.DTOs;

public class EmployeeUpdateDto
{
    [Required(ErrorMessage = "El nombre completo es requerido")]
    [StringLength(120, ErrorMessage = "El nombre no puede exceder 120 caracteres")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de contratación es requerida")]
    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "El cargo es requerido")]
    [StringLength(80, ErrorMessage = "El cargo no puede exceder 80 caracteres")]
    public string Role { get; set; } = string.Empty;

    [Required(ErrorMessage = "El salario es requerido")]
    [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser mayor o igual a 0")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "El departamento es requerido")]
    public int DepartmentId { get; set; }
}