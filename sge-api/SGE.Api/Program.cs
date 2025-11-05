using Microsoft.EntityFrameworkCore;
using SGE.Application.Services;
using SGE.Domain.Contracts;
using SGE.Infrastructure.Persistence;
using SGE.Infrastructure.Repositories;
using SGE.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);
var cors = "_sgeCors";

// Servicios básicos de ASP.NET Core
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración CORS específica para frontend Ionic
builder.Services.AddCors(o => o.AddPolicy(
    cors,
    p => p.WithOrigins("http://localhost:8100") // Ionic
    .AllowAnyHeader()
    .AllowAnyMethod()
));

// ProblemDetails para manejo estandarizado de errores
builder.Services.AddProblemDetails(); // .NET 8

// Entity Framework Core - DbContext
builder.Services.AddDbContext<SgeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Registro de Repositorios (Scoped)
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

// Registro de Servicios de Aplicación (Scoped)
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

var app = builder.Build();

// Seed inicial de datos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SgeDbContext>();
    await DatabaseSeeder.SeedAsync(db);
}

// Pipeline de middleware
app.UseSwagger(); // Documentación OpenAPI
app.UseSwaggerUI(); // Interfaz interactiva

app.UseHttpsRedirection(); // Redirección HTTPS
app.UseCors(cors); // Políticas CORS

app.UseExceptionHandler(); // Manejo global de excepciones
app.MapControllers(); // Mapeo de controladores

app.Run();