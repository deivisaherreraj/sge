# SGE API - Backend

API REST desarrollada en .NET 8 para el Sistema de Gestión de Empleados.

## 🏗️ Arquitectura

Implementa **Clean Architecture** con separación clara de responsabilidades:

```
┌─────────────────────────────────────────────┐
│                SGE.Api                      │
│          (Presentation Layer)               │
│     Controllers │ Program.cs │ CORS         │
└─────────────────┬───────────────────────────┘
                  │ depends on
┌─────────────────▼───────────────────────────┐
│             SGE.Application                 │
│          (Application Layer)                │
│        Services │ DTOs │ Logic              │
└─────────────────┬───────────────────────────┘
                  │ depends on
┌─────────────────▼───────────────────────────┐
│              SGE.Domain                     │
│            (Domain Layer)                   │
│        Entities │ Contracts                 │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│           SGE.Infrastructure                │
│         (Infrastructure Layer)              │
│     EF Core │ Repositories │ Seed           │
└─────────────────────────────────────────────┘
```

## 🚀 Ejecución Local

### Prerrequisitos
- .NET 8 SDK
- SQL Server (LocalDB o Docker)

### Pasos

1. **Restaurar dependencias:**
   ```powershell
   dotnet restore
   ```

2. **Instalar EF Tools (si es necesario):**
   ```powershell
   dotnet tool restore
   # o globalmente:
   dotnet tool install --global dotnet-ef
   ```

3. **Crear base de datos y migraciones:**
   ```powershell
   # Desde la raíz del repo:
   pwsh -NoProfile -ExecutionPolicy Bypass -File ..\scripts\init-db.ps1
   ```

4. **Ejecutar la API:**
   ```powershell
   dotnet run --project .\SGE.Api\SGE.Api.csproj
   ```

**URLs:**
- Swagger: https://localhost:5001/swagger
- API Base: https://localhost:5001/api

## 🔧 Variables de Entorno

La API respeta las siguientes variables de entorno:

| Variable | Descripción | Valor por defecto |
|----------|-------------|-------------------|
| `ConnectionStrings__Default` | Cadena de conexión a SQL Server | Ver appsettings.json |
| `ASPNETCORE_URLS` | URLs donde escucha la API | http://+:5000 |
| `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución | Development |

### Ejemplo Docker:
```yaml
environment:
  - ASPNETCORE_URLS=http://+:5000
  - ConnectionStrings__Default=Server=sqlserver,1433;Database=SGE;User Id=sa;Password=Tu_Password;TrustServerCertificate=True
```

## 🔒 CORS

Por defecto, la API permite requests desde:
- `http://localhost:8100` (Ionic dev server)

Configurado en `Program.cs`:
```csharp
builder.Services.AddCors(o => o.AddPolicy(
    cors,
    p => p.WithOrigins("http://localhost:8100")
    .AllowAnyHeader()
    .AllowAnyMethod()
));
```

## 🧪 Testing

```powershell
# Ejecutar todos los tests
dotnet test

# Con cobertura
dotnet test /p:CollectCoverage=true

# Solo tests del proyecto específico
dotnet test .\SGE.Tests\SGE.Tests.csproj
```

## 📊 Endpoints Principales

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/employees` | Lista empleados (con paginación) |
| GET | `/api/employees/{id}` | Obtiene empleado específico |
| POST | `/api/employees` | Crea nuevo empleado |
| PUT | `/api/employees/{id}` | Actualiza empleado |
| DELETE | `/api/employees/{id}` | Elimina empleado |
| GET | `/api/departments` | Lista departamentos |

**Ejemplo de búsqueda con filtros:**
```
GET /api/employees?searchTerm=ana&departmentId=1&page=1&pageSize=10&sortBy=firstName&sortDesc=false
```

## 🐛 Troubleshooting

### Error de conexión a SQL Server
```
dotnet user-secrets set "ConnectionStrings:Default" "Server=localhost,1433;Database=SGE;..."
```

### EF Tools no encontradas
```powershell
dotnet tool restore
# o instalar globalmente:
dotnet tool install --global dotnet-ef
```

### Puerto ocupado
Cambiar puerto en `appsettings.json` o usar variable de entorno `ASPNETCORE_URLS`.