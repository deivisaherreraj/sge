using Microsoft.AspNetCore.Mvc;
using SGE.Application.DTOs;
using SGE.Application.Services;

namespace SGE.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Obtiene una lista paginada de empleados con filtros opcionales
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<EmployeeListItemDto>>> GetEmployees(
        [FromQuery] string? query = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderBy = "FullName",
        [FromQuery] bool desc = false)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var result = await _employeeService.GetEmployeesAsync(query, page, pageSize, orderBy, desc);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene un empleado por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeListItemDto>> GetEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Empleado no encontrado",
                Detail = $"No se encontró el empleado con ID {id}",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(employee);
    }

    /// <summary>
    /// Crea un nuevo empleado
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EmployeeListItemDto>> CreateEmployee([FromBody] EmployeeCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var employee = await _employeeService.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Error de validación",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Actualiza un empleado existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeListItemDto>> UpdateEmployee(int id, [FromBody] EmployeeUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, dto);
            return Ok(employee);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Empleado no encontrado",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Error de validación",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    /// <summary>
    /// Elimina un empleado
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        try
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Empleado no encontrado",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }
}